using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Users.Interfaces.Infrastructure;

public interface IUserRepository
{
    
    Task<ErrorOr<List<User>>>GetUsersAsync(CancellationToken cancellationToken);
    Task<ErrorOr<User>>GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<ErrorOr<User>>GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<ErrorOr<User>>CreateUserAsync(User user, CancellationToken cancellationToken);

    Task <ErrorOr<User>>UpdateUserAsync(User user, CancellationToken cancellationToken);

    Task <ErrorOr<bool>> UpdatePasswordHashAsync(int id, string newPasswordHash, CancellationToken cancellationToken);

     Task <ErrorOr<bool>> DeactivateUserAsync(int id, CancellationToken cancellationToken);

    Task <ErrorOr<bool>>DeleteUserAsync(int id, CancellationToken cancellationToken);

}