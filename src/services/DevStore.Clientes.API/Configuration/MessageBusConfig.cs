using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevStore.Clientes.API.Services;
using DevStore.Core.Utils;
using DevStore.MessageBus;

namespace DevStore.Clientes.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}