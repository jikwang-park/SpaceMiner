using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonEnterRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private int dungeonType;

    public event System.Action<int> OnButtonClicked;

    public void SetType(int type)
    {
        dungeonType = type;
        nameText.text = type.ToString();
        descriptionText.text = $"{type} Description";
    }

    public void OnButtonClick()
    {
        OnButtonClicked?.Invoke(dungeonType);
    }
}
