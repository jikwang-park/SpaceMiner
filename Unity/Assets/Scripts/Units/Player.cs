using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    private int id;
    private float minArrange;
    private float maxArrange;
    

    [SerializeField]
    private Dictionary<int, PlayerData> playerDictionary = new Dictionary<int, PlayerData>();




    private void Init()
    {
        
    }

    
}
