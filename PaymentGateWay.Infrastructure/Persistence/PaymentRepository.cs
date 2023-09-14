using AutoMapper;
using PaymentGateWay.Application.DTOs;
using PaymentGateWay.Application.Repositories;
using PaymentGateWay.Domain;
using PaymentGateWay.Infrastructure.Contexts;

namespace PaymentGateWay.Infrastructure.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public PaymentRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public PaymentResponse CreatePayment(PaymentRequest request)
    {
        var payment = _mapper.Map<Payment>(request);
        payment.CreatedAt = payment.UpdatedAt = DateTime.Now;

        _dbContext.Payments.Add(payment);
        _dbContext.SaveChanges();

        return _mapper.Map<PaymentResponse>(payment);
    }

    public void DeletePaymentById(int productId)
    {
        var product = this._dbContext.Payments.Find(productId);
        if (product != null)
        {
            _dbContext.Payments.Remove(product);
            _dbContext.SaveChanges();
        }
        else
        {
           // throw new NotFoundException();
        }
    }

    public List<PaymentResponse>? GetPayments()
    {
        return this._dbContext.Payments.Select(p => _mapper.Map<PaymentResponse>(p)).ToList();
    }

    public PaymentResponse GetPaymentById(int productId)
    {
        var product = this._dbContext.Payments.Find(productId);
        if (product != null)
        {
            return _mapper.Map<PaymentResponse>(product);
        }
        return null;
        //throw new NotFoundException();
    }

}