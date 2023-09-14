namespace PaymentGateWay.Infrastructure.Messages;

public static class RabbitMqConsts
{
    public const string PaymentSucceededQueueName = "PaymentSucceeded";
    public const string PaymentFailedQueueName = "PaymentFailed";
    
    public const string PaymentApplicationName = "Payment";
}