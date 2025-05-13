using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public async void OnClickStageRankingButton()
    {
        await SetStageRanking();
    }
    public async void OnClickCombatPowerRankingButton()
    {
        await SetCombatPowerRanking();
    }
    public async void OnClickDungeonDamageRankingButton()
    {
        await SetDungeonDamageRanking();
    }
    private async Task SetStageRanking()
    {
        titleText.SetString(100004);
        currentFirstText.SetString(100006);

        var ranksTask = FirebaseManager.Instance.GetTopHighestStageAsync(topN);
        var myRankTask = FirebaseManager.Instance.GetMyHighestStageRankAsync();

        List<LeaderBoardEntry> ranks = null;
        MyRankEntry myRank = null;

        try
        {
            ranks = await ranksTask;
        }
        catch
        {
            ranks = new List<LeaderBoardEntry>();
        }

        if (ranks.Count > 0)
        {
            var first = ranks[0];
            currentFirstNicknameText.text = first.name;
            currentFirstScoreBoard.gameObject.SetActive(true);
            currentFirstScoreBoard.SetBoard(RankingType.Stage, first.display);
        }
        else
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
        }

        rankingBoard.Initialize(ranks, RankingType.Stage);

        try
        {
            myRank = await myRankTask;
        }
        catch
        {
            myRank = new MyRankEntry { rank = -1, myEntry = null };
        }
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.Stage);

        if (myRank.rank > 0 && myRank.rank <= topN)
        {
            rankingBoard.UpdateElement(myRank.rank);
        }
    }
    private async Task SetCombatPowerRanking()
    {
        titleText.SetString(100003);
        currentFirstText.SetString(100007);
        var ranksTask = FirebaseManager.Instance.GetTopCombatPowerAsync(topN);
        var myRankTask = FirebaseManager.Instance.GetMyCombatPowerRankAsync();

        List<LeaderBoardEntry> ranks = null;
        MyRankEntry myRank = null;

        try
        {
            ranks = await ranksTask;
        }
        catch
        {
            ranks = new List<LeaderBoardEntry>();
        }

        if (ranks.Count > 0)
        {
            var first = ranks[0];
            currentFirstNicknameText.text = first.name;
            currentFirstScoreBoard.gameObject.SetActive(true);
            currentFirstScoreBoard.SetBoard(RankingType.CombatPower, first.display);
        }
        else
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
        }

        rankingBoard.Initialize(ranks, RankingType.CombatPower);

        try
        {
            myRank = await myRankTask;
        }
        catch
        {
            myRank = new MyRankEntry { rank = -1, myEntry = null };
        }
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.CombatPower);

        if (myRank.rank > 0 && myRank.rank <= topN)
        {
            rankingBoard.UpdateElement(myRank.rank);
        }
    }
    private async Task SetDungeonDamageRanking()
    {
        titleText.SetString(100005);
        currentFirstText.SetString(100008);
        var ranksTask = FirebaseManager.Instance.GetTopDungeonDamageAsync(topN);
        var myRankTask = FirebaseManager.Instance.GetMyDungeonDamageRankAsync();

        List<LeaderBoardEntry> ranks = null;
        MyRankEntry myRank = null;

        try
        {
            ranks = await ranksTask;
        }
        catch
        {
            ranks = new List<LeaderBoardEntry>();
        }

        if (ranks.Count > 0)
        {
            var first = ranks[0];
            currentFirstNicknameText.text = first.name;
            currentFirstScoreBoard.gameObject.SetActive(true);
            currentFirstScoreBoard.SetBoard(RankingType.DungeonDamage, first.display);
        }
        else
        {
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
        }

        rankingBoard.Initialize(ranks, RankingType.DungeonDamage);

        try
        {
            myRank = await myRankTask;
        }
        catch
        {
            myRank = new MyRankEntry { rank = -1, myEntry = null };
        }
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.DungeonDamage);

        if (myRank.rank > 0 && myRank.rank <= topN)
        {
            rankingBoard.UpdateElement(myRank.rank);
        }
    }
}
