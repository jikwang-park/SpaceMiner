using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitPartyManager : MonoBehaviour
{
    public List<Unit> unitprefabs = new List<Unit>();

    private Unit[] units;

    public int AliveCount
    {
        get
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
    }


    private void Awake()
    {
        UnitSpwan();
    }
  

    public void UnitSpwan()
    {
        if(unitprefabs.Count <= 0)
        {
            Debug.Log("유닛 프리팹 존재하지않음");
            return;
        }

        if(unitprefabs.Count >= 3)
        {
            Debug.Log("유닛이 3마리 이상입니다");
            return;
        }


        for(int i =0; i< unitprefabs.Count; ++i)
        {
            units[i] = Instantiate(unitprefabs[i]); // 나중에 비동기로드로 바꿈
            unitprefabs[i].transform.position += new Vector3(20, 20, 0);
        }

    }

    private void Update()
    {
        
    }

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

    public Transform GetFirstLineUnit()
    {
        if(units.Length == 0)
        {
            Debug.Log("Unit is Empty");
            return null;
        }

        for(int i =0; i< units.Length; i++)
        {
            if (units[i].IsDead)
            {
                continue;
            }

            return units[i].transform;
        }
        

        return null;
    }
}
