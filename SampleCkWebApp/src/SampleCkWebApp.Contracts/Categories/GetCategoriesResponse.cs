using SampleCkWebApp.Categories;

namespace SampleCkWebApp.Categories;


public class GetCategoriesResponse
{
    

public List<CategoryResponse> Categories { get; set; } = new();


public int TotalCount { get; set; }



}