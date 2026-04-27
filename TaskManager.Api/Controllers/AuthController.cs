

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
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username.ToLower()))
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

    
}