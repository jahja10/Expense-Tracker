using ErrorOr;
namespace SampleCkWebApp.Domain.Errors;


public static class PaymentMethodErrors
{
    
    public static Error InvalidName =>
    Error.Validation($"{nameof(PaymentMethodErrors)}.{nameof(InvalidName)}", "Name must be between 1 and 100 characters.");

     public static Error NotFound =>
        Error.NotFound($"{nameof(PaymentMethodErrors)}.{nameof(NotFound)}", "Payment method not found.");

    public static Error DuplicateName =>
    Error.Validation($"{nameof(PaymentMethodErrors)}.{nameof(DuplicateName)}", "Name already exists.");

    public static Error NameContainsNumbers =>
         Error.Conflict("PaymentMethod.NameContainsNumbers", "Payment method name must be without numbers.");




}