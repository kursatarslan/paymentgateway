namespace PaymentGateWay.Application.Common;

public abstract class BaseEntity<T>
{
    public virtual T Id { get; set; }
    public virtual DateTime DateUpdated { get; set; }
}