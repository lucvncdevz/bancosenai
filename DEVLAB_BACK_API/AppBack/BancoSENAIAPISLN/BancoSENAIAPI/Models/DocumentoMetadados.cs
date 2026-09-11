namespace BancoSENAIAPI.Models
{
    public class DocumentoMetadados
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extensao { get; set;}
        public string Caminho { get; set; }
        public string CodigoClient { get; set;}
    }
}
