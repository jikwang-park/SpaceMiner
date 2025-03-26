using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

public class UnitPartyManager : MonoBehaviour
{
    public List<Unit> unitprefabs = new List<Unit>();

    public List<Unit> generateInstance = new List<Unit>();
    public Dictionary<UnitTypes, Unit> units = new Dictionary<UnitTypes, Unit>();


    public int AliveCount
    { 
        get
        {
            return generateInstance.Where(x => !x.IsDead).Count();
        }     
    }
   


    private void Awake()
    {
        UnitSpwan();
    }
    private void Update()
    {
        
    }

    public void UnitSpwan()
    {           
        if (unitprefabs.Count <= 0)
        {
            Debug.Log("유닛 프리팹 존재하지않음");
            return;
        }
        if(unitprefabs.Count >= 3)
        {
            Debug.Log("유닛이 3마리 이상입니다");
            return;
        }
        for (int i =0; i< unitprefabs.Count; ++i)
        {
            var go = Instantiate(unitprefabs[i], Vector3.zero ,Quaternion.identity);
            // 나중에 비동기로드로 바꿈
            go.transform.position += new Vector3(0, 0, -5 * i);
            generateInstance.Add(go);
        }
        SetInitData();
    }
     

    private void SetInitData()
    {
        var data = DataTableManager.SoldierTable.GetTypeDictionary();
        for(int i = 0; i < generateInstance.Count; ++i)
        {
            generateInstance[i].SetData(data[(UnitTypes)i + 1][0], (UnitTypes)i + 1);
            
            
        }
    }


    //private void SetInitData(UnitTypes type)
    //{
    //    var data = DataTableManager.SoldierTable.GetTypeDictionary();
    //    var UnitData = data[type][0];
    //    unitDic[type].SetData(UnitData, type);
    //}


    public int GetAliveUnitCount()
    {
        foreach (var unit in unitprefabs)
        {
            if (!unit.IsDead)
            {
                unit.aliveCount++;
            }
            return unit.aliveCount;
        }
        return 0;
    }


    public Unit GetFirstLineUnitGo()
    {
        if (generateInstance.Count == 0)
        {
            Debug.Log("Unit is Empty");
            return null;
        }

        for (int i = 0; i < generateInstance.Count; i++)
        {
            if (generateInstance[i].IsDead)
            {
                continue;
            }
            return generateInstance[i];
        }

        return null;
    }

    public Transform GetFirstLineUnitTransform()
    {
  

        if(generateInstance.Count == 0)
        {
            Debug.Log("Unit is Empty");
            return null;
        }

        for(int i =0; i< generateInstance.Count; i++)
        {
            if (generateInstance[i].IsDead)
            {
                continue;
            }
            return generateInstance[i].transform;
        }
        

        return null;
    }
    public void SetUnitData(SoldierTable.Data data, UnitTypes type)
    {
    }

    public Transform GetUnit(UnitTypes type)
    {
        for(int i= 0; i< generateInstance.Count; i++)
        {
            if(type == generateInstance[i].UnitTypes)
                return generateInstance[i].transform;
        }
        return null;
    }


    public Unit GetCurrentTargetType(string targetString)
    {
        switch (targetString)
        {
            case "TankID":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Tanker);

            case "DealID":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Dealer);

            case "HealID":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Healer);

            default:
                return null;
        }
    }
}
