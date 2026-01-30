using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Application.Users.Data;
using SampleCkWebApp.Application.Users.Interfaces.Application;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using System.Runtime.CompilerServices;
using SampleCkWebApp.Application.Users.Interfaces;

namespace SampleCkWebApp.Application.Users;


public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;

    private readonly IPasswordHasher _passwordHasher;

    public UserService (IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<GetUsersResult>> GetUsersAsync(CancellationToken cancellationToken)
    {
        

        var result = await _userRepository.GetUsersAsync(cancellationToken);
        if(result.IsError)
        {
            
            return result.Errors;

        }

        return new GetUsersResult
        {
            
            Users = result.Value
        };



    }



public async Task<ErrorOr<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken)

    {
        
        var result = await _userRepository.GetUserByIdAsync(id, cancellationToken);
        return result;
    }





public async Task <ErrorOr<User>> CreateUserAsync(string name, string email, string password, CancellationToken cancellationToken)

    {
        
        var validationResult = UserValidator.ValidateCreateUserRequest(name, email, password);
        if(validationResult.IsError)
        {

            return validationResult.Errors;     

        }

        var existingUser = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        if(!existingUser.IsError)
        {
            
            return UserErrors.DuplicateEmail;
        }

        var passwordHash = _passwordHasher.Hash(password);

        var user = new User
        {
            
            Name = name, 
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        };

        var createResult = await _userRepository.CreateUserAsync(user, cancellationToken);
        return createResult;


    }

    public async Task <ErrorOr<User>> UpdateUserAsync(int id, string name, string email, string password, CancellationToken cancellationToken)
    {
        
    var existingResult = await _userRepository.GetUserByIdAsync(id, cancellationToken);
    if (existingResult.IsError)
        {
            return existingResult.Errors;
        }

        var existingUser = existingResult.Value;

        if (string.IsNullOrWhiteSpace(name))
        {
            return UserErrors.InvalidName;
        }

        if (string.IsNullOrWhiteSpace(email))
        {       
            return UserErrors.InvalidEmail;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return UserErrors.InvalidPassword;
        }

        if(!string.Equals(existingUser.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            
            var emailCheck = await _userRepository.GetUserByEmailAsync(email, cancellationToken);

            if (!emailCheck.IsError)
            {
                
                return UserErrors.DuplicateEmail;

            }

        }

            var passwordHash =  _passwordHasher.Hash(password);

            var updateUser = new User
            {
                
                Id = existingUser.Id,
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = existingUser.CreatedAt,
                UpdatedAt = DateTime.UtcNow


            };

           var updateResult = await _userRepository.UpdateUserAsync(updateUser, cancellationToken);
           return updateResult;



    }

    public async Task <ErrorOr<bool>> ChangeMyPasswordAsync(int currentUserId,string currentPassword,string newPassword,
    string confirmNewPassword,CancellationToken cancellationToken)
    {
        
        if(string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword)
        || string.IsNullOrWhiteSpace(confirmNewPassword))
        {
            
            return Error.Validation("User.Passord.Invalid", "Password fields are required");

        }

        if (newPassword != confirmNewPassword)
        return UserErrors.PasswordsDoNotMatch;

        if (newPassword.Length < 6)
            return Error.Validation("User.Password.TooShort", "New password must be at least 6 characters long.");

        var userResult = await _userRepository.GetUserByIdAsync(currentUserId, cancellationToken);
        if (userResult.IsError)
            return userResult.Errors;

        var user = userResult.Value;

        if (!_passwordHasher.Verify(currentPassword, user.PasswordHash))
            return UserErrors.InvalidCurrentPassword;

        var newHash = _passwordHasher.Hash(newPassword);

        var updateResult = await _userRepository.UpdatePasswordHashAsync(currentUserId, newHash, cancellationToken);
        return updateResult;


    }

     public async Task <ErrorOr<bool>> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {

        var userResult = await _userRepository.GetUserByIdAsync(id, cancellationToken);
        if(userResult.IsError)
        {
            
            return userResult.Errors;

        }
        
    
    
        var deleteResult = await _userRepository.DeleteUserAsync(id, cancellationToken);
        return deleteResult;

    }
    


}