namespace SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;

public class RecurringTransaction
{
    

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public FrequencyOfTransaction  FrequencyOfTransaction { get; set; }

    public DateOnly? NextRunDate { get; set; }

    public int UserId { get; set;}

    public int CategoryId { get; set;}

    public int PaymentMethodId { get; set;}



}