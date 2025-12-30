using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Transactions.Data;


public class GetTransactionsResult

{
    
    public List<Transaction> Transactions { get; set; } = new();

}