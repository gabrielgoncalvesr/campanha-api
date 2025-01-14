using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;

namespace Fidelicard.Campanha.Core.Mock
{
    public interface IMockCampanhaRepository
    {
        Task<CampanhaResult> ObterCampanhaAsync(int idCampanha);
        Task<IEnumerable<Campanhas>> ObterCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<int> CadastrarCampanhaAsync(Campanhas campanha);
        Task<int> AtualizarCampanhaAsync(Campanhas campanha);
    }
}
