using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Domain.Errors;

public static class BudgetErrors
{


        public static Error NotFound =>
        Error.NotFound($"{nameof(Budget)}.{nameof(NotFound)}", "Budget not found.");

         public static Error ForeignKeyMissing =>
         Error.Validation($"{nameof(Budget)}.{nameof(ForeignKeyMissing)}", "User does not exist..");

         public static Error InvalidAmount =>
         Error.Validation($"{nameof(Budget)}.{nameof(InvalidAmount)}", "Min. amount for budget is 100.");

          public static Error InvalidDate =>
         Error.Validation($"{nameof(Budget)}.{nameof(InvalidDate)}", "Start date must be before end date");

}