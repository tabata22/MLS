using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace MLS.Elastic
{
    public static class ElasticExtensions
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionUrl = configuration["Elastic:Url"];
            if (string.IsNullOrWhiteSpace(connectionUrl))
                throw new ArgumentNullException("elastic url is empty or null");

            var defaultIndex = configuration["Elastic:Index"];
            var connectionSettings = new ConnectionSettings(new Uri(connectionUrl))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(connectionSettings);

            services.AddSingleton(client);
            services.AddSingleton<IElasticService, ElasticService>();
        }
    }
}
