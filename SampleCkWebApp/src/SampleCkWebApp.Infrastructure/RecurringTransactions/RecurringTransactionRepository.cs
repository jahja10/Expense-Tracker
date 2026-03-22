using ErrorOr;
using Npgsql;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Domain.Enums;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Common;


namespace SampleCkWebApp.Infrastructure.RecurringTransactions;



public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    

 private readonly DatabaseOptions _options;

    public RecurringTransactionRepository(DatabaseOptions options)
    {
        
        _options = options ?? throw new ArgumentNullException(nameof(options));

    }



    public async Task<ErrorOr<List<RecurringTransaction>>> GetRecurringTransactionsAsync(int userId, CancellationToken cancellationToken)
    {


        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"SELECT id, name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active
                FROM recurring_transaction 
                WHERE user_id = @user_id
                ORDER BY id",
                connection

            );            

            command.Parameters.AddWithValue("user_id", userId);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var recurringTransactions = new List<RecurringTransaction>();

            while(await reader.ReadAsync(cancellationToken)){
            
                recurringTransactions.Add(MapToDomainEntity(reader));
            
            }

            return recurringTransactions;



        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve transactions: {ex.Message}");

        }


    }

    public async Task <ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync (int id, int userId, CancellationToken cancellationToken)
    {
        

        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(

                @"SELECT id, name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active 
                FROM recurring_transaction 
                WHERE id = @id AND user_id = @user_id",
                connection



            );

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("user_id", userId);
            

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return RecurringTransactionErrors.NotFound;

            }

            return MapToDomainEntity(reader);



        }
        catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve transaction: {ex.Message}");


        }


    }    

     public async Task<ErrorOr<RecurringTransaction>> CreateRecurringTransactionAsync(RecurringTransaction recurringTransaction, CancellationToken cancellationToken)
{
    try
    {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"INSERT INTO ""recurring_transaction"" 
                (name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active)
              VALUES 
                (@name, @frequency_of_transaction::frequency_of_transaction_enum, @user_id, @category_id, @payment_method_id, @amount, @start_date, @last_generated_date, @is_active )
              RETURNING 
                id, name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active;",
            connection
        );

        command.Parameters.AddWithValue("name", recurringTransaction.Name);
        command.Parameters.AddWithValue("frequency_of_transaction", recurringTransaction.FrequencyOfTransaction.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("user_id", recurringTransaction.UserId);
        command.Parameters.AddWithValue("category_id", recurringTransaction.CategoryId);
        command.Parameters.AddWithValue("payment_method_id", recurringTransaction.PaymentMethodId);
        command.Parameters.AddWithValue("amount", recurringTransaction.Amount);
         command.Parameters.AddWithValue("start_date", recurringTransaction.StartDate);
        command.Parameters.AddWithValue("last_generated_date", (object?)recurringTransaction.LastGeneratedDate ?? DBNull.Value);
        command.Parameters.AddWithValue("is_active", recurringTransaction.IsActive);

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

    public async Task<ErrorOr<RecurringTransaction>> GetRecurringTransactionByNameAsync (string name, int userId, CancellationToken cancellationToken)
        {
            

            try
            {
                
                await using var connection = new NpgsqlConnection(_options.ConnectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand(


                    @"SELECT id, name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active
                    FROM recurring_transaction WHERE LOWER(name) = LOWER(@name) AND user_id = @user_id",
                    connection


                );

                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("user_id", userId);

                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if(!await reader.ReadAsync(cancellationToken))
                {
                    
                    return RecurringTransactionErrors.NotFound;

                }

                return MapToDomainEntity(reader);


            }catch(Exception ex)
            {
                
                return Error.Failure("Database.Error", $"Failed to retrieve recurring transaction: {ex.Message}");


            }

        }


    public async Task<ErrorOr<RecurringTransaction>> UpdateRecurringTransactionAsync(RecurringTransaction recurringTransaction, CancellationToken cancellationToken)
    {   
        

         try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"UPDATE recurring_transaction
                SET
                     name = @name, 
                     frequency_of_transaction = @frequency_of_transaction::frequency_of_transaction_enum,                   
                     category_id = @category_id,
                     payment_method_id = @payment_method_id,
                     amount = @amount,
                     start_date = @start_date,
                     last_generated_date = @last_generated_date,
                     is_active = @is_active

                WHERE id = @id AND user_id = @user_id
                RETURNING id, name, frequency_of_transaction, user_id, category_id, payment_method_id, amount, start_date, last_generated_date, is_active;",
                connection



            );

            command.Parameters.AddWithValue("name", recurringTransaction.Name);
            command.Parameters.AddWithValue("id", recurringTransaction.Id);
            command.Parameters.AddWithValue("user_id", recurringTransaction.UserId);
            command.Parameters.AddWithValue("frequency_of_transaction", recurringTransaction.FrequencyOfTransaction.ToString().ToLowerInvariant());
            command.Parameters.AddWithValue("category_id", recurringTransaction.CategoryId);
            command.Parameters.AddWithValue("payment_method_id", recurringTransaction.PaymentMethodId);
            command.Parameters.AddWithValue("amount", recurringTransaction.Amount);
            command.Parameters.AddWithValue("start_date", recurringTransaction.StartDate);
            command.Parameters.AddWithValue("last_generated_date", (object?)recurringTransaction.LastGeneratedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("is_active", recurringTransaction.IsActive);
            
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                

                return RecurringTransactionErrors.NotFound;

            }
            
            return MapToDomainEntity(reader);


        }
        
        catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to update transaction: {ex.Message}");


        }   




    }

    


     public async Task <ErrorOr<bool>> DeleteRecurringTransactionAsync(int id, int userId, CancellationToken cancellationToken)
    {
        
        try {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"DELETE FROM recurring_transaction 
            WHERE id = @id AND user_id = @user_id
            RETURNING  id, user_id;",
            connection



        );


        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("user_id", userId);


        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if(!await reader.ReadAsync(cancellationToken))
        {
            return RecurringTransactionErrors.NotFound;
        }

        return true;



        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to delete transaction: {ex.Message}");

        }


    }






   public static RecurringTransaction MapToDomainEntity(NpgsqlDataReader reader)
{
    return new RecurringTransaction
    {
        Id = reader.GetInt32(0),

        Name = reader.GetString(1),

        FrequencyOfTransaction = Enum.Parse<FrequencyOfTransaction>(
            reader.GetString(2), ignoreCase: true),

        UserId = reader.GetInt32(3),

        CategoryId = reader.GetInt32(4),

        PaymentMethodId = reader.GetInt32(5),

        Amount = reader.GetDecimal(6),

        StartDate = DateOnly.FromDateTime(reader.GetDateTime(7)),

        LastGeneratedDate = reader.IsDBNull(8)
            ? (DateOnly?)null
            : DateOnly.FromDateTime(reader.GetDateTime(8)),

        IsActive = reader.GetBoolean(9)

    };
}




}