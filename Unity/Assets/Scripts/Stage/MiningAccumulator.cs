using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAccumulator : MonoBehaviour
{
    private Dictionary<int, BigNumber> accumulatedMines = new Dictionary<int, BigNumber>();
    private List<int> planetIds;
    private StageManager stageManager;
    private bool isAccumulating = true;

    private const float rewardCycleTime = 10f;
    private float accumulateTime = 0f;
    private void Awake()
    {
        stageManager = GetComponent<StageManager>();
        stageManager.OnIngameStatusChanged += DoIngameStatusChanged;
    }
    void Start()
    {
        planetIds = DataTableManager.PlanetTable.GetIds();
        foreach (var planet in planetIds)
        {
            accumulatedMines.Add(planet, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAccumulating)
        {
            return;
        }

        float delta = Time.deltaTime;
        accumulateTime += delta;

        if(accumulateTime >= rewardCycleTime)
        {
            RewardMining();
            accumulateTime = 0f;
        }

        foreach (var planet in planetIds)
        {
            BigNumber productionPerSecond = MiningRobotInventoryManager.CalculateMiningAmountPerSecond(planet);
            int deltaScaled = (int)(delta * MiningRobotInventoryManager.ScaleFactor);
            BigNumber productionThisFrame = (productionPerSecond * deltaScaled) / MiningRobotInventoryManager.ScaleFactor;

            accumulatedMines[planet] += productionThisFrame;
        }
    }
    public void RewardMining()
    {
        foreach(var accumulatedMine in accumulatedMines)
        {
            var itemId = DataTableManager.PlanetTable.GetData(accumulatedMine.Key).ItemID;
            var value = accumulatedMine.Value / MiningRobotInventoryManager.ScaleFactor;
            ItemManager.AddItem(itemId, value);
            Debug.Log($"{(Currency)itemId} : {value} Get");
        }
        ResetAccumulator();
    }
    public void ResetAccumulator()
    {
        foreach (int planetId in planetIds)
        {
            accumulatedMines[planetId] = 0;
        }
    }
    public void DoIngameStatusChanged(IngameStatus ingameStatus)
    {
        isAccumulating = (ingameStatus != IngameStatus.Mine);
        accumulateTime = 0f;
    }
}
