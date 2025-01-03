using Fidelicard.Campanha.Infra.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Fidelicard.Campanha.Infra.Repository
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public DatabaseContext(IConfiguration config)
        {
            _connectionString = config.GetSection("ConnectionStrings:DBCampanha").Value;
        }
    }
}
