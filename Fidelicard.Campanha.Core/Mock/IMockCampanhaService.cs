using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;

namespace Fidelicard.Campanha.Core.Mock
{
    public interface IMockCampanhaService
    {
        Task<int> CadastrarCampanhaAsync(Campanhas campanha);

        Task<CampanhaResult> ConsultarCampanhaAsync(int idCampanha);

        Task<CampanhaResult> ConsultarCampanhasPorPeriodoAsync(DateTime DataInicio, DateTime DataFim);

        Task<int> AtualizarCampanhaAsync(Campanhas campanha);
    }
}
