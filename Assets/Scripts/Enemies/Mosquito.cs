using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : Enemy
{
    private float flightHeight;
    
    public Mosquito()
    {
        maxHp = 35;
        currentHp = maxHp;
        attackValue = 3;
        speed = 2f;
        isFlying = true;
    }

    public void Attack(int attackValue)
    {

    }
}
