using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonRequirementWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject oneButton;

    [SerializeField]
    private GameObject twoButtons;

    [SerializeField]
    private TextMeshProUGUI messageText;

    public enum Status
    {
        StageClear,
        Power,
        KeyCount,
    }

    public void Open(Status status)
    {
        gameObject.SetActive(true);
        switch (status)
        {
            case Status.StageClear:
                twoButtons.SetActive(false);
                oneButton.SetActive(true);
                messageText.text = "행성 클리어 조건 부족 메시지(임시)";
                break;
            case Status.Power:
                twoButtons.SetActive(false);
                oneButton.SetActive(true);
                messageText.text = "전투력 조건 부족 메시지(임시)";
                break;
            case Status.KeyCount:
                twoButtons.SetActive(true);
                oneButton.SetActive(false);
                messageText.text = "던전 열쇠 부족 메시지(임시)";
                break;
        }
    }
}
