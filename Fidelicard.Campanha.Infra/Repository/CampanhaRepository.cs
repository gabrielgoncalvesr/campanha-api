using AutoMapper;
using Dapper;
using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Fidelicard.Campanha.Infra.Config;
using Fidelicard.Campanha.Infra.EntityMapping.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Fidelicard.Campanha.Infra.Repository
{
    public class CampanhaRepository : BaseRepository, ICampanhaRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly string _connectionString;

        public CampanhaRepository(IDatabaseContext context,
            ILogger<CampanhaRepository> logger,
            IMapper mapper,
            IConfiguration configuration) : base(context)
        {
            _logger = logger;
            _mapper = mapper;
            _connectionString = configuration.GetSection("ConnectionStrings:DBCampanha").Value;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<CampanhaResult> ObterCampanhaAsync(int idCampanha)
        {
            try
            {
                using var connection = GetConnection();
                var query = @"
                            SELECT 
                                cam.Id,
                                cam.Nome,
                                cam.Descricao,
                                cam.Tipo,
                                cam.Regras,
                                cam.Premio,
                                cam.MaxParticipantes,
                                cam.QuantidadePontos,
                                cam.DataInicio,
                                cam.DataFim,
                                cam.Ativo,
                                cam.ImagemUrl,
                                cam.DataCadastro,
                                cam.DataAtualizacao,
                            FROM Campanha cam
                            WHERE usu.Id = @idCampanha";

                var campanhaDto = await connection.QueryFirstOrDefaultAsync<CampanhaDTO>(
                    sql: query,
                    param: new { IdCampanha = idCampanha }
                ).ConfigureAwait(false);

                if (campanhaDto == null)
                {
                    _logger.LogWarning("Usuário com Id {IdCampanha} não encontrado.", idCampanha);
                    return null;
                }

                return _mapper.Map<CampanhaResult>(campanhaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o campanha com Id {IdCampanha}.", idCampanha);
                throw;
            }
        }

        public async Task<IEnumerable<Campanhas>> ObterCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                using var connection = GetConnection();

                var query = @"
                            SELECT Id, Nome, DataInicio, DataFim, Ativo, DataCadastro, DataAtualizacao
                            FROM Campanhas
                            WHERE DataInicio >= @DataInicio AND DataFim <= @DataFim";

                var campanhas = await connection.QueryAsync<Campanhas>(
                    sql: query,
                    param: new { DataInicio = dataInicio, DataFim = dataFim }
                ).ConfigureAwait(false);

                return campanhas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter campanhas no período entre {DataInicio} e {DataFim}.", dataInicio, dataFim);
                throw;
            }
        }

        public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
        {
            var sql = @"
                      INSERT INTO [dbo].[Campanha]
                      (                          
                          cam.Nome,
                          cam.Descricao,
                          cam.Tipo,
                          cam.Regras,
                          cam.Premio,
                          cam.MaxParticipantes,
                          cam.QuantidadePontos,
                          cam.DataInicio,
                          cam.DataFim,
                          cam.Ativo,
                          cam.ImagemUrl,
                          cam.DataCadastro,
                          cam.DataAtualizacao
                      )                              
                      VALUES 
                      (                       
                          @Nome,
                          @Descricao,
                          @Tipo,
                          @Regras,
                          @Premio,
                          @MaxParticipantes,
                          @QuantidadePontos,
                          @DataInicio,
                          @DataFim,
                          true,
                          @ImagemUrl,
                          GETDATE(),
                          null
                       );
                      SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteScalarAsync<int>(
                            sql,
                            new
                            {
                                campanha.Nome,
                                campanha.Descricao,
                                campanha.Tipo,
                                campanha.Regras,
                                campanha.Premio,
                                campanha.MaxParticipantes,
                                campanha.QuantidadePontos,
                                campanha.DataInicio,
                                campanha.DataFim,
                                campanha.Ativo,
                                campanha.ImagemUrl,
                                campanha.DataCadastro,
                                campanha.DataAtualizacao
                            },
                            transaction
                        );

                        transaction.Commit();
                        return result;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Erro ao cadastrar campanha: {Nome}", campanha.Nome);
                        throw;
                    }
                }
            }
        }


        public async Task<int> AtualizarCampanhaAsync(Campanhas campanha)
        {
            const string sql = @"
                                UPDATE [dbo].[Campanha]
                                SET Nome = @Nome,
                                    Descricao = @Descricao,
                                    Tipo = @Tipo,
                                    Regras = @Regras,
                                    Premio = @Premio,
                                    MaxParticipantes = @MaxParticipantes,
                                    QuantidadePontos = @QuantidadePontos,
                                    DataInicio = @DataInicio,
                                    DataFim = @DataFim,
                                    Ativo = @Ativo,
                                    ImagemUrl = @ImagemUrl,
                                    DataAtualizacao = GETDATE()
                                WHERE Id = @IdCampanha";

            try
            {
                using var connection = GetConnection();
                connection.Open();

                var result = await connection.ExecuteAsync(sql, new
                {
                    IdCampanha = campanha.Id,
                    campanha.Nome,
                    campanha.Descricao,
                    campanha.Tipo,
                    campanha.Regras,
                    campanha.Premio,
                    campanha.MaxParticipantes,
                    campanha.QuantidadePontos,
                    campanha.DataInicio,
                    campanha.DataFim,
                    campanha.Ativo,
                    campanha.ImagemUrl,
                    campanha.DataCadastro,
                    campanha.DataAtualizacao
                }).ConfigureAwait(false);

                if (result == 0)
                {
                    throw new InvalidOperationException($"Nenhum registro encontrado para o IdUsuario: {campanha.Id}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o usuário com Id: {IdUsuario}", campanha.Id);
                throw;
            }
        }
    }
}
