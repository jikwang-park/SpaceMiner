using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningRobotInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private List<Sprite> gradeSprites;
    public bool IsEmpty { get; private set; } = true;
    private int miningRobotId;

    public void Initialize(MiningRobotInventorySlotData data)
    {
        IsEmpty = data.isEmpty;
        miningRobotId = data.miningRobotId;   
        if(!IsEmpty)
        {
            image.sprite = gradeSprites[miningRobotId - 3000];
        }
    }
}
