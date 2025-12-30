using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Transactions.Data;
using SampleCkWebApp.Transactions;

namespace SampleCkWebApp.Application.Transactions.Mappings;

public static class TransactionMappings
{
    public static GetTransactionsResponse ToResponse(this GetTransactionsResult result)
    {
        return new GetTransactionsResponse
        {
            Transactions = result.Transactions.Select(t => t.ToResponse()).ToList(),
            TotalCount = result.Transactions.Count
        };
    }

    public static TransactionResponse ToResponse(this Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            Price = transaction.Price,
            TransactionDate = transaction.TransactionDate,
            TransactionType = transaction.TransactionType.ToString(),
            Description = transaction.Description,
            Location = transaction.Location,
            UserId = transaction.UserId,
            CategoryId = transaction.CategoryId,
            PaymentMethodId = transaction.PaymentMethodId
        };
    }
}
