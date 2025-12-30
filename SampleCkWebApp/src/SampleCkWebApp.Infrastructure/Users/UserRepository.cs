using ErrorOr;
using Npgsql;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Infrastructure.Common;

namespace SampleCkWebApp.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly DatabaseOptions  _options;

    public UserRepository(DatabaseOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    
    public async Task<ErrorOr<List<User>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(
                "SELECT id, name, email, password_hash, created_at, updated_at FROM users ORDER BY id",
                connection
            );

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var users = new List<User>();
            while (await reader.ReadAsync(cancellationToken))
            {
                users.Add(MapToDomainEntity(reader));
            }

            return users;
        }
        catch (Exception ex)
        {
            return Error.Failure("Database.Error", $"Failed to retrieve users: {ex.Message}");
        }
    }

    public async Task<ErrorOr<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(
                "SELECT id, name, email, password_hash, created_at, updated_at FROM users WHERE id = @id",
                connection
            );

            command.Parameters.AddWithValue("id", id);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                return UserErrors.NotFound;
            }

            return MapToDomainEntity(reader);
        }
        catch (Exception ex)
        {
            return Error.Failure("Database.Error", $"Failed to retrieve user: {ex.Message}");
        }
    }

    public async Task<ErrorOr<User>> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(
                "SELECT id, name, email, password_hash, created_at, updated_at FROM users WHERE email = @email",
                connection
            );

            command.Parameters.AddWithValue("email", email);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                return UserErrors.NotFound;
            }

            return MapToDomainEntity(reader);
        }
        catch (Exception ex)
        {
            return Error.Failure("Database.Error", $"Failed to retrieve user by email: {ex.Message}");
        }
    }

    public async Task<ErrorOr<User>> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(
                @"INSERT INTO users(name, email, password_hash, created_at, updated_at)
                  VALUES(@name, @email, @password_hash, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
                  RETURNING id, name, email, password_hash, created_at, updated_at;",
                connection
            );

            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("email", user.Email);
            command.Parameters.AddWithValue("password_hash", user.PasswordHash);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                return Error.Failure("Database.Error", "Failed to create user");
            }

            return MapToDomainEntity(reader);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
        
            return UserErrors.DuplicateEmail;
        }
        catch (Exception ex)
        {
            return Error.Failure("Database.Error", $"Failed to create user: {ex.Message}");
        }
    }


    public async Task <ErrorOr<User>> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        
        try
        {


            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            var command = new NpgsqlCommand(

                @"UPDATE users
                SET
                name = @name,
                email = @email,
                password_hash = @passwordHash,
                updated_at = @updatedAt
                WHERE id = @id
                RETURNING id, name, email, password_hash, created_at, updated_at",
                connection
                
            );

            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("email", user.Email);
            command.Parameters.AddWithValue("passwordHash", user.PasswordHash);
            command.Parameters.AddWithValue("updatedAt", user.UpdatedAt);


            await using var reader = await command.ExecuteReaderAsync(cancellationToken);


            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return UserErrors.NotFound;

            }

            return MapToDomainEntity(reader);



        } 
        
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
                return UserErrors.DuplicateEmail;
        }


        catch (Exception ex)
        {

            return Error.Failure("Database.Error", $"Failed to update user: {ex.Message}");

        }


    } 


    public async Task<ErrorOr<bool>> DeleteUserAsync (int id, CancellationToken cancellationToken)
    {
        
        try
        {

        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new NpgsqlCommand(

         @"DELETE FROM users WHERE id = @id
         RETURNING id;",
         connection  


        );

        command.Parameters.AddWithValue("@id", id);
       

    await using var reader = await command.ExecuteReaderAsync(cancellationToken);

    if(!await reader.ReadAsync(cancellationToken))
            {
                
                return UserErrors.NotFound;

            }


        return true;



        }
        catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to delete user: {ex.Message}");


        }

    }


    private static User MapToDomainEntity(NpgsqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            CreatedAt = reader.GetDateTime(4),
            UpdatedAt = reader.GetDateTime(5)
        };
    }
}
