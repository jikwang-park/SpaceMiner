using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitPartyManager : MonoBehaviour
{
    [SerializeField]
    private Unit tankerPrefab;
    [SerializeField]
    private Unit dealerPrefab;
    [SerializeField]
    private Unit healerPrefab;

    private Dictionary<UnitTypes, Unit> prefabs = new Dictionary<UnitTypes, Unit>();
    private Dictionary<UnitTypes, Unit> party = new Dictionary<UnitTypes, Unit>();

    public event System.Action OnUnitAllDead;

    [SerializeField]
    private Vector3 unitOffset = Vector3.back * 5f;

    public int UnitCount => party.Count;

    private void Awake()
    {
        if (tankerPrefab is not null)
        {
            prefabs.Add(UnitTypes.Tanker, tankerPrefab);
        }
        if (dealerPrefab is not null)
        {
            prefabs.Add(UnitTypes.Dealer, dealerPrefab);
        }
        if (healerPrefab is not null)
        {
            prefabs.Add(UnitTypes.Healer, healerPrefab);
        }
    }

    public Dictionary<UnitTypes,Unit> GetCurrentParty()
    {
        return party;
    }
    public void ResetSkillCoolTime()
    {
        foreach(var unit in party.Values)
        {
           unit.lastSkillUsedTime = Time.time;
        }
    }

    public void ResetUnitHealth()
    {
        foreach ( var unit in party.Values)
        {
            unit.unitStats.Hp = unit.unitStats.maxHp;
        }
    }

    public void ResetBehaviorTree()
    {
        foreach ( var unit in party.Values)
        {
            unit.behaviorTree.Reset();
        }
    }
    public void UnitSpawn()
    {
        UnitSpawn(Vector3.zero);
    }

    public void UnitSpawn(Vector3 startPos)
    {
        UnitDespawn();
        ResetUnits(startPos);
    }

    public void SetUnitData(SoldierTable.Data data, UnitTypes type)
    {
        if (party.ContainsKey(type))
        {
            party[type].SetData(data, type);
        }
    }

    private void OnUnitDie(DestructedDestroyEvent sender)
    {
        var unit = sender.GetComponent<Unit>();
        party.Remove(unit.UnitTypes);

        if (party.Count == 0)
        {
            OnUnitAllDead?.Invoke();
        }
    }

    public void AddStats(UnitUpgradeTable.UpgradeType type, float amount)
    {
        foreach (var unit in party)
        {
            unit.Value.unitStats.AddStats(type, amount);
        }
    }

    public void UnitDespawn()
    {
        foreach (var unit in party)
        {
            Destroy(unit.Value.gameObject);
        }
        party.Clear();
    }

    public Transform GetFirstLineUnitTransform()
    {
        if (party.Count == 0)
        {
            Debug.LogError("Unit is Empty");
            return null;
        }

        for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
        {
            if (party.ContainsKey((UnitTypes)i))
            {
                return party[(UnitTypes)i].transform;
            }
        }

        return null;
    }

    public Transform GetUnit(UnitTypes type)
    {
        if (party.ContainsKey(type))
        {
            return party[type].transform;
        }
        return null;
    }

    public Unit GetCurrentTargetType(string targetString)
    {
        int target = int.Parse(targetString);
        if (party.ContainsKey((UnitTypes)target))
        {
            return party[(UnitTypes)target];
        }
        return null;
    }

    // 250403 HKY ���� ���� Ÿ���� ������ ���� ���� ������ ��ȯ���ִ� �޼ҵ� �߰�
    public bool IsUnitExistFront(UnitTypes myType)
    {
        for (int i = (int)myType - 1; i >= (int)UnitTypes.Tanker; --i)
        {
            if (party.ContainsKey((UnitTypes)i))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsUnitExistBack(UnitTypes myType)
    {
        for (int i = (int)myType + 1; i <= (int)UnitTypes.Healer; ++i)
        {
            if (party.ContainsKey((UnitTypes)i))
            {
                return true;
            }
        }
        return false;
    }

    // 250403 HKY ���� ���� Ÿ���� ������ �� ���� ������ ��ȯ���ִ� �޼ҵ� �߰�
    public Unit GetFrontUnit(UnitTypes myType)
    {
        for (int i = (int)myType - 1; i >= (int)UnitTypes.Tanker; --i)
        {
            if (party.ContainsKey((UnitTypes)i))
            {
                return party[(UnitTypes)i];
            }
        }
        return null;
    }

    public Unit GetBackUnit(UnitTypes myType)
    {
        for (int i = (int)myType + 1; i <= (int)UnitTypes.Healer; ++i)
        {
            if (party.ContainsKey((UnitTypes)i))
            {
                return party[(UnitTypes)i];
            }
        }
        return null;
    }

    public void ResetUnits(Vector3 startPos)
    {
        Vector3 position = startPos;

        var typeData = DataTableManager.SoldierTable.GetTypeDictionary();

        for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
        {
            if (!prefabs.ContainsKey((UnitTypes)i))
            {
                position += unitOffset;
                continue;
            }

            var go = Instantiate(prefabs[(UnitTypes)i], position, Quaternion.identity);
            go.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnUnitDie;
            position += unitOffset;
            // ���߿� �񵿱�ε�� �ٲ�
            go.SetData(typeData[(UnitTypes)i][0], (UnitTypes)i);
            party.Add(go.UnitTypes, go);
        }
    }
}
