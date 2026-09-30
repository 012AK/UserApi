using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs;

public class UpdateUserDto
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Range(18, 100)]
    public int Age { get; set; }
}