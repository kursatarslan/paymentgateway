namespace PaymentGateWay.Application.DTOs;

public class PaymentResponse
{
    public int Id  { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string CreditCardNumber { get; set; }
    public decimal Amount { get; set; }
}