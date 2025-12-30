using ErrorOr;
using Npgsql;
using NpgsqlTypes;
using SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Infrastructure.Common;



public class BudgetRepository : IBudgetRepository
{
    

 private readonly DatabaseOptions  _options;

    public BudgetRepository (DatabaseOptions  options)
    {
        
        _options = options ?? throw new ArgumentNullException(nameof(options));
        Console.WriteLine("BUDGET repo ConnectionString = " + (_options.ConnectionString ?? "NULL"));

    }
    public async Task<ErrorOr<Budget?>>  GetActiveBudgetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        

        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"SELECT id, amount, savings, start_date, end_date, user_id FROM budget WHERE user_id = @user_id
                AND start_date <= @today
                AND (end_date IS NULL OR end_date >= @today)
                ORDER BY start_date DESC
                LIMIT 1",
                connection

            );

            

            
            command.Parameters.Add("user_id", NpgsqlDbType.Integer).Value = userId;
            command.Parameters.Add("today", NpgsqlDbType.Date).Value = DateTime.Today;

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                
                return default;

            }
                

            return MapToDomainEntity(reader);



        }catch (Exception ex)
{
    return Error.Failure(
        "Database.Error",
        $"Failed: {ex.GetType().Name} | {ex.Message} | STACK: {ex.StackTrace}"
    );
}

        
    }


    public async Task<ErrorOr<Budget>> CreateBudgetAsync(Budget budget, CancellationToken cancellationToken)
    {
        
         try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"INSERT INTO budget
                (amount, savings, start_date, end_date, user_id)
                VALUES(@amount, @savings, @start_date, @end_date, @user_id)
                RETURNING id, amount, savings, start_date, end_date, user_id;",
                connection
                

            );
            
            command.Parameters.AddWithValue("amount", budget.Amount);
            command.Parameters.AddWithValue("savings", (object?)budget.Savings ?? DBNull.Value);
            command.Parameters.AddWithValue("start_date", budget.StartDate.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("end_date", budget.EndDate is null
            ? DBNull.Value
            : budget.EndDate.Value.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("user_id", budget.UserId);
            

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            await reader.ReadAsync(cancellationToken);
            return MapToDomainEntity(reader);




        }catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to create budget for user: {ex.Message}");

        }





    }


     public async Task<ErrorOr<Budget>> CloseBudgetAsync(int id, DateOnly endDate, int userId, CancellationToken cancellationToken)
    {
        
         try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(
                
                @"UPDATE budget
                SET end_date = @end_date
                WHERE id = @id AND user_id = @user_id
                RETURNING id, amount, savings, start_date, end_date, user_id;",
                connection


            );

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("user_id", userId);
            command.Parameters.AddWithValue("end_date", endDate.ToDateTime(TimeOnly.MinValue));

            
           

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                
                return BudgetErrors.NotFound; 

            }

            return MapToDomainEntity(reader);


        }catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to close budget: {ex.Message}");

        }


    }

    public static Budget MapToDomainEntity(NpgsqlDataReader reader)
{
    return new Budget
    {
        Id = reader.GetInt32(0),

        Amount = reader.GetDecimal(1),

         Savings = reader.IsDBNull(2)
            ? (decimal?)null
            : reader.GetDecimal(2),

        StartDate = DateOnly.FromDateTime(reader.GetDateTime(3)), 

        EndDate = reader.IsDBNull(4)
            ? (DateOnly?)null
            : DateOnly.FromDateTime(reader.GetDateTime(4)),

        UserId = reader.GetInt32(5),
    };
}






}