using UserApi.DTOs;

namespace UserApi.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}