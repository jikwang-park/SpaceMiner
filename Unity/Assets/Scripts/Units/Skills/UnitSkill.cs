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

    protected Unit unit;

    protected float lastSkillUsingTime;

    private StageManager stageManager;

    protected SkillType currentType;


    private Transform TragetTransform
    {
        get
        {
            if (unit == null)
                return null;

            return unit.targetPos;
        }
    }
   
    private void Start()
    {
 
    }
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        unit = stageManager.UnitPartyManager.GetFirstLineUnitGo();
    }

 


    public abstract void Init();
    public abstract void SetTarget(List<Transform> target);
    public abstract void ExcuteSkill();
    
}
