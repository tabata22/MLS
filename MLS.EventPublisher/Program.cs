using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MLS.Data;
using MLS.EventPublisher.Common;
using Serilog;

namespace MLS.EventPublisher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("publisher started...");

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[]? args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(opt =>
                {
                    opt.SetBasePath(Directory.GetCurrentDirectory());
                    opt.AddJsonFile("appsettings.json");
                    opt.AddCommandLine(args);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                    loggerConfiguration.Enrich.FromLogContext();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<LoanDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("AppConnectionString")));
                    services.AddSingleton(_ => hostContext.Configuration.GetSection("RabbitMQ").Get<RabbitConfig>());
                    services.AddSingleton<IRabbitService, RabbitService>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddHostedService<EventPublisherService>();
                });

            return host;
        }
    }
}
