using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.PaymentMethods.Data;

namespace SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;


public interface IPaymentMethodService
{
    

    Task<ErrorOr<GetPaymentMethodsResult>> GetPaymentMethodsAsync(CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> GetPaymentMethodByIdAsync(int id, CancellationToken cancellationToken);


    Task<ErrorOr<PaymentMethod>> CreatePaymentMethodAsync(string name, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> UpdatePaymentMethodAsync(int id, string name, CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeletePaymentMethodAsync(int id, CancellationToken cancellationToken);



}