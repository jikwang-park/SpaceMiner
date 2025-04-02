using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;
using static UnitUpgradeTable;

public class UnitPartyManager : MonoBehaviour
{
    public List<Unit> unitprefabs = new List<Unit>();

    public List<Unit> generateInstance = new List<Unit>();

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

    public void AddStats(UpgradeType type , float amount)
    {
        for(int i =0; i< generateInstance.Count; ++i)
        {
            generateInstance[i].unitStats.AddStats(type, amount);
        }
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
            Debug.LogError("Unit is Empty");
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

  
    
    //public List<Unit> ChangeUnit()
    //{
    //    UnitTypes type;
    //    for(int i = 0; i< generateInstance.Count; i++)
    //    {
    //        type = generateInstance[i].UnitTypes;
    //    }
    //    switch (type)
    //    {
    //        case UnitTypes.Tanker:
    //            break;
    //        case UnitTypes.Dealer:
    //            break;
    //        case UnitTypes.Healer:
    //            break;
    //    }
    //}

    public Unit GetCurrentTargetType(string targetString)
    {
        //TODO: 유닛 타입에 따라 반환 변경하도록 바꿔야함 250331 HKY
        switch (targetString)
        {
            case "1":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Tanker);

            case "2":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Dealer);

            case "3":
                return generateInstance.Find((x) => x.UnitTypes == UnitTypes.Healer);

            default:
                return null;
        }
    }
}
