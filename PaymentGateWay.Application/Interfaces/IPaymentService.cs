using System.Linq.Expressions;
using PaymentGateWay.Application.DTOs;
using PaymentGateWay.Domain;

namespace PaymentGateWay.Application.Interfaces;

public interface IPaymentService : IGenericRepository<Payment>
{
    Task<IReadOnlyList<Payment>> GetAllPaymentsAsync(Expression<Func<Payment, bool>>? filter = null, Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? orderBy = null, params Expression<Func<Payment, object>>[] includeProperties);
    PaymentResponse CreatePayment(PaymentRequest request);
}