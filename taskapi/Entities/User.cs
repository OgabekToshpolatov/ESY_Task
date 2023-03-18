using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace taskapi.Entities;

public class User:IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [NotMapped]
    public string[]? Roles { get; set; }
}