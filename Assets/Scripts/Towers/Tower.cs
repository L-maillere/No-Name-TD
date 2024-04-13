using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public GameObject[] projectilePrefab;
    public float firerate;
    public bool onCooldown = false;
    public int projectileDamageValue;
    public float projectileSpeed;
    public Vector2Int cellPosition;
    public float attackRange;
    public int level;

    public void LevelUp()
    {
        level += 1;
    }

    public Vector3 GetWorldFromCellPosition(Vector2Int cellPosition, float cellSize)
    {
        return new Vector3(cellPosition.x * cellSize, 0, cellPosition.y * cellSize);
    }

    public IEnumerator StartCooldown()
	{
		onCooldown = true;

		yield return new WaitForSeconds(firerate);

		onCooldown = false;
	}
/*
    public Enemy FindClosestEnemy()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("enemy");
        
        foreach (GameObject target in targets)
        {
            float distance = (target.transform.position - towerPrefab[level].transform.position).sqrMagnitude;
            float lastDistance = Mathf.Infinity;
            if (distance < attackRange && distance < lastDistance)
            {
                lastDistance = distance;
                Enemy enemy = (Enemy)target.GetComponent(typeof(Enemy));
                return enemy;
            }
        }
        return null;
    }
*/
    public Enemy FindMostAdvancedEnemy(List<Vector2Int> pathPoints)
    {
        //Check Enemy cellposition, check which Enemy is closest to next tile limit.
        GameObject[] targets = GameObject.FindGameObjectsWithTag("enemy");
        
        foreach (GameObject target in targets)
        {
            Enemy enemy = (Enemy)target.GetComponent(typeof(Enemy));
            
        }
        return null;
    }
}