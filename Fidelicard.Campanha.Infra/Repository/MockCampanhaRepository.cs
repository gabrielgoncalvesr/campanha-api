using Fidelicard.Campanha.Core.Mock;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Microsoft.Extensions.Logging;

public class MockCampanhaRepository : IMockCampanhaRepository
{
    private readonly ILogger _logger;

    public MockCampanhaRepository(ILogger<MockCampanhaRepository> logger)
    {
        _logger = logger;
    }

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

    public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
    {
        await Task.Delay(100);

        try
        {
            if (string.IsNullOrWhiteSpace(campanha.Nome) || campanha.DataInicio == default || campanha.DataFim == default)
            {
                throw new ArgumentException("Dados da campanha são inválidos.");
            }

            var idMock = new Random().Next(1, 1000); // Gerar um ID aleatório para a campanha

            _logger.LogInformation("Campanha {Nome} cadastrada com sucesso com ID {Id}.", campanha.Nome, idMock);

            return idMock;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cadastrar campanha: {Nome}", campanha.Nome);
            throw;
        }
    }

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
}
