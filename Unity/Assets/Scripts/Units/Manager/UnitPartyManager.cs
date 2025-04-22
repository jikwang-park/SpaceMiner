using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UnitPartyManager : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<UnitTypes, Unit> characters;

    [SerializeField]
    private SerializedDictionary<UnitTypes, GameObject[]> weapons;

    [SerializeField]
    private List<Vector3> unitSpawnPos;

    private Dictionary<UnitTypes, Unit> party = new Dictionary<UnitTypes, Unit>();

    public event System.Action OnUnitCreated;
    public event System.Action OnUnitUpdated;

    public event System.Action OnUnitAllDead;

    [SerializeField]
    public UnitSkillButtonManager buttonManager;

    private Vector3 unitOffset = Vector3.left* 1f;

    public int UnitCount => party.Count;

    public void ResetSkillCoolTime()
    {
        foreach (var unit in party.Values)
        {
            unit.lastSkillUsedTime = -unit.unitSkill.coolTime;
        }
    }

    public void ResetUnitHealth()
    {
        foreach (var unit in party.Values)
        {
            unit.unitStats.Hp = unit.unitStats.maxHp;
        }
    }

    public void ResetBehaviorTree()
    {
        foreach (var unit in party.Values)
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
            UnitCombatPowerCalculator.CalculateTotalCombatPower();
        }
    }

    private void OnUnitDie(DestructedDestroyEvent sender)
    {
        var unit = sender.GetComponent<Unit>();
        party.Remove(unit.UnitTypes);
        unit.gameObject.SetActive(false);
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
        UnitCombatPowerCalculator.CalculateTotalCombatPower();
    }

    public void UpgradeSkillStats(int id, UnitTypes type)
    {
        if (!party.ContainsKey(type))
            return;

        var unit = party[type];


        unit.unitSkill.UpgradeUnitSkillStats(id);
    }

    public void AddBuildingStats(BuildingTable.BuildingType type, float amount)
    {
        foreach (var unit in party)
        {
            unit.Value.unitStats.AddBuildingStats(type, amount);
        }
        UnitCombatPowerCalculator.CalculateTotalCombatPower();
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

        Transform frontMostZPos = null;
        float maxZ = float.MinValue;

        foreach ( var unit in party.Values)
        {
            if (unit == null)
                continue;

            float currentUnitZpos = unit.transform.position.z;
            if(currentUnitZpos > maxZ)
            {
                maxZ = currentUnitZpos;
                frontMostZPos = unit.transform;
            }
        }

       return frontMostZPos;
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



        for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
        {
            var currentType = (UnitTypes)i;

            var unit = Instantiate(characters[currentType], position, Quaternion.identity);
            unit.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnUnitDie;
            position += unitOffset;
            var currentSoilderId = InventoryManager.GetInventoryData(currentType).equipElementID;
            var currentSoilderData = DataTableManager.SoldierTable.GetData(currentSoilderId);
            var weaponSocket = unit.transform.Find("Bip001").Find("Bip001 Prop1");
            Instantiate(weapons[currentType][(int)currentSoilderData.Grade - 1], weaponSocket);
            party.Add(currentType, unit);
            unit.SetData(currentSoilderData, currentType);
            GetCurrentStats(unit);
            GetCurrentBulidngStats(unit);
        }
        OnUnitCreated?.Invoke();
        OnUnitUpdated?.Invoke();
    }

    public void GetCurrentStats(Unit unit)
    {
        var unitStats = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;

        for (int j = (int)UnitUpgradeTable.UpgradeType.AttackPoint; j <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; j++)
        {
            var currentUpgradeType = (UnitUpgradeTable.UpgradeType)j;
            var currentTypelevel = unitStats[currentUpgradeType];
            unit.GetSaveStats(currentUpgradeType, currentTypelevel);
        }
    }

    public void GetCurrentBulidngStats(Unit unit)
    {
        var buildingStats = SaveLoadManager.Data.buildingData.buildingLevels;

        for (int k = (int)BuildingTable.BuildingType.AttackPoint; k <= (int)BuildingTable.BuildingType.IdleTime; ++k)
        {
            var currentBuildingType = (BuildingTable.BuildingType)k;
            var currentBuildingLevel = buildingStats[currentBuildingType];
            unit.GetSaveBuildingStats(currentBuildingType, currentBuildingLevel);
        }
    }

}
