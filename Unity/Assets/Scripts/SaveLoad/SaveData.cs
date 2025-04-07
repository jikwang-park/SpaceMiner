using Newtonsoft.Json;
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
    public MiningRobotInventoryData miningRobotInventorySaveData;
    public SaveDataV2() : base()
    {
        miningRobotInventorySaveData = new MiningRobotInventoryData(60);
        Version = 2;
    }
    public SaveDataV2(SaveDataV1 oldData) : base(oldData)
    {
        miningRobotInventorySaveData = new MiningRobotInventoryData(60);
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
    public SaveDataV3() : base()
    {
        unitStatUpgradeData = new UnitStatUpgradeData();
        unitSkillUpgradeData = new UnitSkillUpgradeData();
        Version = 3;
    }
    public SaveDataV3(SaveDataV2 oldData) : base(oldData)
    {
        unitStatUpgradeData = new UnitStatUpgradeData();
        unitSkillUpgradeData = new UnitSkillUpgradeData();
        Version = 3;
    }
    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}
