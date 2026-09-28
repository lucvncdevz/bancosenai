using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Agencia
    {
        [Required]
        [Key]
        public int NumeroAgencia { get; set; } 
        public string Cidade { get; set; }
        public string SiglaEstado { get; set; }

    }
}
