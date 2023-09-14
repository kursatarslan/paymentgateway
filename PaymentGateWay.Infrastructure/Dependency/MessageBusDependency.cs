using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateWay.Infrastructure.Dependency.Models;
using PaymentGateWay.Infrastructure.Interfaces;
using PaymentGateWay.Infrastructure.Messages;
using PaymentGateWay.Infrastructure.Services;

namespace PaymentGateWay.Infrastructure.Dependency;

public static class MessageBusDependencies
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, MessageBusOptions options)
    {
        services.AddMassTransit(x =>
        {
            options.Consumers(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(options.Host, h => {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });

                options.Endpoints(context, cfg);
            });
        });

        if (options.IsProducer)
            services.AddSingleton<IMassTransitHandler, MassTransitHandler>();

        services.AddSingleton(typeof(IIntegrationEventBuilder), options.IntegrationEventBuilderType);

        return services;
    }
}