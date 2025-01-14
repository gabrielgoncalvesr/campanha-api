using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Mock;
using Fidelicard.Campanha.Core.Service;
using Fidelicard.Campanha.Infra.Config;
using Fidelicard.Campanha.Infra.EntityMapping.AutoMapper;
using Fidelicard.Campanha.Infra.Repository;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fidelicard.Campanha.Configs
{
    public static class InjectionConfig
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ConfigureHttpClient(services, builder.Configuration);

            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<ICampanhaRepository, CampanhaRepository>();
            services.AddTransient<ICampanhaService, CampanhaService>();

            services.AddTransient<IMockCampanhaRepository, MockCampanhaRepository>();
            services.AddTransient<IMockCampanhaService, MockCampanhaService>();

            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddProfile(new AutoMapperProfile());
            }, typeof(AutoMapperProfile).Assembly);

            return services;
        }

        private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var config = configuration.GetSection("WebAPI.Services.Communication");
        }
    }
}