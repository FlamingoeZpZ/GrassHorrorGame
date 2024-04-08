using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
	public Transform travelTarget;
	private NavMeshAgent agent;
	[SerializeField] private string MainTarget;

	[SerializeField] private float speed = 5;
	void Start()
	{
		agent=GetComponent<NavMeshAgent>();
		travelTarget = GameObject.Find(MainTarget).transform;
		print(travelTarget);
		agent.SetDestination(travelTarget.position);
		agent.speed=speed;
	}

	void Update()
	{
		agent.SetDestination(travelTarget.position);
	}

	public IEnumerator DelayCoroutine(float delay)
	{
		float restoreSpeed = speed;
		speed = 0;
		yield return new WaitForSeconds(delay);
		speed = restoreSpeed;
	}
}
