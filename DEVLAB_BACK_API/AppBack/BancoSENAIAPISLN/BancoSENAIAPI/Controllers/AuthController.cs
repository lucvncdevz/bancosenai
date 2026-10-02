using BancoSENAIAPI.Data;
using BancoSENAIAPI.Dtos;
using BancoSENAIAPI.Models;
using BancoSENAIAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController (AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest dto) 
        {
            if (await _context.Usuarios.AnyAsync(u => u.NomeUsuario == dto.NomeUsuario)) 
            { 
               return BadRequest(new {message = "Este nome de usuario já está em uso"});
            }
            var usuario = new Usuario {
                NomeUsuario = dto.NomeUsuario,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.SenhaHash)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Created("", new {usuario.Id, usuario.NomeUsuario});
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NomeUsuario == dto.NomeUsuario);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.NomeUsuario))
            {
               return Unauthorized(new { messege = "Usuario ou senha invalidos"});
            }

            var (token, ExpiraEm) = _tokenService.GerarToken(usuario);

            return Ok(new LoginResponseDto { Token = token, ExpiraEm = ExpiraEm});
        }


    }
}
