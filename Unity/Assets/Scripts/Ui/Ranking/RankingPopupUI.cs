using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingPopupUI : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    private Toggle stageToggle;
    [SerializeField]
    private Toggle combatPowerToggle;
    [SerializeField]
    private Toggle dungeonDamageToggle;
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

    private Image stageToggleImage;
    private Image combatPowerToggleImage;
    private Image dungeonDamageToggleImage;

    private const int topN = 50;
    private void Awake()
    {
        stageToggleImage = stageToggle.GetComponent<Image>();
        combatPowerToggleImage = combatPowerToggle.GetComponent<Image>();
        dungeonDamageToggleImage = dungeonDamageToggle.GetComponent<Image>();
    }

    public void ProcessToggles()
    {
        if (stageToggle.isOn)
        {
            OnClickStageRankingButton();
        }
        else if (combatPowerToggle.isOn)
        {
            OnClickCombatPowerRankingButton();
        }
        else if (dungeonDamageToggle.isOn)
        {
            OnClickDungeonDamageRankingButton();
        }

        UpdateToggleSprites();
    }
    private void UpdateToggleSprites()
    {
        stageToggleImage.sprite = stageToggle.isOn ? sprites[1] : sprites[0];
        combatPowerToggleImage.sprite = combatPowerToggle.isOn ? sprites[1] : sprites[0];
        dungeonDamageToggleImage.sprite = dungeonDamageToggle.isOn ? sprites[1] : sprites[0];
    }
    private void OnEnable()
    {
        stageToggle.isOn = false;
        combatPowerToggle.isOn = false;
        dungeonDamageToggle.isOn = false;
        stageToggle.isOn = true;
    }
    public async void OnClickStageRankingButton()
    {
        stageToggle.interactable = false;
        combatPowerToggle.interactable = false;
        dungeonDamageToggle.interactable = false;
        await SetStageRanking();
        stageToggle.interactable = true;
        combatPowerToggle.interactable = true;
        dungeonDamageToggle.interactable = true;
    }
    public async void OnClickCombatPowerRankingButton()
    {
        stageToggle.interactable = false;
        combatPowerToggle.interactable = false;
        dungeonDamageToggle.interactable = false;
        await SetCombatPowerRanking();
        stageToggle.interactable = true;
        combatPowerToggle.interactable = true;
        dungeonDamageToggle.interactable = true;
    }
    public async void OnClickDungeonDamageRankingButton()
    {
        stageToggle.interactable = false;
        combatPowerToggle.interactable = false;
        dungeonDamageToggle.interactable = false;
        await SetDungeonDamageRanking();
        stageToggle.interactable = true;
        combatPowerToggle.interactable = true;
        dungeonDamageToggle.interactable = true;
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
