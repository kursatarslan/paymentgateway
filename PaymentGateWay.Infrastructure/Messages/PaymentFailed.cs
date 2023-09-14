namespace PaymentGateWay.Infrastructure.Messages;

public class PaymentFailed : IIntegrationEvent
{
    public Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }

    public string Type { get; private set; }

    public PaymentFailed(Guid correlationId, Guid userId)
    {
        CorrelationId = correlationId;
        UserId = userId;
        Type = typeof(PaymentFailed).AssemblyQualifiedName;
    }
}