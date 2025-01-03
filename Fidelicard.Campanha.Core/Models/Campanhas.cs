namespace Fidelicard.Campanha.Core.Models
{
    public class Campanhas
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Regras { get; set; }
        public string Premio { get; set; }
        public int? MaxParticipantes { get; set; }
        public int? QuantidadePontos { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Ativo { get; set; }
        public string ImagemUrl { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
