using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;

namespace Fidelicard.Campanha.IntegrationTests
{
    public class MockCampanhaRepository : ICampanhaRepository
    {
        private readonly List<Campanhas> _campanhas;

        public MockCampanhaRepository()
        {
            _campanhas = new List<Campanhas>();
        }

        public async Task<int> CadastrarAsync(Campanhas campanha)
        {
            campanha.Id = _campanhas.Count + 1;
            _campanhas.Add(campanha);
            return await Task.FromResult(campanha.Id);
        }

        public async Task<bool> AtualizarAsync(Campanhas campanha)
        {
            var index = _campanhas.FindIndex(c => c.Id == campanha.Id);
            if (index != -1)
            {
                _campanhas[index] = campanha;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var campanha = _campanhas.FirstOrDefault(c => c.Id == id);
            if (campanha != null)
            {
                _campanhas.Remove(campanha);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Campanhas> ObterPorIdAsync(int id)
        {
            return await Task.FromResult(_campanhas.FirstOrDefault(c => c.Id == id));
        }

        public async Task<IEnumerable<Campanhas>> ListarAsync()
        {
            return await Task.FromResult(_campanhas);
        }

        public async Task<IEnumerable<Campanhas>> ListarAtivasAsync()
        {
            return await Task.FromResult(_campanhas.Where(c => c.Ativo == 1));
        }
    }
}
