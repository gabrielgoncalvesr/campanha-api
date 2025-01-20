using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Repository;
using Fidelicard.Campanha.Core.Service;

namespace Fidelicard.Campanha.Configs
{
    public static class ServiceProviderFixture
    {

        public static IServiceCollection ServiceProviderFixtures(this IServiceCollection services)
        {
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
            //services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<ICampanhaRepository, CampanhaRepository>();
            services.AddScoped<ICampanhaService, CampanhaService>();

            // Configurar o banco de dados em memória, se necessário
            // services.AddDbContext<SeuDbContext>(options => options.UseInMemoryDatabase("TestDb"));

            //ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
