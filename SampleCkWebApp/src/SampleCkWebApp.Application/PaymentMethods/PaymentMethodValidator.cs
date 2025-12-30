using System.Text.RegularExpressions;
using ErrorOr;
using SampleCkWebApp.Domain.Errors;

namespace SampleCkWebApp.Application.PaymentMethods;



public static class PaymentMethodValidator {
public static ErrorOr<Success> ValidateCreatePaymentMethodRequest(string name)
{

    if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return PaymentMethodErrors.InvalidName;

    }

    if (Regex.IsMatch(name, @"\d"))
            return CategoryErrors.NameContainsNumbers;


    return Result.Success;


}

}