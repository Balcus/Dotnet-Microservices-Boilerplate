namespace IdentityService.Models;

public record ErrorResponseModel
{
    public string Title { get; set; } = "An error occurred.";
    public int Status { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}