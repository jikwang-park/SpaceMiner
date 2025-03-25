using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLaneManager : MonoBehaviour
{
    [SerializeField]
    private int laneCount = 3;

    private Dictionary<int, Dictionary<int, MonsterController>> monsterLines;

    private int currentFrontLine = 0;
    private int currentLastLine = 0;

    private int[] laneMonsterCounts;
    public int LaneCount => laneCount;

    private void Awake()
    {
        monsterLines = new Dictionary<int, Dictionary<int, MonsterController>>();
        laneMonsterCounts = new int[laneCount];
    }

    public void AddMonster(int lane, MonsterController monster)
    {
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
            if (monsterLines.ContainsKey(currentLastLine - 1))
            {
                monster.frontLine = currentLastLine - 1;
            }
            else
            {
                monster.frontLine = -1;
            }
            monster.findFrontMonster = GetLineMonster;
            ++laneMonsterCounts[lane];
            int createdLine = currentLastLine;
            destructedEvent.OnDestroyed += (_) => RemoveMonster(createdLine, lane, monster);
        }
    }

    public void RemoveMonster(int createdLine, int lane, MonsterController monster)
    {
        monsterLines[createdLine].Remove(lane);
        --laneMonsterCounts[lane];
        if (monsterLines[createdLine].Count == 0)
        {
            monsterLines.Remove(createdLine);
            if (monsterLines.ContainsKey(currentFrontLine + 1))
            {
                ++currentFrontLine;
                foreach (var nextMonster in monsterLines[currentFrontLine])
                {
                    nextMonster.Value.frontLine = -1;
                }
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
}
