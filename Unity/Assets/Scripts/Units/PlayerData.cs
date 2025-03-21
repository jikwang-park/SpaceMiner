using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerType
{
    Tanker,
    Dealer,
    Healer,
}
public class PlayerData 
{
    public int Id {  get; set; }
    public float MinArrange { get; set; }
    public float MaxArrange { get; set; }

    public PlayerType PlayerType { get; set; }
}
