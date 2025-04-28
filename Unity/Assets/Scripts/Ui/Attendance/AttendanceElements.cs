using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceElements : MonoBehaviour
{
    [SerializeField]
    private Image canClaimPanel;
    [SerializeField]
    private AddressableImage currencyImage;
    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private Image completeImage;

    private int attendanceId;
    private int dayIndex;
    private bool isClaimed = false;

    public void Initialize(AttendanceRewardTable.Data data)
    {
        attendanceId = data.AttendanceID;
        dayIndex = data.Day;

        UpdateUI();
    }

    private void UpdateUI()
    {
        
    }
}
