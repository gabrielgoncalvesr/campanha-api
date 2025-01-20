using Dapper;
using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Fidelicard.Campanha.Core.Repository
{
    public class CampanhaRepository : ICampanhaRepository
    {
        private readonly string _connectionString;

        public CampanhaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Campanhas> ObterPorIdAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<Campanhas>(
                    "SELECT * FROM Campanhas WHERE Id = @Id",
                    new { Id = id });
            }
        }

        public async Task<IEnumerable<Campanhas>> ListarAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Campanhas>("SELECT * FROM Campanhas");
            }
        }

        public async Task<IEnumerable<Campanhas>> ListarAtivasAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Campanhas>(
                    "SELECT * FROM Campanhas WHERE Status = 'Ativa' AND DataFim >= GETDATE()");
            }
        }

        public async Task<int> CadastrarAsync(Campanhas campanha)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO Campanhas (Nome, EstabelecimentoId, DataInicio, DataFim, Status, DataCadastro) 
                           VALUES (@Nome, @EstabelecimentoId, @DataInicio, @DataFim, @Status, GETDATE());
                           SELECT CAST(SCOPE_IDENTITY() as int)";
                return await db.QuerySingleAsync<int>(sql, campanha);
            }
        }

        public async Task<bool> AtualizarAsync(Campanhas campanha)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE Campanhas 
                           SET Nome = @Nome, 
                               EstabelecimentoId = @EstabelecimentoId,
                               DataInicio = @DataInicio,
                               DataFim = @DataFim,
                               Status = @Status
                           WHERE Id = @Id";
                var result = await db.ExecuteAsync(sql, campanha);
                return result > 0;
            }
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Campanhas WHERE Id = @Id";
                var result = await db.ExecuteAsync(sql, new { Id = id });
                return result > 0;
            }
        }
    }
}
