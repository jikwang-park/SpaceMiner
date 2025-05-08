using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardEntry
{
    public string uid;
    public string name;
    public string display;
    public string sortKey;
    public long timestamp;
}
public class MyRankEntry
{
    public int rank;
    public LeaderBoardEntry myEntry;
}
