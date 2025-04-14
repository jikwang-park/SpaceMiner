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
                gachaCostDict[gacha.Key] = gacha.Value.NeedItemCount1;
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
            int ticketId = DataTableManager.GachaTable.GetData(gachaId).NeedItemID2;
            if(!ItemManager.CanConsume(ticketId, count))
            {
                Debug.Log($"{DataTableManager.ItemTable.GetData(ticketId).NameStringID}가 부족합니다");
                return null;
            }
            ItemManager.ConsumeItem(ticketId, count);
        }
        else
        {
            var gachaData = DataTableManager.GachaTable.GetData(gachaId);
            BigNumber requiredCost = CalCulateCost(gachaId, count);
            if (!ItemManager.CanConsume(gachaData.NeedItemID1, requiredCost))
            {
                Debug.Log($"{DataTableManager.ItemTable.GetData(gachaData.NeedItemID1).NameStringID}가 부족합니다");
                return null;
            }
            ItemManager.ConsumeItem(gachaData.NeedItemID1, requiredCost);
            // 250412 HKY 가챠 증가율 삭제 적용
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

        gachaResults = gachaResults.OrderByDescending((e) => e.Grade).ToList();

        return gachaResults;
    }

    private static Grade GetRandomGachaGrade(int gachaId)
    {
        List<GachaGradeTable.Data> gachaGradeDatas = DataTableManager.GachaGradeTable.GetLevelData(gachaId);

        var randomProbability = Random.Range(0f, 1f);
        foreach (var gachaGradeData in gachaGradeDatas)
        {
            if (gachaGradeData.Probability != 0f && randomProbability - gachaGradeData.Probability <= 0f)
            {
                return gachaGradeData.Grade;
            }
            randomProbability -= gachaGradeData.Probability;
        }

        return default;
    }
    private static SoldierTable.Data GetRandomSoldierData(Grade grade)
    {
        List<GachaSoldierTable.Data> soldierDatas = DataTableManager.GachaSoldierTable.GetGradeDatas(grade);

        var randomProbability = Random.Range(0f, 1f);
        foreach(var soldierData in soldierDatas)
        {
            if (randomProbability - soldierData.Probability <= 0f)
            {
                return DataTableManager.SoldierTable.GetData(soldierData.SoldierID);
            }
            randomProbability -= soldierData.Probability;
        }
        return default;
    }
}
