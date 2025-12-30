using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.PaymentMethods.Data;

public class GetPaymentMethodsResult
{
    

    public List<PaymentMethod> PaymentMethods { get; set; } = new ();

}