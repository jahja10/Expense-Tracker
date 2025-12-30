using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Application.Users.Data;
using SampleCkWebApp.Application.Users.Interfaces.Application;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using System.Runtime.CompilerServices;

namespace SampleCkWebApp.Application.Users;


public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;

    public UserService (IUserRepository userRepository)
    {
        
        _userRepository = userRepository;

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

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

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

            var passwordHash =  BCrypt.Net.BCrypt.HashPassword(password);

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