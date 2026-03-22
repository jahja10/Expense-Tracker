using ErrorOr;
using Npgsql;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Domain.Enums;
using SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Common;
using SampleCkWebApp.Contracts.Dashboard;


namespace SampleCkWebApp.Infrastructure.Transactions;


public class TransactionRepository : ITransactionRepository
{
    

    private readonly DatabaseOptions _options;

    public TransactionRepository(DatabaseOptions options)
    {
        
        _options = options ?? throw new ArgumentNullException(nameof(options));

    }



    public async Task<ErrorOr<List<Transaction>>> GetTransactionsAsync(int userId, CancellationToken cancellationToken)
    {


        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"SELECT id, price, transaction_date, transaction_type, description, location, user_id, category_id, payment_method_id FROM transaction 
                WHERE user_id = @user_id
                ORDER BY id",
                connection

            );     

            command.Parameters.AddWithValue("user_id", userId);       

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var transactions = new List<Transaction>();

            while(await reader.ReadAsync(cancellationToken)){
            
                transactions.Add(MapToDomainEntity(reader));
            
            }

            return transactions;



        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve transactions: {ex.Message}");

        }


    }

     public async Task <ErrorOr<Transaction>> GetTransactionByIdAsync (int id, int userId, CancellationToken cancellationToken)
    {
        

        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(

                @"SELECT  id, price, transaction_date, transaction_type, description, location, user_id, category_id, payment_method_id 
                FROM transaction WHERE id = @id AND user_id = @user_id",
                 connection



            );

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("user_id", userId);
            

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return TransactionErrors.NotFound;

            }

            return MapToDomainEntity(reader);



        }
        catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve transaction: {ex.Message}");


        }


    }    

     public async Task<ErrorOr<Transaction>> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
{
    try
    {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"INSERT INTO ""transaction"" 
                (price, transaction_date, transaction_type, description, location, user_id, category_id, payment_method_id)
              VALUES 
                (@price, @transaction_date, @transaction_type::transaction_type_enum, @description, @location, @user_id, @category_id, @payment_method_id)
              RETURNING 
                id, price, transaction_date, transaction_type, description, location, user_id, category_id, payment_method_id;",
            connection
        );

        command.Parameters.AddWithValue("price", (object?)transaction.Price ?? DBNull.Value);
        command.Parameters.AddWithValue("transaction_date", (object?)transaction.TransactionDate ?? DBNull.Value);
        command.Parameters.AddWithValue("transaction_type", transaction.TransactionType.ToString().ToLower());
        command.Parameters.AddWithValue("description", (object?)transaction.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("location", (object?)transaction.Location ?? DBNull.Value);
        command.Parameters.AddWithValue("user_id", transaction.UserId);
        command.Parameters.AddWithValue("category_id", transaction.CategoryId);
        command.Parameters.AddWithValue("payment_method_id", transaction.PaymentMethodId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))

            {
                
                
                return Error.Failure("Database.Error", "Failed to create transaction");
            }
            

        return MapToDomainEntity(reader);

    } catch (Exception ex)
        {
            return Error.Failure("Database.Error", $"Failed to create transaction: {ex.Message}");
        }
}

    public async Task<ErrorOr<Transaction>> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
    {   
        

         try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"UPDATE transaction
                SET
                     price = @price, 
                     transaction_date = @transaction_date,
                     transaction_type = @transaction_type::transaction_type_enum, 
                     description = @description,
                     location = @location, 
                     category_id = @category_id,
                     payment_method_id = @payment_method_id

                WHERE id = @id AND user_id = @user_id
                RETURNING id, price, transaction_date, transaction_type, description, location, user_id, category_id, payment_method_id;",
                connection



            );

            command.Parameters.AddWithValue("price", (object?)transaction.Price ?? DBNull.Value);
            command.Parameters.AddWithValue("id", transaction.Id);
            command.Parameters.AddWithValue("user_id", transaction.UserId);
            command.Parameters.AddWithValue("transaction_date", (object?)transaction.TransactionDate ?? DBNull.Value);
            command.Parameters.AddWithValue("transaction_type", transaction.TransactionType.ToString().ToLower());
            command.Parameters.AddWithValue("description", (object?)transaction.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("location", (object?)transaction.Location ?? DBNull.Value);
            command.Parameters.AddWithValue("category_id", transaction.CategoryId);
            command.Parameters.AddWithValue("payment_method_id", transaction.PaymentMethodId);
            
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                

                return TransactionErrors.NotFound;

            }
            
            return MapToDomainEntity(reader);


        }
        
        catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to update transaction: {ex.Message}");


        }   




    }


     public async Task <ErrorOr<bool>> DeleteTransactionAsync(int id, int userId, CancellationToken cancellationToken)
    {
        
        try {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"DELETE FROM transaction 
            WHERE id = @id AND user_id = @user_id
            RETURNING  id;",
            connection



        );


        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("user_id", userId);


        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if(!await reader.ReadAsync(cancellationToken))
        {
            return TransactionErrors.NotFound;
        }

        return true;



        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to delete transaction: {ex.Message}");

        }


    }


    public async Task<ErrorOr<List<DashboardTransactionsResponse>>> GetRecentTransactionsAsync( int userId, CancellationToken cancellationToken)
{
    try
    {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            @"
            SELECT
                t.price,
                t.transaction_date,
                t.transaction_type,
                t.description,
                t.location,
                c.name AS category_name,
                pm.method_name AS payment_method_name
            FROM ""transaction"" t
            JOIN category c ON t.category_id = c.id
            JOIN paymentmethod pm ON t.payment_method_id = pm.id
            WHERE t.user_id = @user_id
            ORDER BY t.transaction_date DESC, t.id DESC
            LIMIT 2;
            ",
            connection
                );

        command.Parameters.AddWithValue("user_id", userId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var transactions = new List<DashboardTransactionsResponse>();

        while (await reader.ReadAsync(cancellationToken))
        {
            transactions.Add(new DashboardTransactionsResponse(
            reader.IsDBNull(0) ? 0m : reader.GetDecimal(0),
           reader.IsDBNull(1)
           ? DateOnly.FromDateTime(DateTime.Today)
           : DateOnly.FromDateTime(reader.GetDateTime(1)),
            reader.GetString(2),
            reader.IsDBNull(3) ? null : reader.GetString(3),
            reader.IsDBNull(4) ? null : reader.GetString(4),
            reader.GetString(5),
            reader.GetString(6)
            ));
        }

        return transactions;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        return Error.Failure("Database.Error", $"Failed to retrieve recent transactions: {ex.Message}");
    }
}




    public static Transaction MapToDomainEntity(NpgsqlDataReader reader)
{
    return new Transaction
    {
        Id = reader.GetInt32(0),

        Price = reader.IsDBNull(1)
            ? (decimal?)null
            : reader.GetDecimal(1),

        TransactionDate = reader.IsDBNull(2)
            ? (DateOnly?)null
            : DateOnly.FromDateTime(reader.GetDateTime(2)),

        TransactionType = Enum.Parse<TransactionType>(
            reader.GetString(3), ignoreCase: true),

        Description = reader.IsDBNull(4)
            ? null
            : reader.GetString(4),

        Location = reader.IsDBNull(5)
            ? null
            : reader.GetString(5),

        UserId = reader.GetInt32(6),
        CategoryId = reader.GetInt32(7),
        PaymentMethodId = reader.GetInt32(8)
    };
}




}