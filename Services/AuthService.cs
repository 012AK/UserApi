using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(AppDbContext db)
    {
        _db = db;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _db.Users
            .FirstOrDefaultAsync(user => user.Email == dto.Email);

        if (existingUser != null)
            throw new Exception("User with this email already exists.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };

        // Превращаем обычный пароль в hash
        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            dto.Password
        );

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

        public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(user => user.Email == dto.Email);

        if (user == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("THIS_IS_A_DEVELOPMENT_SECRET_KEY_CHANGE_IT")
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: "UserApi",
            audience: "UserApiClient",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}