using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningRobotIcon : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> gradeSprites = new List<Sprite>();
    [SerializeField]
    private Image icon;
    public Sprite sprite
    {
        get { return icon.sprite; }
    }

    public Color color
    {
        get { return icon.color; }
        set { icon.color = value; }
    }
    public void Initialize(int robotId)
    {
        var data = DataTableManager.RobotTable.GetData(robotId);
        if(data == null)
        {
            icon.sprite = null;
            color = new Color(1, 1, 1, 0f);
        }
        else
        {
            icon.sprite = gradeSprites[(int)data.grade - 1];
            color = Color.white;
        }
    }
}
