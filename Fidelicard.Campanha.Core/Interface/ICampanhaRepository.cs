using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using System.Data;

namespace Fidelicard.Campanha.Core.Interface
{
    public interface ICampanhaRepository
    {
        Task<CampanhaResult> ObterCampanhaAsync(int idCampanha);
        Task<IEnumerable<Campanhas>> ObterCampanhasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<int> CadastrarCampanhaAsync(Campanhas campanha);
        Task<int> AtualizarCampanhaAsync(Campanhas campanha);

        IDbConnection GetConnection();
    }
}
