using PaymentGateWay.Infrastructure.Interfaces;

namespace PaymentGateWay.Infrastructure.Messages;

public interface IIntegrationEventBuilder
{
    public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent);
    public string GetQueueName(IIntegrationEvent integrationEvent);
}