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
        var slotOneData = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet[planetId][0];
        slotOne.Initialize(slotOneData);
        var slotTwoData = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet[planetId][1];
        slotTwo.Initialize(slotTwoData);
    }


}
