using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Application.Categories.Data;
using SampleCkWebApp.Application.Categories.Interfaces.Application;
using SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;


namespace SampleCkWebApp.Application.Categories;




public class CategoryService : ICategoryService
{
    
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        
        _categoryRepository = categoryRepository;

    }



    public async Task <ErrorOr<GetCategoriesResult>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        
        var result = await _categoryRepository.GetCategoriesAsync(cancellationToken);
        if (result.IsError)
        {
            
            return result.Errors;

        }

        return new GetCategoriesResult
        {
               Categories = result.Value

        };

    }


    public async Task <ErrorOr<Category>> GetCategoryByIdAsync (int id, CancellationToken cancellationToken)
    {
        
        var result = await _categoryRepository.GetCategoryByIdAsync(id, cancellationToken);

        if(result.IsError)
        {
            
            return result.Errors;

        }

        return result;



    }




    public async Task<ErrorOr<Category>> CreateCategoryAsync (string name, CancellationToken cancellationToken)
    {
        

         var validationResult = CategoryValidator.ValidateCreateCategoryRequest(name);

        if (validationResult.IsError)
        {
            
            return validationResult.Errors;
        }

        var nameCheck = await _categoryRepository.GetCategoryByNameAsync(name, cancellationToken); 
        if (!nameCheck.IsError) 
        { 
            return CategoryErrors.DuplicateName;
        } 

        if (nameCheck.IsError && nameCheck.FirstError.Type != ErrorType.NotFound)
        {
            return nameCheck.Errors;
        }


        var category = new Category
        {

            Name = name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        };

        var createResult = await _categoryRepository.CreateCategoryAsync(category, cancellationToken);
        return createResult;


    }



    public async Task <ErrorOr<Category>> UpdateCategoryAsync (int id, string name, CancellationToken cancellationToken)
    {
        

        var existingResult = await _categoryRepository.GetCategoryByIdAsync(id, cancellationToken);
        if (existingResult.IsError)
        {
            
            return existingResult.Errors;

        }

        var existingCategory = existingResult.Value;


        var validationResult = CategoryValidator.ValidateCreateCategoryRequest(name);

        if (validationResult.IsError)
        {
            
            return validationResult.Errors;

        }

        var nameCheck = await _categoryRepository.GetCategoryByNameAsync(name, cancellationToken); 
        
        if (!nameCheck.IsError && nameCheck.Value.Id != id)
        {
            return CategoryErrors.DuplicateName;
        }

        if (nameCheck.IsError && nameCheck.FirstError.Type != ErrorType.NotFound)
        {
             return nameCheck.Errors;
        }
            


        var updatedCategory = new Category
        {
            Id = existingCategory.Id,
            Name = name,
            CreatedAt = existingCategory.CreatedAt,
            UpdatedAt = DateTime.UtcNow

        };

        var updatedResult = await _categoryRepository.UpdateCategoryAsync(updatedCategory, cancellationToken);
        return updatedResult;
        

    }




    public async Task <ErrorOr<bool>> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
    {
        
        var existingResult = await _categoryRepository.GetCategoryByIdAsync(id, cancellationToken);
        if(existingResult.IsError)
        {
            
            return existingResult.Errors;

        }

        var deleteResult = await _categoryRepository.DeleteCategoryAsync(id, cancellationToken);
        return deleteResult;


    }




}