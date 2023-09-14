namespace Merchant.HealthChecks.GCInfo
{
    public interface IGCInfoOptions
    {
        long Threshold { get; set; }
    }
}