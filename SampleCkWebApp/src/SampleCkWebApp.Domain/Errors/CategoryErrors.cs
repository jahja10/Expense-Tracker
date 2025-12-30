using ErrorOr;

namespace SampleCkWebApp.Domain.Errors;

public static class CategoryErrors
{


    public static Error InvalidName =>
        Error.Validation($"{nameof(CategoryErrors)}.{nameof(InvalidName)}", "Name must be between 1 and 100 characters.");

        public static Error NotFound =>
        Error.NotFound($"{nameof(CategoryErrors)}.{nameof(NotFound)}", "Category not found.");

        public static Error DuplicateName =>
         Error.Conflict("Category.DuplicateName", "Category name already exists.");

         public static Error NameContainsNumbers =>
         Error.Conflict("Category.NameContainsNumbers", "Category name must be without numbers.");



}