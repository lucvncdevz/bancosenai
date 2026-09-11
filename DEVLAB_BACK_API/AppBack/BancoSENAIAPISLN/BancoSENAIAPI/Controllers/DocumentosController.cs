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
            if(arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            string pastaClient = Path.Combine(_caminhoRaiz, codigoClient.ToString());

            if(!Directory.Exists(pastaClient))
            {
                Directory.CreateDirectory(pastaClient);
            }

            string extensao = Path.GetExtension(arquivo.FileName);
            string nomeOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novoNome = $"{codigoClient}.{nomeOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaClient, novoNome);

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await stream.CopyToAsync(stream);
            };

            var documentosMetadados= new Models.DocumentoMetadados()
            {
               Id = _nextId++,
               Name = nomeOriginal,
               Extensao = extensao,
               Caminho = nomeOriginal,
               CodigoClient = codigoClient

            };

            _documentosMetadados.Add(documentosMetadados);

            return Ok(new {mensagem = "Documento criado com sucesso"});
        }
    }
}
