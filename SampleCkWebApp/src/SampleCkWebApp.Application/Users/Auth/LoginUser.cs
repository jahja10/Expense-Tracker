using SampleCkWebApp.Application.Users.Interfaces;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using ErrorOr;

namespace SampleCkWebApp.Application.Users.Auth;

public sealed class LogInUser
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public LogInUser(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<User>> HandleAsync(string email, string password, CancellationToken cancellationToken)
    {
        var userResult = await _users.GetUserByEmailAsync(email, cancellationToken);

       
        if (userResult.IsError)
        {
            
            return UserErrors.NotFound;

            
        }

        var user = userResult.Value;

        if (!user.IsActive)
        return Error.Unauthorized("User.Deactivated", "Account is deactivated.");

        if (!_passwordHasher.Verify(password, user.PasswordHash))
        {
           
            return Error.Unauthorized(description: "Invalid email or password");
        }

        


        return user;
    }
}
