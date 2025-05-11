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

        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "�������� ���൵ ����";
            currentFirstText.text = "�����Ͱ� �����ϴ�";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.Stage);
            return;
        }

        var first = ranks[0];

        titleText.text = "�������� ���൵ ����";
        currentFirstText.text = "���� 1��\n�������� ���൵";
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
        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "������ ����";
            currentFirstText.text = "�����Ͱ� �����ϴ�";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.CombatPower);
            return;
        }

        var first = ranks[0];

        titleText.text = "������ ����";
        currentFirstText.text = "���� 1��\n������";
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
        if (ranks == null || ranks.Count == 0)
        {
            titleText.text = "����2 ������ ����";
            currentFirstText.text = "�����Ͱ� �����ϴ�";
            currentFirstNicknameText.text = "--";
            currentFirstScoreBoard.gameObject.SetActive(false);
            rankingBoard.Initialize(new List<LeaderBoardEntry>(), RankingType.DungeonDamage);
            return;
        }

        var first = ranks[0];

        titleText.text = "����2 ������ ����";
        currentFirstText.text = "���� 1��\n����2 ������";
        currentFirstNicknameText.text = first.name;
        currentFirstScoreBoard.gameObject.SetActive(true);
        currentFirstScoreBoard.SetBoard(RankingType.DungeonDamage, first.display);
        rankingBoard.Initialize(ranks, RankingType.DungeonDamage);

        var myRank = await FirebaseManager.Instance.GetMyDungeonDamageRankAsync();
        myRankingElement.SetInfo(myRank.rank, myRank.myEntry, RankingType.DungeonDamage);
    }
}
