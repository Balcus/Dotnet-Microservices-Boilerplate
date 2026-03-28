using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public record RegisterRequestModel
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] [Phone] public string PhoneNumber { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] 
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}