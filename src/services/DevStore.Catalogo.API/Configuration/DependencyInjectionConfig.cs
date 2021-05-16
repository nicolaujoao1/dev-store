using Microsoft.Extensions.DependencyInjection;
using DevStore.Catalogo.API.Data;
using DevStore.Catalogo.API.Data.Repository;
using DevStore.Catalogo.API.Models;

namespace DevStore.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}