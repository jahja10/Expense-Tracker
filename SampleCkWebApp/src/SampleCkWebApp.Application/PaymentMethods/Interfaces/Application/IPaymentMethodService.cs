using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.PaymentMethods.Data;

namespace SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;


public interface IPaymentMethodService
{
    

    Task<ErrorOr<GetPaymentMethodsResult>> GetPaymentMethodsAsync(int userId, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> GetPaymentMethodByIdAsync(int id, int userId, CancellationToken cancellationToken);


    Task<ErrorOr<PaymentMethod>> CreatePaymentMethodAsync(string name, int userId, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> UpdatePaymentMethodAsync(int id, string name,int userId, CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeletePaymentMethodAsync(int id, int userId, CancellationToken cancellationToken);



}