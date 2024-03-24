using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCrab : Enemy
{
    public BigCrab()
    {
        maxHp = 100;
        currentHp = maxHp;
        attackValue = 10;
        speed = 1f;
        isFlying = false;
    }

    public void Attack(int attackValue)
    {

    }
}
