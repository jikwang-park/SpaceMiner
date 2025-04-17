using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAccumulator : MonoBehaviour
{
    private Dictionary<int, BigNumber> accumulatedMines = new Dictionary<int, BigNumber>();
    private List<int> planetIds;
    private StageManager stageManager;
    private bool isAccumulating = true;
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

        foreach (var planet in planetIds)
        {
            BigNumber productionPerSecond = MiningRobotInventoryManager.CalculateMiningAmountPerSecond(planet);
            int deltaScaled = (int)(delta * MiningRobotInventoryManager.ScaleFactor);
            BigNumber productionThisFrame = (productionPerSecond * deltaScaled) / MiningRobotInventoryManager.ScaleFactor;

            accumulatedMines[planet] += productionThisFrame;
        }
    }
    public void DoStageClear()
    {
        foreach(var accumulatedMine in accumulatedMines)
        {
            var itemId = DataTableManager.PlanetTable.GetData(accumulatedMine.Key).ItemID;
            var value = accumulatedMine.Value / MiningRobotInventoryManager.ScaleFactor;
            ItemManager.AddItem(itemId, value);
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
    }
}
