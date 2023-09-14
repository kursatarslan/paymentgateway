namespace PaymentGateWay.Infrastructure.Interfaces;

public interface IMassTransitHandler
{
    public Task Publish(object @event);
    public Task Send(string queueName, object @event);
}