using System.Reflection;
using ErrorOr;
using Npgsql;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Infrastructure.Common;



namespace SampleCkWebApp.Infrastructure.PaymentMethods;


public class PaymentMethodRepository : IPaymentMethodRepository
{
    
    private readonly DatabaseOptions _options;

    public PaymentMethodRepository(DatabaseOptions options)
    {
        
        _options = options ?? throw new ArgumentNullException(nameof(options));


    }


    public async Task<ErrorOr<List<PaymentMethod>>> GetPaymentMethodsAsync(int userId, CancellationToken cancellationToken)
    {
        
        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"SELECT id, method_name, created_at, updated_at, user_id FROM paymentmethod 
                WHERE user_id = @user_id
                ORDER BY id",
                connection



            );

            command.Parameters.AddWithValue("user_id", userId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var paymentMethods = new List<PaymentMethod>();

            while(await reader.ReadAsync(cancellationToken))
            {
                
                paymentMethods.Add(MapToDomainEntity(reader));

            } 


                return paymentMethods;




        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve PaymentMethods: {ex.Message}");


        }





    }

    public async Task<ErrorOr<PaymentMethod>> GetPaymentMethodByIdAsync(int id, int userId, CancellationToken cancellationToken)
    {
        
        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);


            await using var command = new NpgsqlCommand(

                @"SELECT id, method_name, created_at, updated_at, user_id FROM paymentmethod 
                WHERE id = @id AND user_id = @user_id",
                connection

            );

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("user_id", userId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return PaymentMethodErrors.NotFound;

            }

            return MapToDomainEntity(reader);



        }catch(Exception ex)
        {
            

            return Error.Failure("Database.Error", $"Failed to retrieve Payment method: {ex.Message}");

        }





    }


    public async Task<ErrorOr<PaymentMethod>> GetPaymentMethodByNameAsync (string name, int userId, CancellationToken cancellationToken)
    {
        

        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(


                @"SELECT id, method_name, created_at, updated_at, user_id 
                FROM paymentmethod WHERE LOWER(method_name) = LOWER(@method_name) AND user_id = @user_id",
                connection


            );

            command.Parameters.AddWithValue("method_name", name);
            command.Parameters.AddWithValue("user_id", userId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return PaymentMethodErrors.NotFound;

            }

            return MapToDomainEntity(reader);


        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve payment method: {ex.Message}");


        }




    }


    public async Task<ErrorOr<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken)
    {
        

        try
        {
            
           await using var connection = new NpgsqlConnection(_options.ConnectionString);
           await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(

            @"INSERT INTO paymentmethod(method_name, created_at, updated_at, user_id)
            VALUES(@method_name, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @user_id)
            RETURNING id, method_name, created_at, updated_at, user_id",
            connection

           );     

            command.Parameters.AddWithValue("method_name", paymentMethod.Name);
            command.Parameters.AddWithValue("user_id", paymentMethod.UserId);


            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return Error.Failure("Database.Error", $"Failed to create payment method");


            }

            return MapToDomainEntity(reader);


        }  catch (PostgresException ex) when (ex.SqlState == "23505")

        {
            return PaymentMethodErrors.DuplicateName;
        }
        
        
        catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to create Payment method: {ex.Message}");

        }


    }


    public async Task<ErrorOr<PaymentMethod>> UpdatePaymentMethodAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken)
    {
        

        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(


                @"UPDATE paymentmethod
                SET
                    method_name = @method_name,
                    updated_at = @updated_at,
                WHERE id = @id AND user_id = @user_id
                RETURNING id, method_name, created_at, updated_at, user_id",
                connection
                

            );

            command.Parameters.AddWithValue("method_name", paymentMethod.Name);
            command.Parameters.AddWithValue("id", paymentMethod.Id);
            command.Parameters.AddWithValue("user_id", paymentMethod.UserId);
            command.Parameters.AddWithValue("updated_at", paymentMethod.UpdatedAt);


            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return PaymentMethodErrors.NotFound;

            }


            return MapToDomainEntity(reader);



        } catch (PostgresException ex) when (ex.SqlState == "23505")

        {
            return PaymentMethodErrors.DuplicateName;
        }
        
        
        catch(Exception ex)
        {
            

            return Error.Failure("Database.Error", $"Failed to update payament method: {ex.Message}");

        }



    }


    public async Task<ErrorOr<bool>> DeletePaymentMethodAsync (int id,int userId, CancellationToken cancellationToken)
    {
        
        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"DELETE FROM paymentmethod
                WHERE id = @id AND user_id = @user_id
                RETURNING id, user_id;",
                connection


            );

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("user_id", userId);


             await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if(!await reader.ReadAsync(cancellationToken))
                {
                    return PaymentMethodErrors.NotFound;
                }

        return true;


        }catch(Exception ex)
        {
            

            return Error.Failure("Database.Error", $"Failed to delete payament method: {ex.Message}");

        }


    }




    public static PaymentMethod MapToDomainEntity(NpgsqlDataReader reader)
    {
        
        return new PaymentMethod
        {
            
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            CreatedAt = reader.GetDateTime(2),
            UpdatedAt = reader.GetDateTime(3),
            UserId = reader.GetInt32(4)


        };



    }




}
