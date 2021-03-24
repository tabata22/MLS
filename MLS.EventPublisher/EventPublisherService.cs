using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MLS.Data;
using MLS.EventPublisher.Common;

namespace MLS.EventPublisher
{
    public class EventPublisherService : BackgroundService
    {
        private readonly ILogger<EventPublisherService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private const string PublisherName = "LMS events publisher";

        public EventPublisherService(IServiceProvider serviceProvider, ILogger<EventPublisherService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            using var serviceScope = _serviceProvider.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var publisher = serviceProvider.GetService<IRabbitService>();
            var config = serviceProvider.GetService<RabbitConfig>();
            var unitOfWork = serviceProvider.GetService<IUnitOfWork>();

            PreparePublisher(publisher, config);

            while (!cancellationToken.IsCancellationRequested)
            {
                var loanEvents =
                    await unitOfWork.LoanEvents.FindAsync(p => !p.Processed, null, 0, config.BatchCount, cancellationToken);
                var sentCount = 0;

                foreach (var loan in loanEvents)
                {
                    await publisher.PublishAsync(loan.Id.ToString(), loan.Type, loan.Data);
                    sentCount++;
                }

                if (sentCount == 0)
                    continue;

                try
                {
                    publisher.WaitForConfirmsOrDie();

                    _logger.LogInformation($"{sentCount} messages sent to rabbit");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "can't publish messages");

                    continue;
                }

                try
                {
                    foreach (var loan in loanEvents)
                    {
                        loan.Processed = true;
                        await unitOfWork.LoanEvents.UpdateOneAsync(loan, cancellationToken);
                    }

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "can't save in database");
                    throw;
                }
            }
        }

        private void PreparePublisher(IRabbitService publisher, RabbitConfig config)
        {
            try
            {
                publisher.InitializePublisher(PublisherName, config);

                _logger.LogInformation("connection established successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "can't connect to rabbit");

                throw;
            }
        }
    }
}