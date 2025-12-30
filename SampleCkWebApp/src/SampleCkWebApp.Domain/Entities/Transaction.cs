using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Domain.Entities;

public class Transaction
{
    

    public int Id { get; set; }

    public decimal? Price { get; set; } 

    public DateOnly? TransactionDate { get; set; }

    public TransactionType TransactionType { get; set; }

    public string? Description { get; set;} 

    public string? Location { get; set; } 

    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int PaymentMethodId { get; set; }

    


}