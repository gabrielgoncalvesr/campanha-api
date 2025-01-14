using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Service;
using Fidelicard.Campanha.Infra.Config;
using Fidelicard.Campanha.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();

        // Criar uma configuração fake para testes
        var inMemorySettings = new Dictionary<string, string>
        {
            { "ConnectionStrings:DBCampanha", "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;" }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Registrar o IConfiguration no DI
        services.AddSingleton<IConfiguration>(configuration);

        // Registrar os serviços e repositórios
        services.AddScoped<IDatabaseContext, DatabaseContext>();
        services.AddScoped<ICampanhaRepository, CampanhaRepository>();
        services.AddScoped<ICampanhaService, CampanhaService>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // Liberar recursos, se necessário
    }
}