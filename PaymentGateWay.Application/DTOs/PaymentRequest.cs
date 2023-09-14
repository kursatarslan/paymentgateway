namespace PaymentGateWay.Application.DTOs;

public class PaymentRequest
{
    public int Id  { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email       { get; set; }
    public string CreditCardNumber       { get; set; }
    public decimal Amount { get; set; }
 }