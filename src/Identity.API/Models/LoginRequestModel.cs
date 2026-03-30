using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public record LoginRequestModel
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}
