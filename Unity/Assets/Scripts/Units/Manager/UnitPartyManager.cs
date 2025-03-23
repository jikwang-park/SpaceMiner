using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitPartyManager : MonoBehaviour
{
    public List<Unit> unitprefabs = new List<Unit>();



    private void Awake()
    {
        UnitSpwan();
    }
    private void Start()
    {
    }
    private void Init()
    {
    }

    private void UnitSpwan()
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
            Instantiate(unitprefabs[i]); // 나중에 비동기로드로 바꿈
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
        if(unitprefabs.Count == 0)
        {
            Debug.Log("Unit is Empty");
            return null;
        }

        for(int i =0; i< unitprefabs.Count; i++)
        {
            if (unitprefabs[i].IsDead)
            {
                continue;
            }

            return unitprefabs[i].transform;
        }
        

        return null;
    }
}
