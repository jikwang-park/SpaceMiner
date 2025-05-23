using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    [field: SerializeField]
    public LocalizationText stageText { get; private set; }


    [field: SerializeField]
    public GameObject waveGameObject { get; private set; }

    [field: SerializeField]
    public LocalizationText waveText { get; private set; }

    [field: SerializeField]
    public LocalizationText timerText { get; private set; }


    [field: SerializeField]
    public GameObject miningBattleTimerGameObject { get; private set; }

    [field: SerializeField]
    public LocalizationText miningBattleTimerText { get; private set; }

    [field: SerializeField]
    public Slider miningBattleCenterHpBar { get; private set; }

    [field: SerializeField]
    public GameObject bossDamageGameObject { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI bossDamageText { get; private set; }

    [field: SerializeField]
    public GameObject unitHpBars { get; private set; }

    [field: SerializeField]
    public GameObject unitSkills { get; private set; }

    [field: SerializeField]
    public Dungeon1EndWindow DungeonEndWindow { get; private set; }
    [field: SerializeField]
    public DungeonExitConfirmWindow DungeonExitConfirmWindow { get; private set; }
    [field: SerializeField]
    public Dungeon2EndWindow DamageDungeonEndWindow { get; private set; }
    [field: SerializeField]
    public StageSelectWindow StageSelectWindow { get; private set; }
    [field: SerializeField]
    public GuideQuestRewardWindow GuideQuestRewardWindow { get; private set; }

    [field: SerializeField]
    public Toggle RushSelectToggle { get; private set; }

    [field: SerializeField]
    public Button mineBattleButton { get; private set; }

    [field: SerializeField]
    public MiningBattleResultWindow miningBattleResultWindow { get; private set; }

    [SerializeField]
    private SerializedDictionary<IngameStatus, List<GameObject>> statusObjectLists;

    private IngameStatus ingameStatus;

    public void SetTimer(float remainTime)
    {
        timerText.SetString(Defines.DirectStringID, remainTime.ToString("F2"));
    }

    public void SetStageText(int planet, int stage)
    {
        stageText.SetString(Defines.PlanetStageFormatStringID, planet.ToString(), stage.ToString());
    }

    public void SetWaveText(int wave)
    {
        waveText.SetString(Defines.WaveTextStringID, wave.ToString());
    }

    public void SetDungeonStageText(int stage)
    {
        stageText.SetString(Defines.Dungeon1StringID, stage.ToString());
    }


    public void SetStatus(IngameStatus status)
    {
        if (ingameStatus == status)
        {
            return;
        }

        if (statusObjectLists.ContainsKey(ingameStatus))
        {
            foreach (var gameobject in statusObjectLists[ingameStatus])
            {
                gameobject.SetActive(false);
            }
        }

        if (statusObjectLists.ContainsKey(status))
        {
            foreach (var gameobject in statusObjectLists[status])
            {
                gameobject.SetActive(true);
            }
        }
        ingameStatus = status;
    }
}
