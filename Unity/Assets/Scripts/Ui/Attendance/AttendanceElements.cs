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
    [SerializeField] 
    private Button claimButton;
    [SerializeField]
    private Image lockImage;

    private int attendanceId;
    private int dayIndex;

    public void Initialize(AttendanceRewardTable.Data data)
    {
        attendanceId = data.AttendanceID;
        dayIndex = data.Day;

        currencyImage.SetSprite(DataTableManager.ItemTable.GetData(data.RewardItemID).SpriteID);
        dayText.text = $"{dayIndex} ����";

        UpdateUI();
    }

    private void UpdateUI()
    {
        bool claimed = AttendanceRewardManager.Instance.IsClaimed(attendanceId, dayIndex);
        bool canClaim = AttendanceRewardManager.Instance.CanClaim(attendanceId, dayIndex);

        completeImage.gameObject.SetActive(claimed);
        canClaimPanel.gameObject.SetActive(canClaim && !claimed);
        claimButton.interactable = canClaim && !claimed;
        lockImage.gameObject.SetActive(!claimed && !canClaim);
    }
    public void OnClickButton()
    {
        if(AttendanceRewardManager.Instance.CanClaim(attendanceId, dayIndex))
        {
            AttendanceRewardManager.Instance.Claim(attendanceId, dayIndex);
            UpdateUI();
        }
    }
}
