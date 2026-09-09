using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarteiraController : ControllerBase
    {
        private static List<Carteira> _carteiras = new List<Carteira>
        {
            new Carteira { NumeroCarteira = 1001, NomeCarteira = "Carteira Comercial", ApetiteCarteira = 150000.00m },
            new Carteira { NumeroCarteira = 2002, NomeCarteira = "Carteira de Investimentos", ApetiteCarteira = 500000.00m },
            new Carteira { NumeroCarteira = 3003, NomeCarteira = "Carteira Agro", ApetiteCarteira = 250000.00m }
        };

        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_carteiras);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Carteira novaCarteira)
        {
            if (_carteiras.Any(c => c.NumeroCarteira == novaCarteira.NumeroCarteira))
                return BadRequest(new { message = "Este número de carteira já existe." });

            _carteiras.Add(novaCarteira);
            return Created("", novaCarteira);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var carteira = _carteiras.FirstOrDefault(c => c.NumeroCarteira == codigo);
            if (carteira == null)
                return NotFound(new { message = "Carteira não encontrada." });

            return Ok(carteira);
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Carteira carteiraAtualizada)
        {
            var carteiraExistente = _carteiras.FirstOrDefault(c => c.NumeroCarteira == codigo);
            if (carteiraExistente == null)
                return NotFound();

            carteiraExistente.NomeCarteira = carteiraAtualizada.NomeCarteira;
            carteiraExistente.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;

            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var carteira = _carteiras.FirstOrDefault(c => c.NumeroCarteira == codigo);
            if (carteira == null)
                return NotFound();

            _carteiras.Remove(carteira);
            return Ok(new { message = "Carteira excluída com sucesso." });
        }
    }
}
