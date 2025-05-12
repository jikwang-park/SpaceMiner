using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingPopupUI : MonoBehaviour
{
    [SerializeField]
    private LocalizationText titleText;
    [SerializeField]
    private LocalizationText currentFirstText;
    [SerializeField]
    private TextMeshProUGUI currentFirstNicknameText;
    [SerializeField]
    private ScoreBoard currentFirstScoreBoard;
    [SerializeField]
    private RankingBoardUI rankingBoard;
    [SerializeField]
    private RankingElement myRankingElement;

    private const int topN = 50;
    private void OnEnable()
    {
        OnClickStageRankingButton();
    }
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
        titleText.SetString(100004);
        currentFirstText.SetString(100006);
        if (ranks == null || ranks.Count == 0)
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.Stage);
            return;
        }

        var first = ranks[0];

        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(true);
        currentFirstScoreBoard.SetBoard(RankingType.Stage, first.display);
        rankingBoard.Initialize(ranks, RankingType.Stage);

        var myRank = await FirebaseManager.Instance.GetMyHighestStageRankAsync();
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.Stage);
    }
    private async void SetCombatPowerRanking()
    {
        List<LeaderBoardEntry> ranks = await FirebaseManager.Instance.GetTopCombatPowerAsync(topN);
        titleText.SetString(100003);
        currentFirstText.SetString(100007);
        if (ranks == null || ranks.Count == 0)
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.CombatPower);
            return;
        }

        var first = ranks[0];

        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(true);
        currentFirstScoreBoard.SetBoard(RankingType.CombatPower, first.display);
        rankingBoard.Initialize(ranks, RankingType.CombatPower);

        var myRank = await FirebaseManager.Instance.GetMyCombatPowerRankAsync();
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.CombatPower);
    }
    private async void SetDungeonDamageRanking()
    {
        List<LeaderBoardEntry> ranks = await FirebaseManager.Instance.GetTopDungeonDamageAsync(topN);
        titleText.SetString(100005);
        currentFirstText.SetString(100008);
        if (ranks == null || ranks.Count == 0)
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.DungeonDamage);
            return;
        }

        var first = ranks[0];

        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(true);
        currentFirstScoreBoard.SetBoard(RankingType.DungeonDamage, first.display);
        rankingBoard.Initialize(ranks, RankingType.DungeonDamage);

        var myRank = await FirebaseManager.Instance.GetMyDungeonDamageRankAsync();
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.DungeonDamage);
    }
}
