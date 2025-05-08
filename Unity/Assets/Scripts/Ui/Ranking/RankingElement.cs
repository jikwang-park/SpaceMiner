using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI nicknameText;
    [SerializeField]
    private ScoreBoard scoreBoard;

    public void SetInfo(int grade, LeaderBoardEntry entry, RankingType type)
    {
        rankText.text = $"{grade}";
        nicknameText.text = entry.name;
        scoreBoard.SetBoard(type, entry.display);
    }
}
