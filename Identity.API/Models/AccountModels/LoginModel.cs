using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.AccountModels;

public class LoginModel
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}
