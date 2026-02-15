using ErrorOr;
using SampleCkWebApp.Application.Users.Interfaces;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;

namespace SampleCkWebApp.Application.Users.Auth;

public sealed class RegisterUser
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUser(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<User>> HandleAsync(string name, string email, string password, CancellationToken cancellationToken)
    {
        name = (name ?? "").Trim();
        email = (email ?? "").Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("User.Name", "Name is required.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Error.Validation("User.Email", "Email is invalid.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return Error.Validation("User.Password", "Password must be at least 6 characters.");

    
        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = _passwordHasher.Hash(password),

         
            Role = "User",
            IsActive = true,

      
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _users.CreateUserAsync(user, cancellationToken);

        
        if (created.IsError)
            return created.Errors;

        return created.Value;
    }
}
