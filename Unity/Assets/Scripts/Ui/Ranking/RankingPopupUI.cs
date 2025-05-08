using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingPopupUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI currentFirstText;
    [SerializeField]
    private TextMeshProUGUI currentFirstNicknameText;
    [SerializeField]
    private ScoreBoard currentFirstScoreBoard;
    [SerializeField]
    private RankingBoardUI rankingBoard;
    [SerializeField]
    private RankingElement myRankingElement;

    private const int topN = 50;
    public void OnClickStageRankingButton()
    {
        SetStageRanking();
    }
    public void OnClickCombatPowerRankingButton()
    {
        SetCombatPowerRanking();
    }
    public void OnClickDungeonDamageRankingButton()
    {
        SetDungeonDamageRanking();
    }
    private async void SetStageRanking()
    {
        List<LeaderBoardEntry> ranks = await FirebaseManager.Instance.GetTopHighestStageAsync(topN);

        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "전투력 순위";
            currentFirstText.text = "데이터가 없습니다";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.Stage);
            return;
        }

        var first = ranks[0];

        titleText.text = "스테이지 진행도 순위";
        currentFirstText.text = "현재 1등\n스테이지 진행도";
        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(false);
        currentFirstScoreBoard.SetBoard(RankingType.Stage, first.display);
        rankingBoard.Initialize(ranks, RankingType.Stage);
    }
    private async void SetCombatPowerRanking()
    {
        List<LeaderBoardEntry> ranks = await FirebaseManager.Instance.GetTopCombatPowerAsync(topN);
        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "전투력 순위";
            currentFirstText.text = "데이터가 없습니다";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.CombatPower);
            return;
        }

        var first = ranks[0];

        titleText.text = "전투력 순위";
        currentFirstText.text = "현재 1등\n전투력";
        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(false);
        currentFirstScoreBoard.SetBoard(RankingType.CombatPower, first.display);
        rankingBoard.Initialize(ranks, RankingType.CombatPower);
    }
    private async void SetDungeonDamageRanking()
    {
        List<LeaderBoardEntry> ranks = await FirebaseManager.Instance.GetTopDungeonDamageAsync(topN);
        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "전투력 순위";
            currentFirstText.text = "데이터가 없습니다";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.DungeonDamage);
            return;
        }

        var first = ranks[0];

        titleText.text = "던전2 데미지 순위";
        currentFirstText.text = "현재 1등\n던전2 데미지";
        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(false);
        currentFirstScoreBoard.SetBoard(RankingType.DungeonDamage, first.display);
        rankingBoard.Initialize(ranks, RankingType.DungeonDamage);
    }
}
