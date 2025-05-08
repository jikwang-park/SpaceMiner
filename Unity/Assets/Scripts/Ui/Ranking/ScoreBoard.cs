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
        scoreText.text = score;
    }
}
