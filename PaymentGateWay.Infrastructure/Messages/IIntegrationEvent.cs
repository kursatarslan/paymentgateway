namespace PaymentGateWay.Infrastructure.Messages;

public interface IIntegrationEvent
{
    public string Type { get; }
}