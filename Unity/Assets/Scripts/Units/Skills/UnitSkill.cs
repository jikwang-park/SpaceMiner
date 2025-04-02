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
public class UnitSkill : MonoBehaviour
{

    public float coolTime;

    protected float timer;

    protected Unit unit;

    protected float lastSkillUsingTime;

    protected StageManager stageManager;

    protected SkillType currentSkillType;

    protected UnitStats currentStats;

    public float duration;

    public List<Unit> targetList;



    public bool CanSaerchTarget;

   
    private void Start()
    {
        
    }
    protected virtual void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        targetList = new List<Unit>();
    }
    public virtual void GetTarget()
    {

    }

    public virtual void Init()
    {

    }
    public virtual void ExecuteSkill()
    {

    }
    
}
