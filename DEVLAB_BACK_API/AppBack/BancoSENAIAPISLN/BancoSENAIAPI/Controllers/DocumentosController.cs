using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentosController : Controller
    {
        private readonly string _caminhoRaiz = Path.Combine(Directory.GetCurrentDirectory(), "clienteArquivos");

        private static List<Models.DocumentoMetadados> _documentosMetadados = new List<Models.DocumentoMetadados>();

        private static int _nextId = 1;
        [HttpPost("upload/{codigoClient}")]
        public async Task<IActionResult> AnexarArquivo(int codigoClient, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            // R06F - Limite máximo de 2 MB
            const long limiteMaximo = 2 * 1024 * 1024;

            if (arquivo.Length > limiteMaximo)
            {
                return BadRequest("O arquivo excede o limite máximo permitido de 2 MB.");
            }

            // R06G - Extensões permitidas
            string extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            string[] extensoesPermitidas = { ".pdf", ".jpg", ".png" };

            if (!extensoesPermitidas.Contains(extensao))
            {
                return BadRequest(
                    "Formato de arquivo não permitido. " +
                    "Apenas arquivos .pdf, .jpg e .png são aceitos."
                );
            }

            string pastaClient = Path.Combine(
                _caminhoRaiz,
                codigoClient.ToString()
            );

            if (!Directory.Exists(pastaClient))
            {
                Directory.CreateDirectory(pastaClient);
            }

            string nomeOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novoNome = $"{codigoClient}.{nomeOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaClient, novoNome);

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentoMetadados = new Models.DocumentoMetadados()
            {
                Id = _nextId++,
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = nomeOriginal,
                CodigoClient = codigoClient
            };

            _documentosMetadados.Add(documentoMetadados);

            return Ok(new
            {
                mensagem = "Documento criado com sucesso"
            });
        }


    }
}
