using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateWay.Application.Interfaces;
using PaymentGateWay.Application.Repositories;
using PaymentGateWay.Infrastructure.Contexts;
using PaymentGateWay.Infrastructure.Persistence;
using PaymentGateWay.Infrastructure.Services;

namespace PaymentGateWay.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("WebApiDatabase");
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(defaultConnectionString,
                b => b.MigrationsAssembly("PaymentGateWay.Infrastructure")));

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<PaymentService>();
        return services;
    }
}