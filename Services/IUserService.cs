using UserApi.DTOs;

namespace UserApi.Services;

public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync();

    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<bool> DeleteUserAsync(int id);
}