using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs;

public class RegisterDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";
}