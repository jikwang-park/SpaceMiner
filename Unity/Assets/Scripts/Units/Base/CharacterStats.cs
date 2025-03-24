using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public BigNumber maxHp;
    public BigNumber damage;
    public int armor = 1;

    public BigNumber Hp { get; set; }

    private void Awake()
    {
        maxHp = new BigNumber("50");
        damage = new BigNumber("1");
        Hp = maxHp;
    }
}
