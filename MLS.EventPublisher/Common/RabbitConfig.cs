namespace MLS.EventPublisher.Common
{
    public class RabbitConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string ClientName { get; set; }
        public string TopicName { get; set; }
        public string Queue { get; set; }
        public int BatchCount { get; set; }
    }
}
