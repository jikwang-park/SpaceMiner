using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUiManager : MonoBehaviour
{
    private Dictionary<UnitTypes,Slider> hpDic = new Dictionary<UnitTypes,Slider>();

    [SerializeField]
    private StageManager stageManager;



    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        
    }

    private void Start()
    {
    }


   
}
