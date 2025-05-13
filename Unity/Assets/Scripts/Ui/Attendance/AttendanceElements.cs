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
    private LocalizationText dayText;
    [SerializeField]
    private TextMeshProUGUI amountText;
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
        dayText.SetStringArguments(dayIndex.ToString());
        BigNumber amount = data.RewardItemCount;
        amountText.text = amount.ToString();

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
