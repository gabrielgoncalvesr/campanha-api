using Fidelicard.Campanha.Core.Mock;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Microsoft.Extensions.Logging;

namespace Fidelicard.Campanha.Core.Service
{
    public class MockCampanhaService : IMockCampanhaService
    {
        private readonly IMockCampanhaRepository _mockCampanhaRepository;
        private readonly ILogger<MockCampanhaService> _logger;

        public MockCampanhaService(ILogger<MockCampanhaService> logger
        , IMockCampanhaRepository mockCampanhaRepository)
        {
            _logger = logger;
            _mockCampanhaRepository = mockCampanhaRepository;
        }

        public async Task<CampanhaResult> ConsultarCampanhaAsync(int idCampanha)
        {
            _logger.LogInformation("Iniciando consulta de campanha com Id: {IdCampanha}", idCampanha);

            try
            {
                var campanha = await _mockCampanhaRepository.ObterCampanhaAsync(idCampanha).ConfigureAwait(false);

                if (campanha == null)
                {
                    var mensagem = $"Campanha inexistente pelo código informado: {idCampanha}";
                    _logger.LogWarning(mensagem);
                    return CampanhaResult.DadosInvalidos(new Exception(mensagem), mensagem);
                }

                _logger.LogInformation("Consulta do usuário com Id: {IdCampanha} concluída com sucesso.", idCampanha);
                return CampanhaResult.SucessoObterUsuario(campanha.Campanha);
            }
            catch (Exception ex)
            {
                var mensagemErro = $"Erro ao consultar o usuário com Id: {idCampanha}.";
                _logger.LogError(ex, mensagemErro);
                return CampanhaResult.ErroObterCampanha(ex, mensagemErro);
            }
        }

        public async Task<CampanhaResult> ConsultarCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            _logger.LogInformation("Iniciando consulta de campanhas no período de {DataInicio} a {DataFim}", dataInicio, dataFim);

            try
            {                
                var campanhas = await _mockCampanhaRepository.ObterCampanhasPorPeriodoAsync(dataInicio, dataFim).ConfigureAwait(false);

                if (campanhas == null || !campanhas.Any())
                {
                    var mensagem = $"Nenhuma campanha encontrada no período de {dataInicio:yyyy-MM-dd} a {dataFim:yyyy-MM-dd}.";
                    _logger.LogWarning(mensagem);
                    return CampanhaResult.DadosInvalidos(new Exception(mensagem), mensagem);
                }

                _logger.LogInformation("Consulta de campanhas no período concluída com sucesso.");
                return CampanhaResult.SucessoObterUsuarioPorPeriodo(campanhas);
            }
            catch (Exception ex)
            {
                var mensagemErro = $"Erro ao consultar campanhas no período de {dataInicio:yyyy-MM-dd} a {dataFim:yyyy-MM-dd}.";
                _logger.LogError(ex, mensagemErro);
                return CampanhaResult.ErroObterCampanha(ex, mensagemErro);
            }
        }


        public async Task<int> CadastrarCampanhaAsync(Campanhas campanha)
        {
            _logger.LogInformation("Iniciando cadastro de campanha: {CampanhaNome}", campanha?.Nome);

            try
            {
                var result = await _mockCampanhaRepository.CadastrarCampanhaAsync(campanha).ConfigureAwait(false);

                _logger.LogInformation("Campanha cadastrada com sucesso. Id gerado: {CampanhaId}", result);
                return result;
            }
            catch (Exception ex)
            {
                var mensagemErro = "Erro ao cadastrar o campanha.";
                _logger.LogError(ex, mensagemErro);
                throw new ApplicationException(mensagemErro, ex);
            }
        }

        public async Task<int> AtualizarCampanhaAsync(Campanhas campanha)
        {
            _logger.LogInformation("Iniciando cadastro da campanha: {CampanhaNome}", campanha?.Nome);

            try
            {
                var result = await _mockCampanhaRepository.AtualizarCampanhaAsync(campanha).ConfigureAwait(false);

                _logger.LogInformation("Campanha atualizado com sucesso. Id gerado: {CampanhaId}", result);
                return result;
            }
            catch (Exception ex)
            {
                var mensagemErro = "Erro ao atualizar o campanha.";
                _logger.LogError(ex, mensagemErro);
                throw new ApplicationException(mensagemErro, ex);
            }
        }
    }
}
