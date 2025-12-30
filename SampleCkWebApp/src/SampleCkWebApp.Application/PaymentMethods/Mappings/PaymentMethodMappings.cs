using SampleCkWebApp.Application.PaymentMethods.Data;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.PaymentMethods;

namespace SampleCkWebApp.Application.PaymentMethods.Mappings;

public static class PaymentMethodMappings
{
    public static GetPaymentMethodsResponse ToResponse(this GetPaymentMethodsResult result)
    {
        return new GetPaymentMethodsResponse
        {
            PaymentMethods = result.PaymentMethods.Select(p => p.ToResponse()).ToList(),
            TotalCount = result.PaymentMethods.Count
        };
    }

    public static PaymentMethodResponse ToResponse(this PaymentMethod paymentMethod)
    {
        return new PaymentMethodResponse
        {
            Id = paymentMethod.Id,
            Name = paymentMethod.Name,
            CreatedAt = paymentMethod.CreatedAt,
            UpdatedAt = paymentMethod.UpdatedAt
        };
    }
}
