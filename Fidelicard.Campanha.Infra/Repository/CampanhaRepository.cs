/*using AutoMapper;
using Dapper;
using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Fidelicard.Campanha.Infra.Config;
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

        //public async Task<CampanhaResult> ObterCampanhaAsync(int idCampanha)
        //{
        //    try
        //    {
        //        using var connection = GetConnection();
        //        var query = @"
        //                    SELECT 
        //                        cam.Id,
        //                        cam.Nome,
        //                        cam.Descricao,
        //                        cam.Tipo,
        //                        cam.Regras,
        //                        cam.Premio,
        //                        cam.MaxParticipantes,
        //                        cam.QuantidadePontos,
        //                        cam.DataInicio,
        //                        cam.DataFim,
        //                        cam.Ativo,
        //                        cam.ImagemUrl,
        //                        cam.DataCadastro,
        //                        cam.DataAtualizacao,
        //                    FROM Campanha cam
        //                    WHERE usu.Id = @idCampanha";

        //        var campanhaDto = await connection.QueryFirstOrDefaultAsync<CampanhaDTO>(
        //            sql: query,
        //            param: new { IdCampanha = idCampanha }
        //        ).ConfigureAwait(false);

        //        if (campanhaDto == null)
        //        {
        //            _logger.LogWarning("Usuário com Id {IdCampanha} não encontrado.", idCampanha);
        //            return null;
        //        }

        //        return _mapper.Map<CampanhaResult>(campanhaDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao obter o campanha com Id {IdCampanha}.", idCampanha);
        //        throw;
        //    }
        //}

        public async Task<CampanhaResult> ObterCampanhaAsync(int idCampanha)
        {
            await Task.Delay(100);

            try
            {
                var campanhaMock = new Campanhas
                {
                    Id = 1,
                    Nome = "Campanha Teste 1",
                    Descricao = "Descrição da Campanha 1",
                    Tipo = "Tipo 1",
                    Regras = "Regras de Exemplo",
                    Premio = "Prêmio Simulado",
                    MaxParticipantes = 100,
                    QuantidadePontos = 1000,
                    DataInicio = DateTime.Now.AddDays(-10),
                    DataFim = DateTime.Now.AddDays(10),
                    Ativo = 1,
                    ImagemUrl = "https://example.com/campanha1.jpg",
                    DataCadastro = DateTime.Now.AddDays(-30),
                    DataAtualizacao = DateTime.Now
                };

                if (idCampanha == 1)
                {
                    return CampanhaResult.SucessoObterUsuario(campanhaMock);
                }

                return CampanhaResult.DadosInvalidos(null, "Campanha não encontrada.");
            }
            catch (Exception ex)
            {
                return CampanhaResult.ErroObterCampanha(ex, "Erro ao obter a campanha simulada.");
            }
        }

        //public async Task<IEnumerable<Campanhas>> ObterCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        //{
        //    try
        //    {
        //        using var connection = GetConnection();

        //        var query = @"
        //                    SELECT Id, Nome, DataInicio, DataFim, Ativo, DataCadastro, DataAtualizacao
        //                    FROM Campanhas
        //                    WHERE DataInicio >= @DataInicio AND DataFim <= @DataFim";

        //        var campanhas = await connection.QueryAsync<Campanhas>(
        //            sql: query,
        //            param: new { DataInicio = dataInicio, DataFim = dataFim }
        //        ).ConfigureAwait(false);

        //        return campanhas;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao obter campanhas no período entre {DataInicio} e {DataFim}.", dataInicio, dataFim);
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<Campanhas>> ObterCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            await Task.Delay(100);

            try
            {
                var campanhasMock = new List<Campanhas>
                {
                    new Campanhas
                    {
                        Id = 1,
                        Nome = "Campanha Aniversário",
                        Descricao = "Promoção de aniversário da empresa.",
                        Tipo = "Aniversário",
                        Regras = "Regras da campanha de aniversário.",
                        Premio = "Desconto de 20%",
                        MaxParticipantes = 500,
                        QuantidadePontos = 100,
                        DataInicio = new DateTime(2025, 1, 1),
                        DataFim = new DateTime(2025, 1, 31),
                        Ativo = 1,
                        ImagemUrl = "https://exemplo.com/imagem1.jpg",
                        DataCadastro = new DateTime(2024, 12, 1),
                        DataAtualizacao = DateTime.MinValue
                    },
                    new Campanhas
                    {
                        Id = 2,
                        Nome = "Campanha Verão",
                        Descricao = "Promoção especial para o verão.",
                        Tipo = "Temporada",
                        Regras = "Regras da campanha de verão.",
                        Premio = "Brinde especial",
                        MaxParticipantes = 300,
                        QuantidadePontos = 50,
                        DataInicio = new DateTime(2025, 1, 15),
                        DataFim = new DateTime(2025, 2, 15),
                        Ativo = 1,
                        ImagemUrl = "https://exemplo.com/imagem2.jpg",
                        DataCadastro = new DateTime(2024, 12, 15),
                        DataAtualizacao = DateTime.MinValue
                    }
                };

                var campanhasFiltradas = campanhasMock.Where(c => c.DataInicio >= dataInicio && c.DataFim <= dataFim);

                _logger.LogInformation("Foram encontradas {Count} campanhas no período entre {DataInicio} e {DataFim}.", campanhasFiltradas.Count(), dataInicio, dataFim);

                return campanhasFiltradas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter campanhas no período entre {DataInicio} e {DataFim}.", dataInicio, dataFim);
                throw;
            }
        }


        //public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
        //{
        //    var sql = @"
        //              INSERT INTO [dbo].[Campanha]
        //              (                          
        //                  cam.Nome,
        //                  cam.Descricao,
        //                  cam.Tipo,
        //                  cam.Regras,
        //                  cam.Premio,
        //                  cam.MaxParticipantes,
        //                  cam.QuantidadePontos,
        //                  cam.DataInicio,
        //                  cam.DataFim,
        //                  cam.Ativo,
        //                  cam.ImagemUrl,
        //                  cam.DataCadastro,
        //                  cam.DataAtualizacao
        //              )                              
        //              VALUES 
        //              (                       
        //                  @Nome,
        //                  @Descricao,
        //                  @Tipo,
        //                  @Regras,
        //                  @Premio,
        //                  @MaxParticipantes,
        //                  @QuantidadePontos,
        //                  @DataInicio,
        //                  @DataFim,
        //                  true,
        //                  @ImagemUrl,
        //                  GETDATE(),
        //                  null
        //               );
        //              SELECT CAST(SCOPE_IDENTITY() as int);";

        //    using (var connection = GetConnection())
        //    {
        //        connection.Open();

        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                var result = await connection.ExecuteScalarAsync<int>(
        //                    sql,
        //                    new
        //                    {
        //                        campanha.Nome,
        //                        campanha.Descricao,
        //                        campanha.Tipo,
        //                        campanha.Regras,
        //                        campanha.Premio,
        //                        campanha.MaxParticipantes,
        //                        campanha.QuantidadePontos,
        //                        campanha.DataInicio,
        //                        campanha.DataFim,
        //                        campanha.Ativo,
        //                        campanha.ImagemUrl,
        //                        campanha.DataCadastro,
        //                        campanha.DataAtualizacao
        //                    },
        //                    transaction
        //                );

        //                transaction.Commit();
        //                return result;
        //            }
        //            catch(Exception ex)
        //            {
        //                transaction.Rollback();
        //                _logger.LogError(ex, "Erro ao cadastrar campanha: {Nome}", campanha.Nome);
        //                throw;
        //            }
        //        }
        //    }
        //}

        public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
        {
            await Task.Delay(100);

            try
            {
                if (string.IsNullOrWhiteSpace(campanha.Nome) || campanha.DataInicio == default || campanha.DataFim == default)
                {
                    throw new ArgumentException("Dados da campanha são inválidos.");
                }

                var idMock = new Random().Next(1, 1000);

                _logger.LogInformation("Campanha {Nome} cadastrada com sucesso com ID {Id}.", campanha.Nome, idMock);

                return idMock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar campanha: {Nome}", campanha.Nome);
                throw;
            }
        }


        //public async Task<int> AtualizarCampanhaAsync(Campanhas campanha)
        //{
        //    const string sql = @"
        //                        UPDATE [dbo].[Campanha]
        //                        SET Nome = @Nome,
        //                            Descricao = @Descricao,
        //                            Tipo = @Tipo,
        //                            Regras = @Regras,
        //                            Premio = @Premio,
        //                            MaxParticipantes = @MaxParticipantes,
        //                            QuantidadePontos = @QuantidadePontos,
        //                            DataInicio = @DataInicio,
        //                            DataFim = @DataFim,
        //                            Ativo = @Ativo,
        //                            ImagemUrl = @ImagemUrl,
        //                            DataAtualizacao = GETDATE()
        //                        WHERE Id = @IdCampanha";

        //    try
        //    {
        //        using var connection = GetConnection();
        //        connection.Open();

        //        var result = await connection.ExecuteAsync(sql, new
        //        {
        //            IdCampanha = campanha.Id,
        //            campanha.Nome,
        //            campanha.Descricao,
        //            campanha.Tipo,
        //            campanha.Regras,
        //            campanha.Premio,
        //            campanha.MaxParticipantes,
        //            campanha.QuantidadePontos,
        //            campanha.DataInicio,
        //            campanha.DataFim,
        //            campanha.Ativo,
        //            campanha.ImagemUrl,
        //            campanha.DataCadastro,
        //            campanha.DataAtualizacao
        //        }).ConfigureAwait(false);

        //        if (result == 0)
        //        {
        //            throw new InvalidOperationException($"Nenhum registro encontrado para o IdUsuario: {campanha.Id}");
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao atualizar o usuário com Id: {IdUsuario}", campanha.Id);
        //        throw;
        //    }
        //}

        public async Task<int> AtualizarCampanhaAsync(Campanhas campanha)
        {
            await Task.Delay(100);

            try
            {
                if (campanha.Id <= 0)
                {
                    throw new ArgumentException("ID da campanha inválido.");
                }

                var campanhaMockExistente = new Campanhas
                {
                    Id = campanha.Id,
                    Nome = "Campanha Existente",
                    Descricao = "Descrição Existente",
                    Tipo = "Tipo Existente",
                    Regras = "Regras Existentes",
                    Premio = "Premio Existente",
                    MaxParticipantes = 100,
                    QuantidadePontos = 50,
                    DataInicio = DateTime.Now.AddDays(-10),
                    DataFim = DateTime.Now.AddDays(10),
                    Ativo = 1,
                    ImagemUrl = "https://exemplo.com/imagem-existente.jpg",
                    DataCadastro = DateTime.Now.AddMonths(-1),
                    DataAtualizacao = DateTime.MinValue
                };

                if (campanhaMockExistente == null || campanhaMockExistente.Id != campanha.Id)
                {
                    _logger.LogWarning("Nenhum registro encontrado para o Id: {Id}", campanha.Id);
                    return 0;
                }

                campanhaMockExistente.Nome = campanha.Nome ?? campanhaMockExistente.Nome;
                campanhaMockExistente.Descricao = campanha.Descricao ?? campanhaMockExistente.Descricao;
                campanhaMockExistente.Tipo = campanha.Tipo ?? campanhaMockExistente.Tipo;
                campanhaMockExistente.Regras = campanha.Regras ?? campanhaMockExistente.Regras;
                campanhaMockExistente.Premio = campanha.Premio ?? campanhaMockExistente.Premio;
                campanhaMockExistente.MaxParticipantes = campanha.MaxParticipantes;
                campanhaMockExistente.QuantidadePontos = campanha.QuantidadePontos;
                campanhaMockExistente.DataInicio = campanha.DataInicio;
                campanhaMockExistente.DataFim = campanha.DataFim;
                campanhaMockExistente.Ativo = campanha.Ativo;
                campanhaMockExistente.ImagemUrl = campanha.ImagemUrl ?? campanhaMockExistente.ImagemUrl;
                campanhaMockExistente.DataAtualizacao = DateTime.Now;

                _logger.LogInformation("Campanha com ID {Id} atualizada com sucesso.", campanha.Id);

                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a campanha com ID {Id}.", campanha.Id);
                throw;
            }
        }

    }
}
*/