
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[Serializable]
public class InventoryElementSaveData
{
    public int soldierId; //250331 HKY 데이터형 변경
    public bool isLocked;
    public Grade grade;
    public int count;
    public int level;
}
[Serializable]
public class InventorySaveData
{
    public List<InventoryElementSaveData> elements = new List<InventoryElementSaveData>();
    public int equipElementID; //250331 HKY 데이터형 변경
    public UnitTypes inventoryType;
}
[Serializable]
public class StageSaveData
{
    public int currentPlanet;
    public int currentStage;
    public int highPlanet;
    public int highStage;
}
