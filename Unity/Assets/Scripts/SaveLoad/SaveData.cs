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
    public Dictionary<UnitTypes, SoldierInventoryData> SoldierInventorySaveData = new Dictionary<UnitTypes, SoldierInventoryData>();
    public StageSaveData stageSaveData;
    public Dictionary<int, BigNumber> itemSaveData = new Dictionary<int, BigNumber>();
    public SaveDataV1()
    {
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return new SaveDataV2(this);
    }
}
public class SaveDataV2 : SaveDataV1
{
    public MiningRobotInventoryData miningRobotInventorySaveData;
    public SaveDataV2()
    {
        Version = 2;
        miningRobotInventorySaveData = new MiningRobotInventoryData(60);
    }
    public SaveDataV2(SaveDataV1 oldData)
    {
        this.SoldierInventorySaveData = oldData.SoldierInventorySaveData;
        this.stageSaveData = oldData.stageSaveData;
        this.itemSaveData = oldData.itemSaveData;

        miningRobotInventorySaveData = new MiningRobotInventoryData(60);
        Version = 2;
    }
    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}
