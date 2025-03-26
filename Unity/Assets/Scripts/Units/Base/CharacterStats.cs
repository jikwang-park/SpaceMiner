using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public BigNumber maxHp;
    public BigNumber damage;
    public BigNumber armor = 1;

    public BigNumber Hp { get; set; }

    private void Awake()
    {
        maxHp = new BigNumber("50");
        damage = new BigNumber("1");
    }

    private void OnEnable()
    {
        Hp = maxHp;
    }
}
