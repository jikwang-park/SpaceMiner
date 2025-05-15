using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageMonsterManager : MonoBehaviour
{
    [SerializeField]
    private int laneCount = 3;


    [field: SerializeField]
    public Vector3[] SpawnPoints { get; private set; } = new Vector3[]
    {
        new Vector3(-3f, 0f, 2f),
        new Vector3(0f, 0f, 2f),
        new Vector3(3f, 0f, 2f),
        new Vector3(-3f, 0f, 6f),
        new Vector3(0f, 0f, 6f),
        new Vector3(3f, 0f, 6f),
        new Vector3(-3f, 0f, 10f),
        new Vector3(0f, 0f, 10f),
        new Vector3(3f, 0f, 10f),
        new Vector3(-3f, 0f, 14f),
        new Vector3(0f, 0f, 14f),
        new Vector3(3f, 0f, 14f),
    };

    public event Action OnMonsterCleared;
    public event Action OnMonsterDie;

    private Dictionary<int, Dictionary<int, MonsterController>> monsterLines;
    private HashSet<MonsterController> monsterControllers;

    private int currentFrontLine = 0;
    private int currentLastLine = 0;

    private int[] laneMonsterCounts;
    public int LaneCount => laneCount;
    public int MonsterCount { get; private set; }
    private ObjectPoolManager objectPoolManager;
    private StageManager stageManager;

    private float[] weights = new float[3];

    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
        stageManager = GetComponent<StageManager>();

        monsterLines = new Dictionary<int, Dictionary<int, MonsterController>>();
        monsterControllers = new HashSet<MonsterController>();
        laneMonsterCounts = new int[laneCount];
        MonsterCount = 0;
    }

    public void AddMonster(int lane, MonsterController monster)
    {
        var destructedEvent = monster.GetComponent<DestructedDestroyEvent>();
        if (destructedEvent != null)
        {
            ++MonsterCount;
            monsterControllers.Add(monster);
            if (!monsterLines.ContainsKey(currentLastLine))
            {
                monsterLines.Add(currentLastLine, new Dictionary<int, MonsterController>());
            }
            monsterLines[currentLastLine].Add(lane, monster);
            monster.currentLine = currentLastLine;
            monster.isFrontMonster = IsFrontLine;
            monster.findFrontMonster = GetFrontLineMonster;
            ++laneMonsterCounts[lane];
            int createdLine = currentLastLine;
            destructedEvent.OnDestroyed += (sender) => RemoveMonster(sender, createdLine, lane, monster);
            if (monsterLines[currentLastLine].Count == laneCount)
            {
                ++currentLastLine;
                monsterLines.Add(currentLastLine, new Dictionary<int, MonsterController>());
            }
        }
    }

    public void StopMonster()
    {
        foreach (var line in monsterLines)
        {
            foreach (var monster in line.Value)
            {
                monster.Value.enabled = false;
            }
        }
    }

    public void ClearMonster()
    {
        for (int i = 0; i < laneMonsterCounts.Length; ++i)
        {
            laneMonsterCounts[i] = 0;
        }
        MonsterCount = 0;

        while (monsterControllers.Count > 0)
        {
            var monster = monsterControllers.First();
            monster.Release();
        }

        currentFrontLine = currentLastLine;

        monsterControllers.Clear();
        monsterLines.Clear();
    }

    public void RemoveMonster(DestructedDestroyEvent sender, int createdLine, int lane, MonsterController monster)
    {
        if (!monsterLines.ContainsKey(createdLine))
        {
            return;
        }

        var monsterController = sender.GetComponent<MonsterController>();
        if (monsterController.MonsterData.RewardTableID != 0)
        {
            ItemManager.AddItem(monsterController.RewardData.RewardItemID1, new BigNumber(monsterController.RewardData.RewardItemCount1) * weights[2]);

            int reward2index = monsterController.RewardData.RandomReward2();
            if (reward2index > -1)
            {
                ItemManager.AddItem(monsterController.RewardData.RewardItemID2, monsterController.RewardData.counts[reward2index]);
            }
        }

        OnMonsterDie?.Invoke();

        --MonsterCount;

        if (MonsterCount == 0)
        {
            OnMonsterCleared?.Invoke();
        }

        if (monsterLines[createdLine][lane] == monster)
        {
            monsterLines[createdLine].Remove(lane);
            --laneMonsterCounts[lane];
        }
        if (monsterLines[createdLine].Count == 0)
        {
            monsterLines.Remove(createdLine);
            //if (!monsterLines.ContainsKey(currentFrontLine + 1))
            //{
            //    return;
            //}

            if (monsterLines.Count == 0)
            {
                ++currentFrontLine;
            }
            else
            {
                int frontLine = int.MaxValue;

                foreach (var monsterLine in monsterLines)
                {
                    frontLine = Mathf.Min(frontLine, monsterLine.Key);
                }
                int previousFrontLine = currentFrontLine;
                currentFrontLine = frontLine;

                //foreach (var nextMonster in monsterLines[currentFrontLine])
                //{
                //    nextMonster.Value.currentLine = -1;
                //}

                //if (previousFrontLine != createdLine)
                //{
                //    int back = createdLine;
                //    int front = createdLine;
                //    while (back < currentLastLine)
                //    {
                //        ++back;
                //        if (monsterLines.ContainsKey(back))
                //        {
                //            break;
                //        }
                //    }
                //    while (front > currentFrontLine)
                //    {
                //        ++front;
                //        if (monsterLines.ContainsKey(front))
                //        {
                //            break;
                //        }
                //    }

                //    if (back != currentLastLine)
                //    {
                //        foreach (var nextMonster in monsterLines[currentFrontLine])
                //        {
                //            nextMonster.Value.currentLine = -1;
                //        }
                //    }
                //}
            }


        }
    }

    public void RemoveFromMonsterSet(MonsterController monster)
    {
        monsterControllers.Remove(monster);
    }

    public int GetMonsterCount(int lane)
    {
        if (lane < 0 || lane >= laneMonsterCounts.Length)
        {
            return 0;
        }
        if (!monsterLines.ContainsKey(currentFrontLine))
        {
            return 0;
        }
        if (!monsterLines[currentFrontLine].ContainsKey(lane))
        {
            return 0;
        }
        return laneMonsterCounts[lane];
    }

    public Transform GetFirstMonster(int lane)
    {
        if (lane < 0 || lane >= laneMonsterCounts.Length)
        {
            return null;
        }

        if (laneMonsterCounts[lane] == 0)
        {
            return null;
        }

        return GetLineMonster(currentFrontLine);
    }

    public List<Transform> GetMonsters(int count)
    {
        List<Transform> monsters = new List<Transform>();

        int line = currentFrontLine;

        while (line <= currentLastLine && monsters.Count < count)
        {
            if (!monsterLines.ContainsKey(line)
                || monsterLines[line].Count == 0)
            {
                ++line;
                continue;
            }

            for (int i = 0; i < laneCount; ++i)
            {
                if (!monsterLines[line].ContainsKey(i))
                {
                    continue;
                }
                monsters.Add(monsterLines[line][i].transform);
                if (monsters.Count >= count)
                {
                    break;
                }
            }
            ++line;
        }
        return monsters;
    }

    public List<Transform> GetMonsters(int count, Transform unit, float range)
    {
        List<Transform> monsters = new List<Transform>();

        int line = currentFrontLine;

        while (line <= currentLastLine && monsters.Count < count)
        {
            if (!monsterLines.ContainsKey(line)
                || monsterLines[line].Count == 0)
            {
                ++line;
                continue;
            }

            for (int i = 0; i < laneCount; ++i)
            {
                if (!monsterLines[line].ContainsKey(i))
                {
                    continue;
                }
                switch (stageManager.IngameStatus)
                {
                    case IngameStatus.Planet:
                    case IngameStatus.Dungeon:
                    case IngameStatus.LevelDesign:
                        if (monsterLines[line][i].transform.position.z - unit.position.z <= range)
                        {
                            monsters.Add(monsterLines[line][i].transform);
                        }
                        break;
                    case IngameStatus.Mine:
                        Vector3 displacement = monsterLines[line][i].transform.position - unit.position;
                        displacement.y = 0f;
                        if (Vector3.Magnitude(displacement) <= range)
                        {
                            monsters.Add(monsterLines[line][i].transform);
                        }
                        break;
                }
                if (monsters.Count >= count)
                {
                    break;
                }
            }
            ++line;
        }
        return monsters;
    }

    public MonsterController GetMonster(Vector3 position, float range)
    {
        float sqrDistance = float.MaxValue;
        MonsterController monsterController = null;
        Vector3 displacement;
        foreach (var monster in monsterControllers)
        {
            displacement = position - monster.transform.position;
            displacement.y = 0f;
            float tempsqrDist = Vector3.SqrMagnitude(displacement);
            if (tempsqrDist < sqrDistance)
            {
                sqrDistance = tempsqrDist;
                monsterController = monster;
            }
        }
        if (sqrDistance < range * range)
        {
            return monsterController;
        }
        else
        {
            return null;
        }
    }

    public Transform GetLineMonster(int line)
    {
        if (!monsterLines.ContainsKey(line))
        {
            return null;
        }

        if (monsterLines[line].Count == 0)
        {
            return null;
        }

        for (int i = 0; i < laneCount; ++i)
        {
            if (monsterLines[line].ContainsKey(i))
            {
                return monsterLines[line][i].transform;
            }
        }

        return null;
    }

    public Transform GetFrontLineMonster(int line)
    {
        int findline = line - 1;

        while (findline >= currentFrontLine)
        {
            if (monsterLines.ContainsKey(findline) && monsterLines[findline].Count > 0)
            {
                for (int i = 0; i < laneCount; ++i)
                {
                    if (monsterLines[findline].ContainsKey(i))
                    {
                        return monsterLines[findline][i].transform;
                    }
                }
            }

            --findline;
        }

        return null;


        int front = line - 1;

        if (!monsterLines.ContainsKey(front))
        {
            return null;
        }

        if (monsterLines[front].Count == 0)
        {
            return null;
        }

        for (int i = 0; i < laneCount; ++i)
        {
            if (monsterLines[front].ContainsKey(i))
            {
                return monsterLines[front][i].transform;
            }
        }

        return null;
    }

    public bool IsFrontLine(int line)
    {
        int findline = line - 1;
        while (findline >= currentFrontLine)
        {
            if (monsterLines.ContainsKey(findline))
            {
                return false;
            }
            --findline;
        }
        return true;
    }

    public void Spawn(Vector3 frontPosition, CorpsTable.Data data)
    {
        frontPosition.x = 0f;
        if (data.FrontSlots == 0 && data.BackSlots == 0 && data.BossMonsterID != 0)
        {
            int lane = 1;
            SpawnMonster(lane, lane, frontPosition, data.BossMonsterID);
            EndLine();
            return;
        }

        int TypeCount = data.NormalMonsterIDs.Length;
        int eachMaxCount = data.FrontSlots / TypeCount;
        int[] createdCount = new int[TypeCount];

        for (int i = 0; i < data.FrontSlots; ++i)
        {
            int index = Random.Range(0, TypeCount);
            if (i < eachMaxCount * TypeCount)
            {
                while (createdCount[index] >= eachMaxCount)
                {
                    index = Random.Range(0, TypeCount);
                }
            }

            int monsterId = data.NormalMonsterIDs[index];
            int lane = i % 3;
            SpawnMonster(lane, i, frontPosition, monsterId);
            ++createdCount[index];
        }
        EndLine();

        int backStartPos = Mathf.CeilToInt((float)data.FrontSlots / 3) * 3;
        TypeCount = data.RangedMonsterIDs.Length;
        eachMaxCount = data.BackSlots / TypeCount;
        createdCount = new int[TypeCount];

        for (int i = 0, j = backStartPos; i < data.BackSlots; ++i, ++j)
        {
            int index = Random.Range(0, TypeCount);
            if (i < eachMaxCount * TypeCount)
            {
                while (createdCount[index] >= eachMaxCount)
                {
                    index = Random.Range(0, TypeCount);
                }
            }

            int monsterId = data.RangedMonsterIDs[index];
            int lane = i % 3;
            SpawnMonster(lane, j, frontPosition, monsterId);
            ++createdCount[index];
        }
        EndLine();
    }

    public void Spawn(Vector3 frontPosition, LevelDesignStageStatusMachine.WaveMonsterData data, MonsterSkillTable.Data skillData)
    {
        if (data.slots[0] == 0 && data.slots[1] == 0 && data.slots[2] != 0)
        {
            int lane = 1;
            SpawnMonster(lane, lane, frontPosition, 91001, data, MonsterType.Boss, skillData);
            EndLine();
            return;
        }

        for (int i = 0; i < data.slots[0]; ++i)
        {
            int monsterId = 12001;
            int lane = i % 3;
            SpawnMonster(lane, i, frontPosition, monsterId, data, MonsterType.Normal);
        }
        EndLine();

        int backStartPos = Mathf.CeilToInt((float)data.slots[0] / 3) * 3;

        for (int i = 0, j = backStartPos; i < data.slots[1]; ++i, ++j)
        {
            int monsterId = 13001;
            int lane = i % 3;
            SpawnMonster(lane, j, frontPosition, monsterId, data, MonsterType.Ranged);
        }
        EndLine();
    }

    public MonsterController Spawn(Vector3 SpawnPosition, int monsterId)
    {
        var monsterData = DataTableManager.MonsterTable.GetData(monsterId);

        var prefabID = DataTableManager.AddressTable.GetData(monsterData.PrefabID);
        var monster = objectPoolManager.Get(prefabID);
        var monsterController = monster.GetComponent<MonsterController>();
        monsterController.enabled = true;
        monsterController.SetMonsterId(monsterId);

        monsterController.SetWeight(weights);

        monster.transform.parent = null;
        monster.transform.position = SpawnPosition;
        monster.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        stageManager.StageUiManager.HPBarManager.SetHPBar(monster.transform);
        ++MonsterCount;
        monsterControllers.Add(monsterController);

        monsterController.GetComponent<DestructedDestroyEvent>().OnDestroyed += (sender) =>
        {
            --MonsterCount;
        };

        return monsterController;
    }

    private void EndLine()
    {
        if (monsterLines[currentLastLine].Count == 0 || monsterLines[currentLastLine].Count == laneCount)
        {
            return;
        }
        ++currentLastLine;
        monsterLines.Add(currentLastLine, new Dictionary<int, MonsterController>());
    }

    private void SpawnMonster(int lane, int index, Vector3 frontPosition, int monsterId)
    {
        var monsterData = DataTableManager.MonsterTable.GetData(monsterId);

        var prefabID = DataTableManager.AddressTable.GetData(monsterData.PrefabID);
        var monster = objectPoolManager.Get(prefabID);
        var monsterController = monster.GetComponent<MonsterController>();
        monsterController.enabled = true;
        monsterController.SetMonsterId(monsterId);

        monsterController.SetWeight(weights);

        monster.transform.parent = null;
        monster.transform.position = frontPosition + SpawnPoints[index];
        monster.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        stageManager.StageUiManager.HPBarManager.SetHPBar(monster.transform);

        AddMonster(lane, monsterController);
    }

    private void SpawnMonster(int lane, int index, Vector3 frontPosition, int monsterId,
        LevelDesignStageStatusMachine.WaveMonsterData data,
        MonsterType type,
        MonsterSkillTable.Data skillData = null)
    {
        var monsterData = DataTableManager.MonsterTable.GetData(monsterId);

        var prefabID = DataTableManager.AddressTable.GetData(monsterData.PrefabID);
        var monster = objectPoolManager.Get(prefabID);
        var monsterController = monster.GetComponent<MonsterController>();
        monsterController.enabled = true;
        monsterController.SetMonsterId(monsterId);
        monsterController.monsterType = type;

        monster.transform.parent = null;
        monster.transform.position = frontPosition + SpawnPoints[index];
        monster.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        var stat = monster.GetComponent<MonsterStats>();
        stat.damage = data.attack[(int)type];
        stat.maxHp = data.maxHp[(int)type];
        stat.Hp = data.maxHp[(int)type];
        stat.range = data.attackRange[(int)type];
        stat.coolDown = 100f / data.attackSpeed[(int)type];
        stat.moveSpeed = data.moveSpeed[(int)type];

        monsterController.SetWeight(weights);

        if (type == MonsterType.Boss)
        {
            var skill = monster.GetComponent<MonsterSkill>();
            skill.SetSkill(skillData, stat);
        }

        stageManager.StageUiManager.HPBarManager.SetHPBar(monster.transform);

        AddMonster(lane, monsterController);
    }

    public void SetWeight(float[] weight)
    {
        weight.CopyTo(weights, 0);
    }
}
