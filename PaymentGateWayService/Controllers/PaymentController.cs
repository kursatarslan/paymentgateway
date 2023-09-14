using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateWay.Application.Common;
using PaymentGateWay.Application.DTOs;
using PaymentGateWay.Application.Interfaces;
using PaymentGateWay.Domain;
using PaymentGateWay.Infrastructure.Services;

namespace PaymentGateWayService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;
    public PaymentController(PaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }
    
    public async Task<List<Payment>> Index()
    {
        var payments = await _paymentService.GetAllPaymentsAsync(filter: null, orderBy: null, a => a.FirstName, c => c.Email);

        return payments.ToList();
    }
        
    [HttpPost]
    public ApiResponse<PaymentResponse> Post([FromBody] PaymentRequest paymentRequest)
    {
        var response = new ApiResponse<PaymentResponse>();

        try
        {
            var payment = _paymentService.CreatePayment(paymentRequest);

            response.Success = true;
            response.Data = payment;
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An unexpected error occurred.");

            response.Success = false;
            response.ErrorMessage = "An unexpected error occurred while processing the payment.";
        }

        return response;
    }
}