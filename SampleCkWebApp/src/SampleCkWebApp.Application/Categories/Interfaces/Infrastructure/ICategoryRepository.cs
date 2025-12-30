using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;



public interface ICategoryRepository
{
    
        Task<ErrorOr<List<Category>>> GetCategoriesAsync (CancellationToken cancellationToken);

        Task <ErrorOr<Category>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

        Task<ErrorOr<Category>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken);  

        Task <ErrorOr<Category>> CreateCategoryAsync(Category category, CancellationToken cancellationToken);

        Task <ErrorOr<Category>> UpdateCategoryAsync (Category category, CancellationToken cancellationToken);

        Task <ErrorOr<bool>> DeleteCategoryAsync (int id, CancellationToken cancellationToken);




}