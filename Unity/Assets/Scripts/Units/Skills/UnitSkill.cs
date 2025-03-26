using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    Normal,
    Rare,
    Epic,
    Legend,
}
public abstract class UnitSkill : MonoBehaviour
{

    public float coolTime;

    protected float timer;

    protected Unit unit;

    protected float lastSkillUsingTime;

    protected StageManager stageManager;

    protected SkillType currentSkillType;


  
   
    private void Start()
    {
 
    }
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public abstract void Init();
    public abstract void ExcuteSkill();
    
}
