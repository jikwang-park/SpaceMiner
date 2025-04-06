using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMonsterManager : MonoBehaviour
{
    [SerializeField]
    private int laneCount = 3;

    public event Action OnMonsterCleared;
    public event Action OnMonsterDie;

    private Dictionary<int, Dictionary<int, MonsterController>> monsterLines;

    private int currentFrontLine = 0;
    private int currentLastLine = 0;

    private int[] laneMonsterCounts;
    public int LaneCount => laneCount;
    private int monsterCount = 0;

    private WaveSpawner waveSpawner;

    private void Awake()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        waveSpawner.OnMonsterSpawn += AddMonster;

        monsterLines = new Dictionary<int, Dictionary<int, MonsterController>>();
        laneMonsterCounts = new int[laneCount];
    }

    public void ClearMonster()
    {
        monsterCount = 0;
        for (int i = 0; i < laneMonsterCounts.Length; ++i)
        {
            laneMonsterCounts[i] = 0;
        }
        foreach (var line in monsterLines)
        {
            foreach (var monster in line.Value)
            {
                monster.Value.Release();
            }
            if (!monsterLines.ContainsKey(currentFrontLine + 1))
            {
                break;
            }
            ++currentFrontLine;
        }
        monsterLines.Clear();
    }

    public void AddMonster(int lane, MonsterController monster)
    {
        ++monsterCount;
        var destructedEvent = monster.GetComponent<DestructedDestroyEvent>();
        if (destructedEvent != null)
        {
            if (!monsterLines.ContainsKey(currentLastLine))
            {
                monsterLines.Add(currentLastLine, new Dictionary<int, MonsterController>());
            }
            if (monsterLines[currentLastLine].ContainsKey(lane))
            {
                ++currentLastLine;
                monsterLines.Add(currentLastLine, new Dictionary<int, MonsterController>());
            }
            monsterLines[currentLastLine].Add(lane, monster);
            monster.currentLine = currentLastLine;
            monster.isFrontMonster = IsFrontLine;
            monster.findFrontMonster = GetFrontLineMonster;
            ++laneMonsterCounts[lane];
            int createdLine = currentLastLine;
            destructedEvent.OnDestroyed += (sender) => RemoveMonster(sender, createdLine, lane, monster);
        }
    }

    public void RemoveMonster(DestructedDestroyEvent sender, int createdLine, int lane, MonsterController monster)
    {
        if (!monsterLines.ContainsKey(createdLine))
        {
            return;
        }

        --monsterCount;

        var monsterController = sender.GetComponent<MonsterController>();
        ItemManager.AddItem(monsterController.RewardData.Reward1, monsterController.RewardData.Count);

        int reward2index = monsterController.RewardData.RandomReward2();
        if (reward2index > -1)
        {
            ItemManager.AddItem(monsterController.RewardData.Reward2, monsterController.RewardData.counts[reward2index]);
        }

        OnMonsterDie?.Invoke();

        if (monsterCount == 0)
        {
            OnMonsterCleared?.Invoke();
        }

        monsterLines[createdLine].Remove(lane);
        --laneMonsterCounts[lane];
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

    public void Spawn(Vector3 position, CorpsTable.Data data)
    {
        waveSpawner.Spawn(position, data);
    }
}
