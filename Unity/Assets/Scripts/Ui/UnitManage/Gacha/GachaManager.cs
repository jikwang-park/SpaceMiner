using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GachaManager
{
    private static Dictionary<int, BigNumber> gachaCostDict;
    static GachaManager()
    {
        gachaCostDict = new Dictionary<int, BigNumber>();
        var gachaDict = DataTableManager.GachaTable.GetDict();
        foreach(var gacha in gachaDict)
        {
            if(!gachaCostDict.ContainsKey(gacha.Key))
            {
                gachaCostDict[gacha.Key] = gacha.Value.cost;
            }
        }
    }

    public static BigNumber CalCulateCost(int gachaId, int count)
    {
        if(gachaCostDict.ContainsKey(gachaId))
        {
            var cost = gachaCostDict[gachaId] * count;
            return cost;
        }
        return null;
    }

    public static List<SoldierTable.Data> Gacha(int gachaId, int count, bool useTicket = false)
    {
        if(useTicket)
        {
            int ticketId = DataTableManager.GachaTable.GetData(gachaId).cost_Item2ID;
            if(!ItemManager.CanConsume(ticketId, count))
            {
                Debug.Log($"{DataTableManager.ItemTable.GetData(ticketId).ItemStringID}가 부족합니다");
                return null;
            }
            ItemManager.ConsumeItem(ticketId, count);
        }
        else
        {
            var gachaData = DataTableManager.GachaTable.GetData(gachaId);
            BigNumber requiredCost = CalCulateCost(gachaId, count);
            if (!ItemManager.CanConsume(gachaData.cost_ItemID, requiredCost))
            {
                Debug.Log($"{DataTableManager.ItemTable.GetData(gachaData.cost_ItemID).ItemStringID}가 부족합니다");
                return null;
            }
            ItemManager.ConsumeItem(gachaData.cost_ItemID, requiredCost);
        }

        List<SoldierTable.Data> gachaResults = new List<SoldierTable.Data>();
        List<GachaGradeTable.Data> gachaGradeDatas = DataTableManager.GachaGradeTable.GetLevelData(gachaId);

        for(int i = 0; i < count; i++)
        {
            Grade grade = GetRandomGachaGrade(gachaId);
            SoldierTable.Data gachaResult = GetRandomSoldierData(grade);
            if (gachaResult != default)
            {
                gachaResults.Add(gachaResult);
            }
        }

        gachaResults = gachaResults.OrderByDescending((e) => e.Rating).ToList();

        return gachaResults;
    }

    private static Grade GetRandomGachaGrade(int gachaId)
    {
        List<GachaGradeTable.Data> gachaGradeDatas = DataTableManager.GachaGradeTable.GetLevelData(gachaId);

        var randomProbability = Random.Range(0f, 1f);
        foreach (var gachaGradeData in gachaGradeDatas)
        {
            if (gachaGradeData.probability != 0f && randomProbability - gachaGradeData.probability <= 0f)
            {
                return gachaGradeData.grade;
            }
            randomProbability -= gachaGradeData.probability;
        }

        return default;
    }
    private static SoldierTable.Data GetRandomSoldierData(Grade grade)
    {
        List<GachaSoldierTable.Data> soldierDatas = DataTableManager.GachaSoldierTable.GetGradeDatas(grade);

        var randomProbability = Random.Range(0f, 1f);
        foreach(var soldierData in soldierDatas)
        {
            if (randomProbability - soldierData.probability <= 0f)
            {
                return DataTableManager.SoldierTable.GetData(soldierData.soldierID);
            }
            randomProbability -= soldierData.probability;
        }
        return default;
    }
}
