public class RAMUsageData
{
    public double Free { get; private set; } = 0;
    public double Active { get; private set; } = 0;
    public double Inactive { get; private set; } = 0;
    public double Wired { get; private set; } = 0;
    public double Compressed { get; private set; } = 0;

    public void SetData(double free, double active, double inactive, double wired, double compressed)
    {
        Free = free;
        Active = active;
        Inactive = inactive;
        Wired = wired;
        Compressed = compressed;
    }
}