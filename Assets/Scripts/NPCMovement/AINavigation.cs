using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
	public Transform travelTarget;
	private NavMeshAgent agent;
	[SerializeField] private GameObject mainTarget;
	private float restoreSpeed;
	[SerializeField] private float speed = 5;
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		travelTarget = mainTarget.transform;
		print(travelTarget);
		agent.SetDestination(travelTarget.position);
		agent.speed = speed;
		restoreSpeed = speed;
	}

	void Update()
	{
		agent.SetDestination(travelTarget.position);
	}

	public IEnumerator DelayCoroutine(float delay)
	{

		agent.speed = 0;
		yield return new WaitForSeconds(delay);
		agent.speed = restoreSpeed;
	}
}
