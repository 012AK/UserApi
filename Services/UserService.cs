using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Models;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/users
    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await _db.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            })
            .ToListAsync();
    }

    // GET /api/users/{id}
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _db.Users
            .Where(user => user.Id == id)
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            })
            .FirstOrDefaultAsync();
    }

    // POST /api/users
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    // PUT /api/users/{id}
    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
            return null;

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Age = dto.Age;

        await _db.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

        public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
            return false;

        _db.Users.Remove(user);

        await _db.SaveChangesAsync();

        return true;
    }
}