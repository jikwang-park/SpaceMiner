using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkill : MonoBehaviour
{

    public float coolTime;

    protected float timer;

    protected Unit unit;

    protected float lastSkillUsingTime;

    protected StageManager stageManager;

    public Grade currentSkillGrade;

    protected UnitStats currentStats;

    public float duration;

    public List<Unit> targetList;

    public UnitTypes currentType;

    public int currentSkillId;

    public int nextId;

    public float remainCooltime = 0;

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
 

    public virtual void TankerInit(UnitTypes type , Grade grade) { }


    public virtual void DealerInit(UnitTypes type, Grade grade) { }

    public virtual void HealerInit(UnitTypes type, Grade grade) { }
        

    public virtual void ExecuteSkill()
    {
        remainCooltime = coolTime;
    }
    public virtual void Update()
    {
        if(remainCooltime >0)
        {
            remainCooltime -= Time.deltaTime;
            remainCooltime = Mathf.Max(remainCooltime, 0f);
        }

    }

    public virtual void UpgradeUnitSkillStats(int id)
    {
        
    }

}
