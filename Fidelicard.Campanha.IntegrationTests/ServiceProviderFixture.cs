using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Repository;
using Fidelicard.Campanha.Core.Service;
using Fidelicard.Campanha.Infra.Config;
using Fidelicard.Campanha.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fidelicard.Campanha.IntegrationTests
{
    public class ServiceProviderFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }

        public ServiceProviderFixture()
        {
            var services = new ServiceCollection();

            // Configurar logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            // Criar uma configuração fake para testes
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionStrings:DBCampanha", "Server=(localdb)\\mssqllocaldb;Database=FidelicardCampanhaTests;Trusted_Connection=True;MultipleActiveResultSets=true" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Registrar o IConfiguration no DI
            services.AddSingleton<IConfiguration>(configuration);

            // Registrar os serviços e repositórios
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<ICampanhaService, CampanhaService>();
            services.AddScoped<ICampanhaRepository>(sp => new MockCampanhaRepository());

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Liberar recursos, se necessário
            ServiceProvider?.Dispose();
        }
    }
}