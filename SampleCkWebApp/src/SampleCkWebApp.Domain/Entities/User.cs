namespace SampleCkWebApp.Domain.Entities;


public class User
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash {get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Role { get; set; }

    public bool IsActive { get; set; }

}