using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PaymentGateWay.Application.DTOs;
using PaymentGateWay.Application.Repositories;
using PaymentGateWay.Domain;
using PaymentGateWay.Infrastructure.Contexts;

namespace PaymentGateWay.Infrastructure.Services;

public class PaymentService : BaseService<Payment>
{
    private readonly IPaymentRepository _paymentRepository;
    
    public PaymentService(ApplicationDbContext context,IPaymentRepository paymentRepository): base(context)
    {
        _paymentRepository= paymentRepository;
    }

    public async Task<IReadOnlyList<Payment>> GetAllPaymentsAsync(Expression<Func<Payment, bool>>? filter = null, Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? orderBy = null, params Expression<Func<Payment, object>>[] includeProperties)
    {
        IQueryable<Payment> query = _table;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties)
            {
                query = query.Include(includeProp);
            }

        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }
    
    public List<PaymentResponse>? GetAllPayments( )
    {
        try
        {
            var products = _paymentRepository.GetPayments();
            return products;
        }
        catch (Exception)
        {
            return default;
        }
    }
    
    public PaymentResponse CreatePayment(PaymentRequest request)
    {
        var payment = _paymentRepository.CreatePayment(request);
        return payment;
    }
}
