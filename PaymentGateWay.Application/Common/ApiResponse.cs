namespace PaymentGateWay.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public T Data { get; set; }
}