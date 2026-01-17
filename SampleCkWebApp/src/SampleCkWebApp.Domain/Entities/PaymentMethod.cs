namespace SampleCkWebApp.Domain.Entities;



public class PaymentMethod
{
    


    public int Id { get; set;}

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set;}

    public int UserId { get; set; }





}