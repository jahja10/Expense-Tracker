using ErrorOr;
using SampleCkWebApp.Domain.Entities;


namespace SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;



public interface IPaymentMethodRepository
{
    

    Task<ErrorOr<List<PaymentMethod>>> GetPaymentMethodsAsync(int userId, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> GetPaymentMethodByIdAsync(int id, int userId, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> GetPaymentMethodByNameAsync(string name, int userId, CancellationToken cancellationToken);    

    Task<ErrorOr<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken);

    Task<ErrorOr<PaymentMethod>> UpdatePaymentMethodAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeletePaymentMethodAsync(int id, int userId, CancellationToken cancellationToken);







}