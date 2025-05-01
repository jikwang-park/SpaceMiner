using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnitUpgradeTable;

public class UnitPartyManager : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<UnitTypes, Unit> characters;

    [SerializeField]
    private SerializedDictionary<UnitTypes, GameObject[]> weapons;

    [SerializeField]
    private GameObject shield;

    [SerializeField]
    private List<Vector3> unitSpawnPos;

    public Dictionary<UnitTypes, Unit> PartyUnits { get; private set; } = new Dictionary<UnitTypes, Unit>();

    public event System.Action OnUnitCreated;
    public event System.Action OnUnitUpdated;

    public event System.Action OnUnitAllDead;

    [SerializeField]
    public UnitSkillButtonManager buttonManager;

    private Vector3 unitOffset = Vector3.left * 1f;

    public int UnitCount => PartyUnits.Count;

    public void ResetSkillCoolTime()
    {
        foreach (var unit in PartyUnits.Values)
        {
            unit.lastSkillTime = float.MinValue;
        }
    }
    public void RestUnitBarrier()
    {
        foreach(var unit in PartyUnits.Values)
        {
            unit.unitStats.barrier = 0;
        }
    }
    public void ResetUnitHealth()
    {
        foreach (var unit in PartyUnits.Values)
        {
            unit.unitStats.Hp = unit.unitStats.maxHp;
        }
    }

    public void ResetStatus()
    {
        foreach (var unit in PartyUnits.Values)
        {
            unit.ResetStatus();
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
        if (PartyUnits.ContainsKey(type))
        {
            UnitCombatPowerCalculator.Init(type);
            PartyUnits[type].SetData(data);
            ParticleEffectManager.Instance.PlayOneShot("UnitChangeEffect", PartyUnits[type].transform);
            UnitCombatPowerCalculator.CalculateTotalCombatPower();
        }
    }

    private void OnUnitDie(DestructedDestroyEvent sender)
    {
        var unit = sender.GetComponent<Unit>();
        PartyUnits.Remove(unit.UnitTypes);
        unit.gameObject.SetActive(false);
        if (PartyUnits.Count == 0)
        {
            OnUnitAllDead?.Invoke();
        }
    }

    public void AddStats(UnitUpgradeTable.UpgradeType type, float amount)
    {
        UnitCombatPowerCalculator.ChangeStats(type);
        UnitCombatPowerCalculator.CalculateTotalCombatPower();
    }

    public void UpgradeSkillStats(int id, UnitTypes type)
    {
        if (!PartyUnits.ContainsKey(type))
            return;

        var unit = PartyUnits[type];


        unit.Skill.UpgradeUnitSkillStats(id);
    }

    public void AddBuildingStats(BuildingTable.BuildingType type, float amount)
    {
        int index = (int)type;
        UnitCombatPowerCalculator.ChangeStats((UpgradeType)index);
        UnitCombatPowerCalculator.CalculateTotalCombatPower();
    }

    public void UnitDespawn()
    {
        foreach (var unit in PartyUnits)
        {
            Destroy(unit.Value.gameObject);
        }
        PartyUnits.Clear();
    }



    public Transform GetFirstLineUnitTransform()
    {
        if (PartyUnits.Count == 0)
        {
            Debug.LogError("Unit is Empty");
            return null;
        }

        Transform frontMostZPos = null;
        float maxZ = float.MinValue;

        foreach (var unit in PartyUnits.Values)
        {
            if (unit == null)
                continue;

            float currentUnitZpos = unit.transform.position.z;
            if (currentUnitZpos > maxZ)
            {
                maxZ = currentUnitZpos;
                frontMostZPos = unit.transform;
            }
        }

        return frontMostZPos;
    }


    public Transform GetUnit(UnitTypes type)
    {
        if (PartyUnits.ContainsKey(type))
        {
            return PartyUnits[type].transform;
        }
        return null;
    }

    public Unit GetCurrentTargetType(string targetString)
    {
        int target = int.Parse(targetString);
        if (PartyUnits.ContainsKey((UnitTypes)target))
        {
            return PartyUnits[(UnitTypes)target];
        }
        return null;
    }



    public bool IsUnitExistFront(UnitTypes myType)
    {
        for (int i = (int)myType - 1; i >= (int)UnitTypes.Tanker; --i)
        {
            if (PartyUnits.ContainsKey((UnitTypes)i))
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
            if (PartyUnits.ContainsKey((UnitTypes)i))
            {
                return true;
            }
        }
        return false;
    }

    public Unit GetFrontUnit(UnitTypes myType)
    {
        for (int i = (int)myType - 1; i >= (int)UnitTypes.Tanker; --i)
        {
            if (PartyUnits.ContainsKey((UnitTypes)i))
            {
                return PartyUnits[(UnitTypes)i];
            }
        }
        return null;
    }

    public Unit GetBackUnit(UnitTypes myType)
    {
        for (int i = (int)myType + 1; i <= (int)UnitTypes.Healer; ++i)
        {
            if (PartyUnits.ContainsKey((UnitTypes)i))
            {
                return PartyUnits[(UnitTypes)i];
            }
        }
        return null;
    }



    public void ResetUnits(Vector3 startPos)
    {
        Vector3 position = startPos;

        var stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();

        for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
        {
            var currentType = (UnitTypes)i;

            var unit = Instantiate(characters[currentType], position, Quaternion.identity);
            unit.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnUnitDie;
            position += unitOffset;
            var currentSoilderId = InventoryManager.GetInventoryData(currentType).equipElementID;
            var currentSoilderData = DataTableManager.SoldierTable.GetData(currentSoilderId);
            //var weaponSocket = unit.transform.Find("Bip001").Find("Bip001 Prop1");
            var weaponPosition = unit.RightHandPosition;
            var weaponGo = Instantiate(weapons[currentType][(int)currentSoilderData.Grade - 1], weaponPosition);
            var weaponPoint = weaponGo.transform.Find("BulletStarter");
            unit.GetComponent<UnitEffectController>().attackEffectPoint = weaponPoint;

            if (currentType == UnitTypes.Tanker)
            {
                var shieldPosition = unit.LeftHandPosition;
                var shieldGo = Instantiate(shield, shieldPosition);
            }

            PartyUnits.Add(currentType, unit);
            if (stageManager.IngameStatus != IngameStatus.LevelDesign)
            {
                UnitCombatPowerCalculator.Init(currentType);
            }
            unit.SetData(currentSoilderData);
        }
        OnUnitCreated?.Invoke();
        OnUnitUpdated?.Invoke();
    }





    public bool NeedHealUnit()
    {
        foreach (var unit in PartyUnits)
        {
            if (unit.Value.unitStats.HPRate < Variables.healerSkillHPRatio)
            {
                return true;
            }
        }
        return false;
    }
}
