using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkill : MonoBehaviour
{
    protected float timer;

    protected float currentTime;

    protected abstract void UseSkill();


}
