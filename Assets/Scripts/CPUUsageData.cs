public class CPUUsageData
{
    public double System { get; private set; } = 0;
    public double User { get; private set; } = 0;
    public double Idle { get; private set; } = 0;
    public double Nice { get; private set; } = 0;

    public void SetData(double system, double user, double idle, double nice)
    {
        System = system;
        User = user;
        Idle = idle;
        Nice = nice;
    }
}