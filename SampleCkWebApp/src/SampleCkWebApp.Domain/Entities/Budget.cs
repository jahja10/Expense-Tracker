namespace SampleCkWebApp.Domain.Entities;

public class Budget
{
    
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public decimal? Savings { get; set; }    

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int UserId { get; set; }

  
}