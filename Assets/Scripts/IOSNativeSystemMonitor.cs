using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AOT;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class IOSNativeSystemMonitor : MonoBehaviour
{
    [SerializeField] private Button StartTrackingButton;
    [SerializeField] private Button StopTrackingButton;

    [SerializeField] private TMP_Text CPUUsageTMPText;
    [SerializeField] private TMP_Text GPUUsageTMPText;
    [SerializeField] private TMP_Text RAMUsageTMPText;

    private const string CPUUsageTextTemplate =
        "<b><size=200%>--CPU--</size></b>\nSystem: {0:N2}%\nUser: {1:N2}%\nIdle: {2:N2}%\nNice: {3:N2}%";

    private const string GPUUsageTextTemplate = "<b><size=200%>--GPU--</size></b>\nAllocated Size: {0}";

    private const string RAMUsageTextTemplate =
        "<b><size=200%>--RAM--</size></b>\nFree: {0}\nActive: {1}\nInactive: {2}\nWired: {3}\nCompressed: {4}";

    private static CPUUsageData _cpuUsageData;
    private static int _gpuUsage;
    private static RAMUsageData _ramUsageData;

    private bool _isTracking;

    #region iOS Native Integration

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void StartTracking(CPUUsageDataCallback cpuUsageDataCallbackHandler,
        GPUUsageDataCallback gpuUsageDataCallbackHandler, RAMUsageDataCallback ramUsageDataCallbackHandler);

    [DllImport("__Internal")]
    private static extern void StopTracking();
#endif

    private delegate void CPUUsageDataCallback(double system, double user, double idle, double nice);

    private delegate void GPUUsageDataCallback(int allocatedSize);

    private delegate void RAMUsageDataCallback(double free, double active, double inactive, double wired,
        double compressed);

    [MonoPInvokeCallback(typeof(CPUUsageDataCallback))]
    private static void CPUUsageDataCallbackHandler(double system, double user, double idle,
        double nice)
    {
        _cpuUsageData.SetData(system, user, idle, nice);
    }

    [MonoPInvokeCallback(typeof(GPUUsageDataCallback))]
    private static void GPUUsageDataCallbackHandler(int allocatedSize)
    {
        _gpuUsage = allocatedSize;
    }

    [MonoPInvokeCallback(typeof(RAMUsageDataCallback))]
    private static void RAMUsageDataCallbackHandler(double free, double active, double inactive, double wired,
        double compressed)
    {
        _ramUsageData.SetData(free, active, inactive, wired, compressed);
    }

    #endregion

    private void InvokeStartTracking()
    {
        _isTracking = true;

#if UNITY_IOS
        StartTracking(CPUUsageDataCallbackHandler, GPUUsageDataCallbackHandler, RAMUsageDataCallbackHandler);
#endif
    }

    private void InvokeStopTracking()
    {
        _isTracking = false;

#if UNITY_IOS
        StopTracking();
#endif
    }

    private void UpdateUsageTMPText()
    {
        CPUUsageTMPText.text = string.Format(CPUUsageTextTemplate, _cpuUsageData.System, _cpuUsageData.User,
            _cpuUsageData.Idle, _cpuUsageData.Nice);
        GPUUsageTMPText.text = string.Format(GPUUsageTextTemplate, MemorySizeToString(_gpuUsage));
        RAMUsageTMPText.text = string.Format(RAMUsageTextTemplate, MemorySizeToString(_ramUsageData.Free),
            MemorySizeToString(_ramUsageData.Active), MemorySizeToString(_ramUsageData.Inactive),
            MemorySizeToString(_ramUsageData.Wired), MemorySizeToString(_ramUsageData.Compressed));
    }

    private string MemorySizeToString(double usageInBytes)
    {
        double usageDisplayValue = usageInBytes / 1000 / 1000; // Converted to MB
        if (!(usageDisplayValue >= 1000)) return $"{usageDisplayValue:N2} MB";

        usageDisplayValue /= 1000;
        return $"{usageDisplayValue:N2} GB";
    }

    #region Unity Events

    private void Start()
    {
        StartTrackingButton.onClick.AddListener(InvokeStartTracking);
        StopTrackingButton.onClick.AddListener(InvokeStopTracking);

        _cpuUsageData = new CPUUsageData();
        _gpuUsage = 0;
        _ramUsageData = new RAMUsageData();

        UpdateUsageTMPText();
    }

    private void Update()
    {
        if (!_isTracking) return;

        UpdateUsageTMPText();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;

        InvokeStopTracking();
    }

    #endregion
}