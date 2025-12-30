using System.Text.RegularExpressions;
using ErrorOr;
using SampleCkWebApp.Domain.Errors;

namespace SampleCkWebApp.Application.Categories;



public static class CategoryValidator {
public static ErrorOr<Success> ValidateCreateCategoryRequest(string name)
{

    if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return CategoryErrors.InvalidName;

    }

    if (Regex.IsMatch(name, @"\d"))
            return CategoryErrors.NameContainsNumbers;


    return Result.Success;


}

}