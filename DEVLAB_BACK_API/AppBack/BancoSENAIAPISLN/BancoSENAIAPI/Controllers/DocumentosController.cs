using BancoSENAIAPI.DB;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly string _caminhoRaiz =
            Path.Combine(Directory.GetCurrentDirectory(), "ClienteArquivo");

        private static int _nextId = 1;

        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> PostArquivo(
            int codigoCliente,
            IFormFile arquivo)
        {
            double tamanhoMax = 2 * 1024 * 1024;

            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo encontrado");
            }

           /* if (arquivo.Length > tamanhoMax)
            {
                return BadRequest("Tamanho do arquivo passou de 2MB, então não pode ser enviado");
            }*/

            if (!Banco._cliente.Any(e => e.CodigoClient == codigoCliente))
            {
                return NotFound("Nenhum cliente encontrado");
            }

           string extensao = Path.GetExtension(arquivo.FileName).ToLower();

           /* if (extensao != ".png" &&
                extensao != ".jpg" &&
                extensao != ".pdf")
            {
                return BadRequest(
                    "Somente podem ser enviados arquivos do tipo png, jpg ou pdf");
            }*/

            string pastaCliente = Path.Combine(
                _caminhoRaiz,
                codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            string nomeOriginal =
                Path.GetFileNameWithoutExtension(arquivo.FileName);

           string novoNome =
                $"{codigoCliente}_{nomeOriginal}_{Guid.NewGuid()}{extensao}";

            string caminhoFinal =
                Path.Combine(pastaCliente, novoNome);

            using (var stream = new FileStream(
                caminhoFinal,
                FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentoNovo = new DocumentoMetadados
            {
                Id = _nextId++,
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = caminhoFinal,
                CodigoClient = codigoCliente
            };

            Banco._document.Add(documentoNovo);

            return Ok(new
            {
                mensagem = "Documento criado com sucesso"
            });
        }

        [HttpGet("listagem/{codigoCliente}")]
        public IActionResult ListarDocumentos(int codigoCliente)
        {
            if (!Banco._cliente.Any(e => e.CodigoClient == codigoCliente))
            {
                return NotFound("Nenhum cliente encontrado");
            }

            var documentos = Banco._document
                .Where(d => d.CodigoClient == codigoCliente)
                .ToList();

            return Ok(documentos);
        }

        [HttpGet("cliente/{codigoCliente}/download/{id}")]
        public async Task<IActionResult> DownloadArquivo(
            int id,
            int codigoCliente)
        {
            var documento = Banco._document
                .FirstOrDefault(e => e.Id == id);

            if (documento == null)
            {
                return NotFound("Nenhum arquivo encontrado");
            }

            if (documento.CodigoClient != codigoCliente)
            {
                return BadRequest("Você não pode ver esse arquivo");
            }

            if (!System.IO.File.Exists(documento.Caminho))
            {
                return NotFound("Arquivo não encontrado no servidor");
            }

            var fileBytes =
                await System.IO.File.ReadAllBytesAsync(documento.Caminho);

            string contentType = documento.Extensao.ToLower() switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            return File(
                fileBytes,
                contentType,
                documento.Name + documento.Extensao);
        }

        [HttpDelete("cliente/{codigoCliente}/excluir/{id}")]
        public IActionResult DeleteDocument(
            int id,
            int codigoCliente)
        {
            var documento = Banco._document
                .FirstOrDefault(e => e.Id == id);

            if (documento == null)
            {
                return NotFound("Nenhum arquivo encontrado");
            }

            if (documento.CodigoClient != codigoCliente)
            {
                return BadRequest("Você não pode excluir esse arquivo");
            }

            Banco._document.Remove(documento);

            if (System.IO.File.Exists(documento.Caminho))
            {
                System.IO.File.Delete(documento.Caminho);
            }

            return NoContent();
        }
    }
}
