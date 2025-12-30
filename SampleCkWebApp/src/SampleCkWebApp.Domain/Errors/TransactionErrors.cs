using ErrorOr;

namespace SampleCkWebApp.Domain.Errors;

public static class TransactionErrors
{


   

        public static Error NotFound =>
        Error.NotFound($"{nameof(CategoryErrors)}.{nameof(NotFound)}", "Transaction not found.");

        public static Error ForeignKeyMissing =>
         Error.NotFound($"{nameof(CategoryErrors)}.{nameof(NotFound)}", "One of foreign keys are missing.");

        public static Error InvalidTransactionType =>
        Error.Validation(
            code: "Transaction.InvalidTransactionType",
            description: "Invalid transaction type."
        );

        public static Error InvalidPrice =>
        Error.Validation(
            code: "Transaction.InvalidPrice",
            description: "Transaction price must be greater than 0 and within allowed range."
        );

        public static Error DescriptionTooLong =>
        Error.Validation(
            code: "Transaction.DescriptionTooLong",
            description: "Description cannot exceed 255 characters."
        );

        public static Error LocationTooLong =>
        Error.Validation(
            code: "Transaction.LocationTooLong",
            description: "Location cannot exceed 55 characters."
        );

        

       

}