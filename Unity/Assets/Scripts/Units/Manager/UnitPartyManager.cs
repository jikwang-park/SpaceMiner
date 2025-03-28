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

    public bool IsUnitAllDead
    {
        get
        {
            if(AliveCount ==  3)
                return true;
            
            return false;
            
        }
    }

    public int AliveCount
    { 
        get
        {
            return generateInstance.Where(x => !x.IsDead).Count();
        }     
    }


    // HKY 250328 Awake에서 스폰시 WaitForCompletion 발생하여 Start로 변경
    private void Start()
    {
        UnitSpwan();
    }
    private void Update()
    {
        
    }

    //public Unit SearchNextUnit()
    //{
    //    for(int i = 0; i< generateInstance.Count; i++)
    //    {
    //        var firstUnit = generateInstance[0];
    //        if (generateInstance[i].IsDead)
    //        {
                
    //        }
    //    }
    //}

    public void UnitSpwan()
    {           
        if (unitprefabs.Count <= 0)
        {
            Debug.Log("유닛 프리팹 존재하지않음");
            return;
        }
        if(unitprefabs.Count >= 4)
        {
            Debug.Log("넘쳐서 소환이 안됩니다");
            return;
        }
        for (int i =0; i< unitprefabs.Count; ++i)
        {
            var go = Instantiate(unitprefabs[i], Vector3.zero ,Quaternion.identity);
            go.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnUnitDie;
            // 나중에 비동기로드로 바꿈
            go.transform.position += new Vector3(0, 0, -5 * i);
            generateInstance.Add(go);
        }
        SetInitData();
    }


     

    private void OnUnitDie(DestructedDestroyEvent e)
    {
        var gameobejct = e.GetComponent<Unit>();
        generateInstance.Remove(gameobejct);
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
                unit.unitAliveCount++;
            }
            return unit.unitAliveCount;
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

    public Unit GetCurrentUnitGo(UnitTypes type)
    {
        for(int i = 0; i< generateInstance.Count; i++)
        {
            if (type == generateInstance[i].UnitTypes)
                return generateInstance[i];
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
