using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Categories.Data;


namespace SampleCkWebApp.Application.Categories.Interfaces.Application;




public interface ICategoryService
{
    
    Task<ErrorOr<GetCategoriesResult>> GetCategoriesAsync (CancellationToken cancellationToken);

    Task <ErrorOr<Category>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

    Task <ErrorOr<Category>> CreateCategoryAsync (string name, CancellationToken cancellationToken);

    Task <ErrorOr<Category>> UpdateCategoryAsync (int id, string name, CancellationToken cancellationToken);

    Task <ErrorOr<bool>> DeleteCategoryAsync (int id, CancellationToken cancellationToken);
    
}

