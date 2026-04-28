

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Dtos;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService) {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        // 1. Verificar se o nome já existe
        if (await _context.Users.AnyAsync(u => string.Equals(u.Username, registerDto.Username)))
            return BadRequest("Esse usuário já está ocupado.");

        // 2. Criar o usuário e encriptar a password
        var user = new User
        {
            Username = registerDto.Username.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        // 3. Salvar no banco de dados
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User criado com sucesso");
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(LoginDto loginDto)
    {
        // 1. Procurar o usuário pelo nome
        var user = await _context.Users.SingleOrDefaultAsync(u => string.Equals(u.Username,loginDto.Username));

        // 2. Verificar se o user é nulo e mostrar erro
        if (user == null) return Unauthorized("Usuário ou senha inválidos.");

        // 3. Comparar a senha enviada com a hash guardada no banco
        if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) return Unauthorized("User ou senha inválidos");

        // Atribuímos o token ao user
        return Ok(_tokenService.CreateToken(user));
    }
    
}