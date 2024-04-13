using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int attackValue;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isFlying;
    [SerializeField] Vector2Int cellPosition;
    [SerializeField] NavMeshAgent navMeshAgent;
    private Vector3 target;

    public void ApplyDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Death();
        }
    }

    private void Move(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public Vector2Int GetCellFromWorldPosition(float cellSize)
    {
        return new Vector2Int((int)(transform.position.x / cellSize) , (int)(transform.position.z / cellSize));
    }
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform.position;
        Debug.Log(target.ToString());
    }
    void Update()
    {
        cellPosition = GetCellFromWorldPosition(1.0f);
        Move(target);
    }
}
