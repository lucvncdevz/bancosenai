namespace BancoSENAIAPI.Models
{
    public class Client
    {
        public int CodigoClient {  get; set; }
        public string NomeClient { get; set; }
        public string CPF {  get; set; }
        public int NumeroAgencia { get; set; }
        public float SaldoTotal { get; set; }
        public string Sexo {  get; set; }

        public string Enderco { get; set; }
        
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
