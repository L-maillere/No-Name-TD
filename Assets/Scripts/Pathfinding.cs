using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
	GameObject base;
	NavMeshAgent agent;

	void start()
	{
		base = GameObject.FindWithTag("Player"):
		agent = this.GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		agent.SetDestination(base.transform.position);
	}
}