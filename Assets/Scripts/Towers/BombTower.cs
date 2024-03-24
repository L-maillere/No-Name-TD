using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTower : Tower
{
    public BombTower()
    {
        firerate = 2f;
        projectileDamageValue = 5;
        attackRange = 1f;
        level = 0;
    }
    void Attack()
    {
        
    }
}
