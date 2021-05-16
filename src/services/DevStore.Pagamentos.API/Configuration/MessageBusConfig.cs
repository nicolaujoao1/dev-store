using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevStore.Core.Utils;
using DevStore.MessageBus;
using DevStore.Pagamentos.API.Services;

namespace DevStore.Pagamentos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}