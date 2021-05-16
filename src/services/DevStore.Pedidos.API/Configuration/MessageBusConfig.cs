using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevStore.Core.Utils;
using DevStore.MessageBus;
using DevStore.Pedidos.API.Services;

namespace DevStore.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PedidoOrquestradorIntegrationHandler>()
                .AddHostedService<PedidoIntegrationHandler>();
        }
    }
}