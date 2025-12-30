using SampleCkWebApp.PaymentMethods;

namespace SampleCkWebApp.PaymentMethods;


public class GetPaymentMethodsResponse
{
    

public List<PaymentMethodResponse> PaymentMethods { get; set; } = new();


public int TotalCount { get; set; }



}