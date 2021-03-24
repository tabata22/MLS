using System;
using System.Threading.Tasks;

namespace MLS.EventPublisher.Common
{
    public interface IRabbitService : IDisposable
    {
        void InitializePublisher(string name, RabbitConfig config);
        Task PublishAsync(string messageId, string routingKey, string message);
        void WaitForConfirmsOrDie();
    }
}
