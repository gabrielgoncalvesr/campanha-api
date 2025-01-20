using Fidelicard.Campanha.Core.Models;

namespace Fidelicard.Campanha.Core.Interface
{
    public interface ICampanhaRepository
    {
        Task<Campanhas> ObterPorIdAsync(int id);
        Task<IEnumerable<Campanhas>> ListarAsync();
        Task<IEnumerable<Campanhas>> ListarAtivasAsync();
        Task<int> CadastrarAsync(Campanhas campanha);
        Task<bool> AtualizarAsync(Campanhas campanha);
        Task<bool> ExcluirAsync(int id);
    }
}
