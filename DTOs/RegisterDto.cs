using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Range(18, 100)]
    public int Age { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";
}