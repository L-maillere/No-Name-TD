using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int attackValue;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isFlying;
    [SerializeField] Vector2Int cellPosition;

    public void ApplyDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Death();
        }
    }

    private void Move()
    {
        //Voir avec Louis comment récupérer les pathPoints
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public Vector2Int GetCellFromWorldPosition(float cellSize)
    {
        return new Vector2Int((int)(transform.position.x / cellSize) , (int)(transform.position.z / cellSize));
    }

    void Update()
    {
        cellPosition = GetCellFromWorldPosition(1.0f);
    }
}
