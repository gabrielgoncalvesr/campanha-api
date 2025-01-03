namespace Fidelicard.Campanha.Core.Models
{
    public class CampanhasResponse
    {
        public int Id { get; set; }
        public Campanhas Campanha { get; set; }
        public int StatusCode { get; set; }
        public DateTime DataCadastro { get; set; }
        public string ErrorMessage { get; set; }        
    }
}
