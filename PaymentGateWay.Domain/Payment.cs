using System.ComponentModel.DataAnnotations;

namespace PaymentGateWay.Domain;

public class Payment 
{
    public int Id  { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email       { get; set; }
    public string CreditCardNumber       { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}