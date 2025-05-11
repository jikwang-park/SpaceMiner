using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void SetBoard(RankingType type, string score)
    {
        image.sprite = sprites[(int)type];
        if(type == RankingType.Stage)
        {
            var stageData = DataTableManager.StageTable.GetData(int.Parse(score));
            scoreText.text = $"{stageData.Planet}-{stageData.Stage}";
            return;
        }
        scoreText.text = score;
    }
}
