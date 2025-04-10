using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMiningRobotToPlanetUI : MonoBehaviour
{
    [SerializeField]
    MiningRobotInventorySlot slotOne;
    [SerializeField]
    MiningRobotInventorySlot slotTwo;

    private void Awake()
    {
        MiningRobotInventoryManager.onEquipRobot += Initialize;
    }

    public void Initialize(int planetId)
    {
        if (!MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet.ContainsKey(planetId))
        {
            SaveLoadManager.Data.miningRobotInventorySaveData = MiningRobotInventoryData.CreateDefault();
        }
        var slotOneData = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet[planetId][0];
        slotOne.Initialize(slotOneData);
        var slotTwoData = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet[planetId][1];
        slotTwo.Initialize(slotTwoData);

    }


}
