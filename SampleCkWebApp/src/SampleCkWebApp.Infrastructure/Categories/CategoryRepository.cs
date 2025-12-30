using ErrorOr;
using Npgsql;
using SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Infrastructure.Common;



namespace SampleCkWebApp.Infrastructure.Categories;



public class CategoryRepository : ICategoryRepository
{
    

    private readonly DatabaseOptions  _options;

    public CategoryRepository (DatabaseOptions  options)
    {
        
        _options = options ?? throw new ArgumentNullException(nameof(options));

    }

    public async Task<ErrorOr<List<Category>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {

        try
        {
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(


                "SELECT id, name, created_at, updated_at FROM category ORDER BY id",
                connection


            );


            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var categories = new List<Category>();

            while(await reader.ReadAsync(cancellationToken)){
            
                categories.Add(MapToDomainEntity(reader));
            
            }

            return categories;


        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve categories: {ex.Message}");

        }



    }



    public async Task <ErrorOr<Category>> GetCategoryByIdAsync (int id, CancellationToken cancellationToken)
    {
        

        try
        {

            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

           await using var command = new NpgsqlCommand(

                "SELECT id, name, created_at, updated_at FROM category WHERE id = @id",
                 connection



            );

            command.Parameters.AddWithValue("id", id);
            

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return CategoryErrors.NotFound;

            }

            return MapToDomainEntity(reader);



        }
        catch (Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve category: {ex.Message}");


        }





    }    

     public async Task<ErrorOr<Category>> GetCategoryByNameAsync (string name, CancellationToken cancellationToken)
    {
        

        try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(


                @"SELECT id, name, created_at, updated_at FROM category WHERE LOWER(name) = LOWER(@name)",
                connection


            );

            command.Parameters.AddWithValue("name", name);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                
                return CategoryErrors.NotFound;

            }

            return MapToDomainEntity(reader);

            


        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to retrieve category: {ex.Message}");


        }

        


    }



    public async Task<ErrorOr<Category>> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        

        try {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"INSERT INTO category(name, created_at, updated_at)
            VALUES (@name, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
            RETURNING id, name, created_at, updated_at", 
            connection

        );


        command.Parameters.AddWithValue("name", category.Name);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        

        if(!await reader.ReadAsync(cancellationToken))
        {
            

             return Error.Failure("Database.Error", "Failed to create category");

        }

        return MapToDomainEntity(reader);



        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return CategoryErrors.DuplicateName;
        }

        
        
        catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to create category: {ex.Message}");

        }
        

    }



    public async Task<ErrorOr<Category>> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {   
        

         try
        {
            
            await using var connection = new NpgsqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(

                @"UPDATE category
                SET
                    name = @name,
                    updated_at = @updatedAt
                WHERE id = @id
                RETURNING id, name, created_at, updated_at",
                connection



            );

            command.Parameters.AddWithValue("name", category.Name);
            command.Parameters.AddWithValue("id", category.Id);
            command.Parameters.AddWithValue("updated_at", category.UpdatedAt);
            
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if(!await reader.ReadAsync(cancellationToken))
            {
                

                return CategoryErrors.NotFound;

            }
            
            return MapToDomainEntity(reader);






        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return CategoryErrors.DuplicateName;
        }
        
        
        catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to update category: {ex.Message}");


        }   




    }



    public async Task <ErrorOr<bool>> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
    {
        
        try {
        await using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(

            @"DELETE FROM category 
            WHERE id = @id
            RETURNING  id;",
            connection



        );


        command.Parameters.AddWithValue("id", id);


        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if(!await reader.ReadAsync(cancellationToken))
        {
            return CategoryErrors.NotFound;
        }

        return true;



        }catch(Exception ex)
        {
            
            return Error.Failure("Database.Error", $"Failed to delete category: {ex.Message}");

        }


    }






    public static Category MapToDomainEntity(NpgsqlDataReader reader)
    {
        
        return new Category
        {
            

            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            CreatedAt = reader.GetDateTime(2),
            UpdatedAt = reader.GetDateTime(3)

        };



    }




}