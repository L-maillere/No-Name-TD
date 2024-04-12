using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
	GameObject Base;
	NavMeshAgent agent;

	void Start()
	{
		Base = GameObject.FindWithTag("Player");
		agent = this.GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		agent.SetDestination(Base.transform.position);
	}
}