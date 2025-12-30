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



    public async Task<ErrorOr<List<RecurringTransaction>>> GetRecurringTransactionsAsync(CancellationToken cancellationToken)
    {


        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                "SELECT id, name, frequency_of_transaction, next_run_date, user_id, category_id, payment_method_id FROM recurring_transaction ORDER BY id",
                connection

            );            

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

    public async Task <ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync (int id, CancellationToken cancellationToken)
    {
        

        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(

                "SELECT id, name, frequency_of_transaction, next_run_date, user_id, category_id, payment_method_id FROM recurring_transaction WHERE id = @id",
                 connection



            );

            command.Parameters.AddWithValue("id", id);
            

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
                (name, frequency_of_transaction, next_run_date, user_id, category_id, payment_method_id)
              VALUES 
                (@name, @frequency_of_transaction::frequency_of_transaction_enum, @next_run_date, @user_id, @category_id, @payment_method_id)
              RETURNING 
                id, name, frequency_of_transaction, next_run_date, user_id, category_id, payment_method_id;",
            connection
        );

        command.Parameters.AddWithValue("name", recurringTransaction.Name);
        command.Parameters.AddWithValue("frequency_of_transaction", recurringTransaction.FrequencyOfTransaction.ToString().ToLowerInvariant());
        command.Parameters.AddWithValue("next_run_date", (object?)recurringTransaction.NextRunDate ?? DBNull.Value);
        command.Parameters.AddWithValue("user_id", recurringTransaction.UserId);
        command.Parameters.AddWithValue("category_id", recurringTransaction.CategoryId);
        command.Parameters.AddWithValue("payment_method_id", recurringTransaction.PaymentMethodId);

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
                     next_run_date = @next_run_date,                   
                     category_id = @category_id,
                     payment_method_id = @payment_method_id

                WHERE id = @id
                RETURNING id, name, frequency_of_transaction, next_run_date, user_id, category_id, payment_method_id;",
                connection



            );

            command.Parameters.AddWithValue("name", recurringTransaction.Name);
            command.Parameters.AddWithValue("id", recurringTransaction.Id);
            command.Parameters.AddWithValue("frequency_of_transaction", recurringTransaction.FrequencyOfTransaction.ToString().ToLowerInvariant());
            command.Parameters.AddWithValue("next_run_date", (object?)recurringTransaction.NextRunDate ?? DBNull.Value);
            command.Parameters.AddWithValue("category_id", recurringTransaction.CategoryId);
            command.Parameters.AddWithValue("payment_method_id", recurringTransaction.PaymentMethodId);
            
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


     public async Task <ErrorOr<bool>> DeleteRecurringTransactionAsync(int id, CancellationToken cancellationToken)
    {
        
        try {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"DELETE FROM recurring_transaction 
            WHERE id = @id
            RETURNING  id;",
            connection



        );


        command.Parameters.AddWithValue("id", id);


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

        NextRunDate = reader.IsDBNull(3)
            ? (DateOnly?)null
            : DateOnly.FromDateTime(reader.GetDateTime(3)),

        UserId = reader.GetInt32(4),

        CategoryId = reader.GetInt32(5),

        PaymentMethodId = reader.GetInt32(6)
    };
}









}