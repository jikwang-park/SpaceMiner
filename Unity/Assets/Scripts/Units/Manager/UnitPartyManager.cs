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
            Debug.Log("���� ������ ������������");
            return;
        }

        if(unitprefabs.Count >= 3)
        {
            Debug.Log("������ 3���� �̻��Դϴ�");
            return;
        }


        for(int i =0; i< unitprefabs.Count; ++i)
        {
            Instantiate(unitprefabs[i]); // ���߿� �񵿱�ε�� �ٲ�
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
