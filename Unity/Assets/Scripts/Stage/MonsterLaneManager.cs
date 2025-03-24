using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLaneManager : MonoBehaviour
{
    [SerializeField]
    private int laneCount = 3;

    private List<MonsterController>[] lanes;
    public int LaneCount => laneCount;

    private void Awake()
    {
        lanes = new List<MonsterController>[laneCount];
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
            destructedEvent.OnDestroyed += (_) => RemoveMonster(lane, monster);
        }
    }

    public void RemoveMonster(int lane, MonsterController monster)
    {
        lanes[lane].Remove(monster);
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
