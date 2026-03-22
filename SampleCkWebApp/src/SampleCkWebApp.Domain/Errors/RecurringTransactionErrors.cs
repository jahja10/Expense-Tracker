using ErrorOr;

namespace SampleCkWebApp.Domain.Errors;

public static class RecurringTransactionErrors
{


   

        public static Error NotFound =>
        Error.NotFound($"{nameof(CategoryErrors)}.{nameof(NotFound)}", "Transaction not found.");

        public static Error InvalidName =>
        Error.NotFound($"{nameof(CategoryErrors)}.{nameof(InvalidName)}", "Invalid name.");

        public static Error ForeignKeyMissing =>
         Error.NotFound($"{nameof(CategoryErrors)}.{nameof(NotFound)}", "One of foreign keys are missing.");

        public static Error InvalidTransactionType =>
        Error.Validation(
            code: "Transaction.InvalidTransactionType",
            description: "Invalid transaction type."
        );

        public static Error DuplicateName =>
        Error.Validation($"{nameof(RecurringTransactionErrors)}.{nameof(DuplicateName)}", "Name already exists.");
        public static Error NameContainsNumbers =>
         Error.Conflict("RecurringTransaction.NameContainsNumbers", "Transaction name must be without numbers.");

        

        

       

}