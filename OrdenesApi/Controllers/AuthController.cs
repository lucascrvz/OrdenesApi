using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data; 
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly AuthService _authService;

    public AuthController(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User userDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
            return BadRequest("El nombre de usuario ya está en uso.");

        var user = new User
        {
            Username = userDto.Username,
        };

        user.Password = _passwordHasher.HashPassword(user, userDto.Password);

        var token = _authService.GenerateJwtToken(user.Username);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id, user.Username, Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

        if (user == null)
            return Unauthorized("Usuario o contraseña incorrectos.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return Unauthorized("Usuario o contraseña incorrectos.");

        return Ok(new
        {
            user.Id,
            user.Username,
            Token = _authService.GenerateJwtToken(user.Username)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        return Ok(new { user.Id, user.Username });
    }
}
