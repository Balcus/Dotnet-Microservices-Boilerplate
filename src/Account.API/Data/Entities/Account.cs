using System.ComponentModel.DataAnnotations;
using Account.API.Data.Enums;
using SharedAbstractions.Interfaces;

namespace Account.API.Data.Entities;

public class Account : IEntityBase<string>
{
    [Key] 
    [MaxLength(36)] 
    public string Id { get; set; } = null!;

    [Required] 
    [MaxLength(100)] 
    public string FirstName { get; set; } = null!;

    [Required] 
    [MaxLength(100)] 
    public string LastName { get; set; } = null!;

    [Required] 
    [MaxLength(20)] 
    public string PhoneNumber { get; set; } = null!;

    [Required] 
    [MaxLength(256)] 
    public string Email { get; set; } = null!;
    
    [MaxLength(256)]
    public string? PlaceOfBirth { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    
    public Gender Gender { get; set; } = Gender.Unknown;

    [MaxLength(2048)]
    public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}