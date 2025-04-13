using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MiningRobotInventoryManager
{
    public static event Action<int, MiningRobotInventorySlotData> onChangedInventory;
    public static event Action<int> onEquipRobot;
    public static event Action<int, MergeResponseCallback> onRequestMerge;
    public static int currentPlanetId;

    public static readonly int ScaleFactor = 1000;
    public static MiningRobotInventoryData Inventory
    {
        get
        {
            return SaveLoadManager.Data.miningRobotInventorySaveData;
        }
    }
    public delegate void MergeResponseCallback(bool confirmed);
    public static void AddRobot(int robotId)
    {
        if(DataTableManager.RobotTable.GetData(robotId) == null)
        {
            return;
        }

        for(int i = 0; i < Inventory.slots.Count; i++)
        {
            if (Inventory.slots[i].isEmpty)
            {
                Inventory.slots[i].isEmpty = false;
                Inventory.slots[i].miningRobotId = robotId;
                Grade grade = DataTableManager.RobotTable.GetData(Inventory.slots[i].miningRobotId).grade;
                Inventory.slots[i].grade = grade;
                onChangedInventory?.Invoke(i, Inventory.slots[i]);
                return;
            }
        }
    }
    public static void ProcessSlots(int indexA, int indexB)
    {
        if(indexA < 0 || indexA >= Inventory.slots.Count || indexB < 0 || indexB >= Inventory.slots.Count)
        {
            return;
        }

        if(indexA == indexB)
        {
            return;
        }

        MiningRobotInventorySlotData slotA = Inventory.slots[indexA];
        MiningRobotInventorySlotData slotB = Inventory.slots[indexB];

        if (!slotA.isEmpty && !slotB.isEmpty)
        {
            if(slotA.miningRobotId == slotB.miningRobotId)
            {
                MergeSlots(indexA, indexB);
            }
            else
            {
                SwapSlots(indexA, indexB);
            }
        }
        else if(!slotA.isEmpty && slotB.isEmpty)
        {
            MoveSlots(indexA, indexB);
        }
    }
    private static void MergeSlots(int indexA, int indexB)
    {
        int currentMergedRobotId = Inventory.slots[indexA].miningRobotId;
        RobotMergeTable.Data data = DataTableManager.RobotMergeTable.GetData(currentMergedRobotId);
        int canMerge = DataTableManager.RobotTable.GetData(currentMergedRobotId).IsMergeable;
        if(data == null || canMerge == 0)
        {
            return;
        }

        onRequestMerge.Invoke(currentMergedRobotId, (bool confirmed) =>
        {
            if (confirmed)
            {
                int randomValue = Random.Range(0, 101);
                if (randomValue < data.probability)
                {
                    Inventory.slots[indexB].miningRobotId = data.resultID;
                    Grade grade = DataTableManager.RobotTable.GetData(Inventory.slots[indexB].miningRobotId).grade;
                    Inventory.slots[indexB].grade = grade;
                }
                else
                {
                    Inventory.slots[indexB].isEmpty = true;
                    Inventory.slots[indexB].grade = Grade.None;
                    Inventory.slots[indexB].miningRobotId = 0;
                }
                Inventory.slots[indexA].isEmpty = true;
                Inventory.slots[indexA].grade = Grade.None;
                Inventory.slots[indexA].miningRobotId = 0;

                onChangedInventory?.Invoke(indexA, Inventory.slots[indexA]);
                onChangedInventory?.Invoke(indexB, Inventory.slots[indexB]);
            }
            else
            {
                Debug.Log("합성이 취소되었습니다.");
            }
        });
    }
    private static void SwapSlots(int indexA, int indexB)
    {
        MiningRobotInventorySlotData temp = Inventory.slots[indexA];
        Inventory.slots[indexA] = Inventory.slots[indexB];
        Inventory.slots[indexB] = temp;

        onChangedInventory?.Invoke(indexA, Inventory.slots[indexA]);
        onChangedInventory?.Invoke(indexB, Inventory.slots[indexB]);
    }
    private static void MoveSlots(int indexA, int indexB)
    {
        Inventory.slots[indexB].isEmpty = false;
        Inventory.slots[indexB].miningRobotId = Inventory.slots[indexA].miningRobotId;
        Grade grade = DataTableManager.RobotTable.GetData(Inventory.slots[indexB].miningRobotId).grade;
        Inventory.slots[indexB].grade = grade;
        onChangedInventory?.Invoke(indexB, Inventory.slots[indexB]);

        Inventory.slots[indexA].isEmpty = true;
        Inventory.slots[indexA].grade = Grade.None;
        Inventory.slots[indexA].miningRobotId = 0;
        onChangedInventory?.Invoke(indexA, Inventory.slots[indexA]);
    }
    public static void SwapInventoryAndEquipmentSlot(int slotIndex, int equipIndex)
    {
        if(slotIndex < 0 || slotIndex >= Inventory.slots.Count || equipIndex < 0 || equipIndex >= Inventory.equipmentSlotsToPlanet[currentPlanetId].Length)
        {
            return;
        }

        MiningRobotInventorySlotData tmp = Inventory.equipmentSlotsToPlanet[currentPlanetId][equipIndex];
        Inventory.equipmentSlotsToPlanet[currentPlanetId][equipIndex] = Inventory.slots[slotIndex];
        Inventory.slots[slotIndex] = tmp;

        Inventory.slots[slotIndex].slotType = SlotType.Inventory;
        Inventory.equipmentSlotsToPlanet[currentPlanetId][equipIndex].slotType = SlotType.Equip;

        onEquipRobot?.Invoke(currentPlanetId);
        onChangedInventory?.Invoke(slotIndex, Inventory.slots[slotIndex]);
    }
    public static void SortInventorySlots()
    {
        Inventory.slots.Sort((a, b) =>
        {
            if (a.isEmpty && !b.isEmpty)
            { 
                return 1; 
            }
            if (!a.isEmpty && b.isEmpty)
            {
                return -1;
            }

            if (a.isEmpty && b.isEmpty)
            {
                return 0;
            }

            return ((int)b.grade).CompareTo((int)a.grade);
        });

        for (int i = 0; i < Inventory.slots.Count; i++)
        {
            onChangedInventory?.Invoke(i, Inventory.slots[i]);
        }
    }
    public static Dictionary<int, bool> CheckPlanetsOpen()
    {
        Dictionary<int, bool> results = new Dictionary<int, bool>();
        List<int> planetIds = DataTableManager.PlanetTable.GetIds();
        for (int i = 0; i < planetIds.Count; i++)
        {
            var planetData = DataTableManager.PlanetTable.GetData(planetIds[i]);
            var stageData = DataTableManager.StageTable.GetData(planetData.stageToOpen);


            bool cleared = (SaveLoadManager.Data.stageSaveData.clearedPlanet == stageData.Planet && SaveLoadManager.Data.stageSaveData.clearedStage >= stageData.Stage)
                    || (SaveLoadManager.Data.stageSaveData.clearedPlanet > stageData.Planet);

            results.Add(planetIds[i], cleared);
        }

        return results;
    }
    public static Dictionary<int, BigNumber> GetIdleRewardOpenPlanet()
    {
        Dictionary<int, BigNumber> result = new Dictionary<int, BigNumber>();

        var checkPlanets = CheckPlanetsOpen();

        foreach(var planet in checkPlanets) 
        {
            if(!planet.Value)
            {
                continue;
            }
            int itemId = DataTableManager.PlanetTable.GetData(planet.Key).ItemID;
            BigNumber amountPerMinute = CalculateMiningAmountPerMinute(planet.Key);

            result.Add(itemId, amountPerMinute );
        }

        return result;
    }
    public static BigNumber CalculateMiningAmountPerSecond(int planetId)
    {
        BigNumber amountPerSecond = 0;
        if (Inventory.equipmentSlotsToPlanet.ContainsKey(planetId))
        {
            MiningRobotInventorySlotData[] robots = Inventory.equipmentSlotsToPlanet[planetId];

            for (int i = 0; i < robots.Length; i++)
            {
                if (!robots[i].isEmpty)
                {
                    amountPerSecond += CalculateRobotMiningAmountPerSecond(planetId, robots[i].miningRobotId, i);
                }
            }
        }
        return amountPerSecond;
    }
    public static BigNumber CalculateMiningAmountPerMinute(int planetId)
    {
        return CalculateMiningAmountPerSecond(planetId) * 60;
    }
    public static BigNumber CalculateRobotMiningAmountPerSecond(int planetId, int robotId, int index)
    {
        float robotMiningAmount;
        var robotData = DataTableManager.RobotTable.GetData(robotId);
        var planetData = DataTableManager.PlanetTable.GetData(planetId);

        float planetLevel = (index == 1) ? planetData.miningLevel2 : planetData.miningLevel;

        robotMiningAmount = (float)robotData.loadCapacity / (float)((planetLevel / robotData.moveSpeed) + (planetLevel / robotData.miningSpeed));

        return (BigNumber)robotMiningAmount * ScaleFactor;
    }
}
