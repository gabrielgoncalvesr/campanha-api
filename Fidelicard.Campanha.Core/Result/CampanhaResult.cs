using Fidelicard.Campanha.Core.Models;

namespace Fidelicard.Campanha.Core.Result
{
    public class CampanhaResult
    {
        public CampanhaStatus Status { get; protected set; }

        public Campanhas Campanha { get; protected set; }
        public IEnumerable<Campanhas> Campanhas { get; protected set; }

        public Exception ErroProcessamento { get; protected set; }

        public string Mensagem { get; protected set; }

        protected CampanhaResult(CampanhaStatus status, Campanhas campanha, IEnumerable<Campanhas> campanhas, Exception erroProcessamento, string mensagem)
        {
            Status = status;
            Campanha = campanha;
            Campanhas = campanhas;
            ErroProcessamento = erroProcessamento;
            Mensagem = mensagem;
        }

        public static CampanhaResult SucessoObterUsuario(Campanhas campanha) =>
            new CampanhaResult(CampanhaStatus.SucessoObterCampanha, campanha, null, null, string.Empty);

        public static CampanhaResult SucessoObterUsuarioPorPeriodo(IEnumerable<Campanhas> campanhas) =>
            new CampanhaResult(CampanhaStatus.SucessoObterCampanhasPorPeriodo, null, campanhas, null, string.Empty);

        public static CampanhaResult DadosInvalidos(Exception erroProcessamento, string mensagem) =>
           new CampanhaResult(CampanhaStatus.DadosInvalidos, null, null, erroProcessamento, mensagem);

        public static CampanhaResult ErroObterCampanha(Exception erroProcessamento, string mensagem) =>
            new CampanhaResult(CampanhaStatus.ErroObterCampanha, null, null, erroProcessamento, mensagem);
    }
}