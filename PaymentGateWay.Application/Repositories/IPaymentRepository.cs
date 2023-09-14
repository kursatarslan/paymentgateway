using PaymentGateWay.Application.DTOs;

namespace PaymentGateWay.Application.Repositories;

public interface IPaymentRepository
{
    List<PaymentResponse>? GetPayments();

    PaymentResponse GetPaymentById(int paymentId);

    void DeletePaymentById(int paymentId);
    PaymentResponse CreatePayment(PaymentRequest request);
}
