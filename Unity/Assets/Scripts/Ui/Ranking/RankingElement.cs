using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingElement : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI nicknameText;
    [SerializeField]
    private ScoreBoard scoreBoard;

    private Image backgroundImage;

    private readonly Color goldColor = new Color(1f, 0.84f, 0f, 1f);
    private readonly Color silverColor = new Color(0.75f, 0.75f, 0.75f, 1f);
    private readonly Color bronzeColor = new Color(0.72f, 0.45f, 0.20f, 1f);
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }
    public void SetInfo(int grade, LeaderBoardEntry entry, RankingType type, bool isMyEntry = false)
    {
        rankText.text = $"{grade}";
        nicknameText.text = entry.name;
        scoreBoard.SetBoard(type, entry.display);
        UpdateBackground(isMyEntry);
        UpdateImage(grade);
    }

    private void UpdateImage(int grade)
    {
        switch(grade)
        {
            case 1:
                image.color = goldColor;
                break;
            case 2:
                image.color = silverColor;
                break;
            case 3:
                image.color = bronzeColor;
                break;
            default:
                image.color = new Color(0f, 0f, 0f, 0f);
                break;
        }
    }
    public void UpdateBackground(bool isMyEntry)
    {
        backgroundImage.sprite = isMyEntry ? sprites[0] : sprites[1];
    }
}
