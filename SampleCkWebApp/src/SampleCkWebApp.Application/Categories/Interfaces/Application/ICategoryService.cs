using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Categories.Data;


namespace SampleCkWebApp.Application.Categories.Interfaces.Application;




public interface ICategoryService
{
    
    Task<ErrorOr<GetCategoriesResult>> GetCategoriesAsync (int userId, CancellationToken cancellationToken);

    Task <ErrorOr<Category>> GetCategoryByIdAsync(int id, int userId, CancellationToken cancellationToken);

    Task <ErrorOr<Category>> CreateCategoryAsync (string name, int userId, CancellationToken cancellationToken);

    Task <ErrorOr<Category>> UpdateCategoryAsync (int id, string name, int userId, CancellationToken cancellationToken);

    Task <ErrorOr<bool>> DeleteCategoryAsync (int id, int userId, CancellationToken cancellationToken);
    
}

