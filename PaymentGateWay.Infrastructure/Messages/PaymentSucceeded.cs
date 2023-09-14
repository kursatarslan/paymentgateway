namespace PaymentGateWay.Infrastructure.Messages;

public class PaymentSucceeded : IIntegrationEvent
{
    public Guid CorrelationId { get; private set; }
    public Guid UserId { get; private set; }

    public string Type { get; private set; }

    public PaymentSucceeded(Guid correlationId, Guid userId)
    {
        CorrelationId = correlationId;
        UserId = userId;
        Type = typeof(PaymentSucceeded).AssemblyQualifiedName;
    }
}