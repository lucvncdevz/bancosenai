using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController : ControllerBase
    {
        // Lista estática simulando o banco de dados com a entidade Client
        private static List<Client> _clients = new List<Client>
        {
            new Client { CodigoClient = 1, NomeClient = "João Silva", CPF = "123.456.789-00", NumeroAgencia = 1001, SaldoTotal = 1500.50f, Sexo = "M", Enderco = "Rua A, 123", Cidade = "São Paulo", Estado = "SP" },
            new Client { CodigoClient = 2, NomeClient = "Maria Souza", CPF = "987.654.321-11", NumeroAgencia = 2002, SaldoTotal = 50000.00f, Sexo = "F", Enderco = "Av B, 456", Cidade = "Rio de Janeiro", Estado = "RJ" }
        };

        [HttpGet]
        public IActionResult ListarTodos()
        {
            return Ok(_clients);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Client novoClient)
        {
            if (_clients.Any(c => c.CodigoClient == novoClient.CodigoClient))
                return BadRequest(new { message = "Este código de cliente já existe." });

            _clients.Add(novoClient);
            return Created("", novoClient);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var client = _clients.FirstOrDefault(c => c.CodigoClient == codigo);
            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            return Ok(client);
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Client clientAtualizado)
        {
            var clientExistente = _clients.FirstOrDefault(c => c.CodigoClient == codigo);
            if (clientExistente == null)
                return NotFound(new { message = "Cliente não encontrado para atualização." });

            // Atualizando as propriedades do objeto existente
            clientExistente.NomeClient = clientAtualizado.NomeClient;
            clientExistente.CPF = clientAtualizado.CPF;
            clientExistente.NumeroAgencia = clientAtualizado.NumeroAgencia;
            clientExistente.SaldoTotal = clientAtualizado.SaldoTotal;
            clientExistente.Sexo = clientAtualizado.Sexo;
            clientExistente.Enderco = clientAtualizado.Enderco;
            clientExistente.Cidade = clientAtualizado.Cidade;
            clientExistente.Estado = clientAtualizado.Estado;

            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var client = _clients.FirstOrDefault(c => c.CodigoClient == codigo);
            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            _clients.Remove(client);
            return Ok(new { message = "Cliente excluído com sucesso." });
        }
    }
}
