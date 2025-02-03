using Microsoft.AspNetCore.Mvc;
using Webshop_Backend.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Webshop_Backend.Data;
using Webshop_Backend.DTOs;

namespace Webshop_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly WebShopDbContext _context;

        public AuthController(IConfiguration config, WebShopDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
        {
            // Provjera postoji li email u bazi
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest(new { Message = "Email se već koristi" });
            }

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registracija uspješna" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Unauthorized("Neispravni podaci");

            // Generiraj token s ispravnim claimovima
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = GenerateJwtToken(claims);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
