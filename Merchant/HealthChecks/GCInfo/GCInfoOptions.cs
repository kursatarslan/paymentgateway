namespace Merchant.HealthChecks.GCInfo
{
    public class GCInfoOptions : IGCInfoOptions
    {
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}