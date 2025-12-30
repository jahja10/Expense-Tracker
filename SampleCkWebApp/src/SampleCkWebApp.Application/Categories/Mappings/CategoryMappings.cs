using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Categories.Data;
using SampleCkWebApp.Categories;

namespace SampleCkWebApp.Application.Categories.Mappings;


public static class CategoryMappings
{
    

    public static GetCategoriesResponse ToResponse(this GetCategoriesResult result)
    {
        

        return new GetCategoriesResponse
        {
            
            Categories = result.Categories.Select(c => c.ToResponse()).ToList(),
            TotalCount = result.Categories.Count

        };





    }

      public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }




}