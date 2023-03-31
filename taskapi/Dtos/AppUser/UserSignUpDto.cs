using System.ComponentModel.DataAnnotations;

namespace taskapi.Dtos.AppUser;

public class UserSignUpDto
{
    [Required]
    [MinLength(4)]
    public string? UserName { get; set; }

    [Required]
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name ="Confirm Password")]    
    [Required]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}