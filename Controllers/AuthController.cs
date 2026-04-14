using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MzansiBuilds.Data; // Ensure this matches your namespace for ApplicationDbContext
using MzansiBuilds.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context; // Required for database access

    // Constructor injection for both configuration (JWT) and database context
    public AuthController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    /// Registers a new user into the Azure SQL Database.
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Force the ID to 0 so EF knows it's a new record and lets the DB handle it
        user.Id = 0;

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            return BadRequest("Username already taken.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully!" });
    }

    /// Authenticates a user and returns a JWT token for secure API access.
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Query Azure SQL to find a user matching the provided credentials
        // Note: In production, use hashing (e.g., BCrypt) instead of plain-text comparison
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.Password);

        if (user != null)
        {
            // Generate token if credentials are valid
            var token = GenerateJwtToken(user.Username);
            return Ok(new { token });
        }

        // Return 401 if authentication fails
        return Unauthorized();
    }

    /// Helper method to generate a signed JWT token containing user identity claims.
    private string GenerateJwtToken(string username)
    {
        // Define identity claims to be encoded in the token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Secure the token using the secret key from appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Configure the JWT payload and expiration
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: creds
        );

        // Return the serialized token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}