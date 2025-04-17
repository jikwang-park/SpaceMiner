using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    private int monsterCount;
    private ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();

        monsterLines = new Dictionary<int, Dictionary<int, MonsterController>>();
        monsterControllers = new HashSet<MonsterController>();
        laneMonsterCounts = new int[laneCount];
        monsterCount = 0;
    }

    public void AddMonster(int lane, MonsterController monster)
    {
        var destructedEvent = monster.GetComponent<DestructedDestroyEvent>();
        if (destructedEvent != null)
        {
            ++monsterCount;
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
        monsterCount = 0;

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
            ItemManager.AddItem(monsterController.RewardData.RewardItemID1, monsterController.RewardData.RewardItemCount1);

            int reward2index = monsterController.RewardData.RandomReward2();
            if (reward2index > -1)
            {
                ItemManager.AddItem(monsterController.RewardData.RewardItemID2, monsterController.RewardData.counts[reward2index]);
            }
        }

        OnMonsterDie?.Invoke();

        --monsterCount;

        if (monsterCount == 0)
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
            if (!monsterLines.ContainsKey(currentFrontLine + 1))
            {
                return;
            }
            ++currentFrontLine;
            foreach (var nextMonster in monsterLines[currentFrontLine])
            {
                nextMonster.Value.currentLine = -1;
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
        return !monsterLines.ContainsKey(line - 1);
    }

    public void Spawn(Vector3 frontPosition, CorpsTable.Data data)
    {
        if (data.FrontSlots == 0 && data.BackSlots == 0 && data.BossMonsterID != 0)
        {
            int lane = 1;
            SpawnMonster(lane, lane, frontPosition, data.BossMonsterID);
            EndLine();
            return;
        }

        int frontTypeCount = data.NormalMonsterIDs.Length;
        int eachMaxCount = data.FrontSlots / frontTypeCount;
        int[] createdCount = new int[eachMaxCount];

        for (int i = 0; i < data.FrontSlots; ++i)
        {
            int index = Random.Range(0, frontTypeCount);
            if (i < eachMaxCount * frontTypeCount)
            {
                while (createdCount[index] >= eachMaxCount)
                {
                    index = Random.Range(0, frontTypeCount);
                }
            }

            int monsterId = data.NormalMonsterIDs[index];
            int lane = i % 3;
            SpawnMonster(lane, i, frontPosition, monsterId);
            ++createdCount[index];
        }
        EndLine();

        int backStartPos = Mathf.CeilToInt((float)data.FrontSlots / 3) * 3;

        for (int i = 0, j = backStartPos; i < data.BackSlots; ++i, ++j)
        {
            int lane = i % 3;
            SpawnMonster(lane, j, frontPosition, data.RangedMonsterID);
        }
        EndLine();
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
        monster.transform.parent = null;
        monster.transform.position = frontPosition + SpawnPoints[index];
        monster.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        AddMonster(lane, monsterController);
    }
}
