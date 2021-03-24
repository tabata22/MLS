using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace MLS.EventPublisher.Common
{
    public class RabbitService : IRabbitService
    {
        private RabbitConfig _rabbitConfig;
        private string _appName; 

        private IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _rabbitChannel;

        public void InitializePublisher(string name, RabbitConfig config)
        {
            _rabbitConfig = config;
            _appName = name;

            Prepare();
        }

        public Task PublishAsync(string messageId, string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var basicProperties = _rabbitChannel.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.MessageId = messageId;
            basicProperties.Type = routingKey;
            basicProperties.ContentType = "application/json";
            basicProperties.AppId = _appName;

            _rabbitChannel.BasicPublish(_rabbitConfig.TopicName, routingKey, false, basicProperties, body);

            return Task.CompletedTask;
        }

        public void WaitForConfirmsOrDie() => _rabbitChannel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
        
        private void Prepare()
        {
            if (_connection == null || !_connection.IsOpen)
                CreateConnection();

            if (_rabbitChannel != null && _rabbitChannel.IsOpen) 
                return;

            _rabbitChannel = _connection.CreateModel();
            _rabbitChannel.ConfirmSelect();

            _rabbitChannel.ExchangeDeclare(
                exchange: _rabbitConfig.TopicName,
                type: "topic",
                durable: true,
                autoDelete: false);

            _rabbitChannel.QueueDeclare(
                queue: _rabbitConfig.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false);

            _rabbitChannel.QueueBind(
                queue: _rabbitConfig.Queue,
                exchange: _rabbitConfig.TopicName,
                routingKey: _rabbitConfig.TopicName + ".#");
        }

        private void CreateConnection()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitConfig.HostName,
                Port = _rabbitConfig.Port,
                VirtualHost = _rabbitConfig.VirtualHost,
                UserName = _rabbitConfig.UserName,
                Password = _rabbitConfig.Password
            };

            _connection = _connectionFactory.CreateConnection(_rabbitConfig.ClientName);
        }

        public void Dispose()
        {
            if (_rabbitChannel != null)
            {
                _rabbitChannel?.Dispose();
                _rabbitChannel = null;
            }

            if (_connection != null)
            {
                _connection?.Dispose();
                _connection = null;
            }
        }
    }
}
