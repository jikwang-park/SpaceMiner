using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    [JsonProperty("inventorySaveData")]
    public Dictionary<UnitTypes, SoldierInventoryData> soldierInventorySaveData = new Dictionary<UnitTypes, SoldierInventoryData>();
    public StageSaveData stageSaveData;
    public Dictionary<int, BigNumber> itemSaveData = new Dictionary<int, BigNumber>();
    public SaveDataV1()
    {
        Version = 1;
    }
    public SaveDataV1(SaveDataV1 oldData)
    {
        this.soldierInventorySaveData = oldData.soldierInventorySaveData;
        this.stageSaveData = oldData.stageSaveData;
        this.itemSaveData = oldData.itemSaveData;
        Version = oldData.Version;
    }
    public override SaveData VersionUp()
    {
        return new SaveDataV2(this);
    }
}
public class SaveDataV2 : SaveDataV1
{
    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public MiningRobotInventoryData miningRobotInventorySaveData;
    public SaveDataV2() : base()
    {
        Version = 2;
        miningRobotInventorySaveData = MiningRobotInventoryData.CreateDefault();
    }

    public SaveDataV2(SaveDataV1 oldData) : base(oldData)
    {
        miningRobotInventorySaveData = MiningRobotInventoryData.CreateDefault();
        Version = 2;
    }
    public override SaveData VersionUp()
    {
        return new SaveDataV3(this);
    }
}
public class SaveDataV3 : SaveDataV2
{
    public UnitStatUpgradeData unitStatUpgradeData;
    public UnitSkillUpgradeData unitSkillUpgradeData;
    public QuestProgressData questProgressData;
    public BuildingData buildingData;
    public DungeonKeyShopData dungeonKeyShopData;
    public DateTime quitTime;
    public SaveDataV3() : base()
    {
        unitStatUpgradeData = UnitStatUpgradeData.CreateDefault();
        unitSkillUpgradeData = UnitSkillUpgradeData.CreateDefault();
        questProgressData = QuestProgressData.CreateDefault();
        buildingData = BuildingData.CreateDefault();
        dungeonKeyShopData = DungeonKeyShopData.CreateDefault();
        quitTime = DateTime.Now;
        Version = 3;
    }
    public SaveDataV3(SaveDataV2 oldData) : base(oldData)
    {
        unitStatUpgradeData = UnitStatUpgradeData.CreateDefault();
        unitSkillUpgradeData = UnitSkillUpgradeData.CreateDefault();
        questProgressData = QuestProgressData.CreateDefault();
        buildingData = BuildingData.CreateDefault();
        dungeonKeyShopData = DungeonKeyShopData.CreateDefault();
        quitTime = DateTime.Now;
        Version = 3;
    }
    public override SaveData VersionUp()
    {
        return new SaveDataV4(this);
    }
}
public class SaveDataV4 : SaveDataV3
{
    public Dictionary<int, AttendanceData> attendanceStates;
    public MineBattleData mineBattleData;
    public Dictionary<EffectItemTable.ItemType, int> possessionEffectItemDatas;
    public Dictionary<TutorialTable.QuestTypes, bool> TutorialOpened;
    public Dictionary<TutorialTable.QuestTypes, bool> TutorialRewardGot;
    public Dictionary<int, bool> contentsOpened;
    public float healerSkillSliderValue;
    public StageMode stageMode;
    public SaveDataV4() : base()
    {
        attendanceStates = new Dictionary<int, AttendanceData>();

        foreach (var entry in DataTableManager.AttendanceTable.GetList())
        {
            attendanceStates[entry.ID] = AttendanceData.CreateDefault(entry.ID);
        }
        TutorialOpened = new Dictionary<TutorialTable.QuestTypes, bool>();
        TutorialRewardGot = new Dictionary<TutorialTable.QuestTypes, bool>();
        foreach(var type in Enum.GetValues(typeof(TutorialTable.QuestTypes)))
        {
            TutorialOpened.Add((TutorialTable.QuestTypes)type, false);
            TutorialRewardGot.Add((TutorialTable.QuestTypes)type, false);
        }

        mineBattleData = MineBattleData.CreateDefault();

        possessionEffectItemDatas = new Dictionary<EffectItemTable.ItemType, int>();
        foreach (var type in Enum.GetValues(typeof(EffectItemTable.ItemType)))
        {
            possessionEffectItemDatas.Add((EffectItemTable.ItemType)type, 0);
        }

        contentsOpened = new Dictionary<int, bool>();
        foreach(var id in DataTableManager.ContentsOpenTable.GetIds())
        {
            contentsOpened.Add(id, false);
        }
        healerSkillSliderValue = 0.5f;
        stageMode = StageMode.Ascend;
        Version = 4;
    }
    public SaveDataV4(SaveDataV3 oldData) : base(oldData)
    {
        attendanceStates = new Dictionary<int, AttendanceData>();
        foreach (var entry in DataTableManager.AttendanceTable.GetList())
        {
            attendanceStates[entry.ID] = AttendanceData.CreateDefault(entry.ID);
        }
        TutorialOpened = new Dictionary<TutorialTable.QuestTypes, bool>();
        TutorialRewardGot = new Dictionary<TutorialTable.QuestTypes, bool>();
        foreach (var type in Enum.GetValues(typeof(TutorialTable.QuestTypes)))
        {
            TutorialOpened.Add((TutorialTable.QuestTypes)type, false);
            TutorialRewardGot.Add((TutorialTable.QuestTypes)type, false);
        }
        mineBattleData = MineBattleData.CreateDefault();

        possessionEffectItemDatas = new Dictionary<EffectItemTable.ItemType, int>();
        foreach (var type in Enum.GetValues(typeof(EffectItemTable.ItemType)))
        {
            possessionEffectItemDatas.Add((EffectItemTable.ItemType)type, 0);
        }

        contentsOpened = new Dictionary<int, bool>();
        foreach (var id in DataTableManager.ContentsOpenTable.GetIds())
        {
            contentsOpened.Add(id, false);
        }
        healerSkillSliderValue = 0.5f;
        stageMode = StageMode.Ascend;
        Version = 4;
    }
    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}