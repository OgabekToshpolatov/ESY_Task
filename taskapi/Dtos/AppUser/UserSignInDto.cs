using System.ComponentModel.DataAnnotations;

namespace taskapi.Dtos.AppUser;

public class UserSignInDto
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}