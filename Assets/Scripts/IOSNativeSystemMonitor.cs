using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IOSNativeSystemMonitor : MonoBehaviour
{
    [SerializeField] private Button StartTrackingButton;
    [SerializeField] private Button StopTrackingButton;
    
    [SerializeField] private TMP_Text CPUUsageTMPText;
    [SerializeField] private TMP_Text GPUUsageTMPText;
    [SerializeField] private TMP_Text RAMUsageTMPText;

    private const string CPUUsageTextTemplate = "CPU Usage: {0}%";
    private const string GPUUsageTextTemplate = "GPU Usage: {0}%";
    private const string RAMUsageTextTemplate = "RAM Usage: {0}%";

    [DllImport("__Internal")]
    private static extern void StartTracking();

    [DllImport("__Internal")]
    private static extern void StopTracking();

    private void Start()
    {
        StartTrackingButton.onClick.AddListener(InvokeStartTracking);
        StopTrackingButton.onClick.AddListener(InvokeStopTracking);
    }

    private void InvokeStartTracking()
    {
        CPUUsageTMPText.text = string.Format(CPUUsageTextTemplate, 1);
        GPUUsageTMPText.text = string.Format(GPUUsageTextTemplate, 12);
        RAMUsageTMPText.text = string.Format(RAMUsageTextTemplate, 13);
    }

    private void InvokeStopTracking()
    {
    }
}