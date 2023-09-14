using AutoMapper;
using PaymentGateWay.Application.DTOs;
using PaymentGateWay.Domain;

namespace PaymentGateWay.Application.Mapper;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<PaymentRequest, Payment>();
        CreateMap<Payment, PaymentResponse>();
    }
    
}