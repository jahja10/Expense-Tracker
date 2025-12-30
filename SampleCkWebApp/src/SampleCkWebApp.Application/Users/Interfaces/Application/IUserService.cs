using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Users.Data;

namespace SampleCkWebApp.Application.Users.Interfaces.Application;


public interface IUserService
{
    
    Task<ErrorOr<GetUsersResult>> GetUsersAsync(CancellationToken cancellationToken);

    Task <ErrorOr<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken);

    Task <ErrorOr<User>> CreateUserAsync(string name, string email, string password, CancellationToken cancellationToken);

    Task <ErrorOr<User>> UpdateUserAsync(int id, string name, string email, string password, CancellationToken cancellationToken); 

    Task <ErrorOr<bool>> DeleteUserAsync(int id, CancellationToken cancellationToken);

    

}
