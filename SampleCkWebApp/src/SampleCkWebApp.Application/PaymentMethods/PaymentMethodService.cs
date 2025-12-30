using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Application.PaymentMethods.Data;

namespace SampleCkWebApp.Application.PaymentMethods;


public class PaymentMethodService : IPaymentMethodService
{
    
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
    {
        
        _paymentMethodRepository = paymentMethodRepository;

    }





    public async Task<ErrorOr<GetPaymentMethodsResult>> GetPaymentMethodsAsync(CancellationToken cancellationToken)
    {
        

        var result = await _paymentMethodRepository.GetPaymentMethodsAsync(cancellationToken);

        if(result.IsError)
        {
            
            return result.Errors;

        }

        return new GetPaymentMethodsResult
        {

            PaymentMethods = result.Value

        };




    }


    public async Task<ErrorOr<PaymentMethod>> GetPaymentMethodByIdAsync(int id, CancellationToken cancellationToken)
    {
        

        var result = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id, cancellationToken);
        return result;



    }


    public async Task<ErrorOr<PaymentMethod>> CreatePaymentMethodAsync(string name, CancellationToken cancellationToken)
    {
        
        var validationResult = PaymentMethodValidator.ValidateCreatePaymentMethodRequest(name);

        if (validationResult.IsError)
        {
            
            return validationResult.Errors;
        }

        var nameCheck = await _paymentMethodRepository.GetPaymentMethodByNameAsync(name, cancellationToken); 
        if (!nameCheck.IsError) 
        { 
            return PaymentMethodErrors.DuplicateName;
        } 

        if (nameCheck.IsError && nameCheck.FirstError.Type != ErrorType.NotFound)
        {
            return nameCheck.Errors;
        }
        
        

        
        var paymentMethod = new PaymentMethod {

            Name = name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        };

        
        var createResult = await _paymentMethodRepository.CreatePaymentMethodAsync(paymentMethod, cancellationToken);
        return createResult;


    }

    public async Task<ErrorOr<PaymentMethod>> UpdatePaymentMethodAsync (int id, string name, CancellationToken cancellationToken)
    {
        

        var existingResult = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id, cancellationToken);

        if(existingResult.IsError)
        {
            return existingResult.Errors;

        }

        var existingPaymentMethod = existingResult.Value;

        var validationResult = PaymentMethodValidator.ValidateCreatePaymentMethodRequest(name);

        if (validationResult.IsError)
        {
            
            return validationResult.Errors;

        }

        var nameCheck = await _paymentMethodRepository.GetPaymentMethodByNameAsync(name, cancellationToken); 
        
        if (!nameCheck.IsError && nameCheck.Value.Id != id)
        {
            return PaymentMethodErrors.DuplicateName;
        }

        if (nameCheck.IsError && nameCheck.FirstError.Type != ErrorType.NotFound)
        {
             return nameCheck.Errors;
        }


        var paymentMethod = new PaymentMethod
        {
            
            Id = existingPaymentMethod.Id,
            Name = name,
            CreatedAt = existingPaymentMethod.CreatedAt,
            UpdatedAt = DateTime.UtcNow


        };

        var updatePaymentMethod = await _paymentMethodRepository.UpdatePaymentMethodAsync(paymentMethod, cancellationToken);
        return updatePaymentMethod;



    }



    public async Task<ErrorOr<bool>> DeletePaymentMethodAsync(int id, CancellationToken cancellationToken)
    {
        
         var existingResult = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id, cancellationToken);

        if(existingResult.IsError)
        {
            return existingResult.Errors;

        }
            
        var deletePaymentMethod = await _paymentMethodRepository.DeletePaymentMethodAsync(id, cancellationToken);
        return deletePaymentMethod; 



    }




}