using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLaneManager : MonoBehaviour
{
    private List<MonsterController>[] lanes = new List<MonsterController>[3];
    public event Action AllDead;

    private void Awake()
    {
        for (int i = 0; i < lanes.Length; ++i)
        {
            lanes[i] = new List<MonsterController>();
        }
    }

    public void AddMonster(int lane, MonsterController monster)
    {
        var destructedEvent = monster.GetComponent<DestructedDestroyEvent>();
        if (destructedEvent != null)
        {
            lanes[lane].Add(monster);
            destructedEvent.OnDestroyed += () => RemoveMonster(lane, monster);
        }
    }

    public void RemoveMonster(int lane, MonsterController monster)
    {
        lanes[lane].Remove(monster);

        int count = 0;
        for (int i = 0; i < lanes.Length; ++i)
        {
            count += lanes[i].Count;
        }
        if (count == 0)
        {
            AllDead?.Invoke();
        }
    }

    public int GetMonsterCount(int lane)
    {
        if (lane < 0 || lane >= lanes.Length)
        {
            return 0;
        }

        return lanes[lane].Count;
    }

    public Transform GetFirstMonster(int lane)
    {
        if (lane < 0 || lane >= lanes.Length)
        {
            return null;
        }

        if (lanes[lane].Count == 0)
        {
            return null;
        }

        return lanes[lane][0].transform;
    }
}
