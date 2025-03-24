using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitPartyManager : MonoBehaviour
{
    public List<Unit> unitprefabs = new List<Unit>();

    public List<Unit> generateInstance = new List<Unit>();

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
            Debug.Log("���� ������ ������������");
            return;
        }

        if(unitprefabs.Count >= 3)
        {
            Debug.Log("������ 3���� �̻��Դϴ�");
            return;
        }

        
        for (int i =0; i< unitprefabs.Count; ++i)
        {
            var go = Instantiate(unitprefabs[i], Vector3.zero ,Quaternion.identity); // ���߿� �񵿱�ε�� �ٲ�
            go.transform.position += new Vector3(0, 0, -5 * i);
            generateInstance.Add(go);
        }
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
}
