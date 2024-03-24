using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBomb : Enemy
{
    public BallBomb()
    {
        maxHp = 200;
        currentHp = maxHp;
        attackValue = 3;
        speed = 2f;
        isFlying = false;
    }

    public void Attack(int attackValue)
    {

    }
}
