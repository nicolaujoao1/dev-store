using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevStore.Catalogo.API.Services;
using DevStore.Core.Utils;
using DevStore.MessageBus;

namespace DevStore.Catalogo.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<CatalogoIntegrationHandler>();
        }
    }
}