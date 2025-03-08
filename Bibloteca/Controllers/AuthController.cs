using BCrypt.Net;
using Bibloteca.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Bibloteca.Models.Domain;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly JwtService _jwtService;

    public AuthController(ApplicationDBContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
            return Unauthorized("Credenciales incorrectas");

        var token = _jwtService.GenerateToken(usuario);
        return Ok(new { token,usuario });
    }

    [HttpGet("me")]
    [Authorize] // 🔒 Requiere autenticación con JWT
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = User.FindFirst("UserId")?.Value;

        Console.WriteLine(userId);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Usuario no autenticado" });

        var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.PkUsuario.ToString() == userId);

        if (usuario == null)
            return NotFound("Usuario no encontrado");

        return Ok(new
        {
            id = usuario.PkUsuario,
            name = usuario.Nombre,
            email = usuario.Email,
            role = usuario.Rol.Nombre
        });
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        
        var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            return BadRequest(new { message = "El correo ya está registrado" });

        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "User");
        if (adminRole == null)
            return BadRequest(new { message = "El rol de administrador no está configurado en la base de datos" });

        
        var newUser = new Usuario
        {
            Nombre = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Rol = adminRole, 
        };

        _context.Usuarios.Add(newUser);
        await _context.SaveChangesAsync();

        // Generar token JWT
        var token = _jwtService.GenerateToken(newUser);

        return Ok(new { token, usuario = new { id = newUser.PkUsuario, name = newUser.Nombre, email = newUser.Email, role = "User" } });
    }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}


public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}


