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
    public void Initialize(Grade grade)
    {
        if(grade == Grade.None)
        {
            icon.sprite = null;
            color = new Color(1, 1, 1, 0f);
        }
        else
        {
            icon.sprite = gradeSprites[(int)grade - 1];
            color = Color.white;
        }
    }
}
