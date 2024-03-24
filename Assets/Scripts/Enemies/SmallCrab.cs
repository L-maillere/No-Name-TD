using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCrab : Enemy
{
    public SmallCrab()
    {
        maxHp = 35;
        currentHp = maxHp;
        attackValue = 3;
        speed = 4f;
        isFlying = false;
    }

    public void Attack(int attackValue)
    {

    }
}
