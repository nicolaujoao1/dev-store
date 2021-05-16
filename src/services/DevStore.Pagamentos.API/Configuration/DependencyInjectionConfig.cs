using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DevStore.Pagamentos.API.Data;
using DevStore.Pagamentos.API.Data.Repository;
using DevStore.Pagamentos.API.Models;
using DevStore.Pagamentos.API.Services;
using DevStore.Pagamentos.CardAntiCorruption;
using DevStore.Pagamentos.Facade;
using DevStore.WebAPI.Core.Usuario;

namespace DevStore.Pagamentos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<PagamentosContext>();
        }
    }
}