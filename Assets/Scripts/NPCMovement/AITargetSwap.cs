using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetSwap : MonoBehaviour
{
	[SerializeField] GameObject locattionHolder;
	[SerializeField] private Transform[] locations;
	private int locationCount = 0;
	private readonly System.Random rnd = new();
	private int rolledNumber = 0;
	bool sameROll;
	// Start is called before the first frame update
	void Start()
	{
		locations = locattionHolder.GetComponentsInChildren<Transform>();
		locationCount = locations.Length;
	}

	private void OnTriggerEnter(Collider _collider)
	{
		int previosVal = rolledNumber;
		rolledNumber = rnd.Next(locationCount);
		print(rolledNumber);
		if (previosVal == rolledNumber)
		{
			sameROll = true;
		}
		while (sameROll)
		{
			print("Duplicate roll");
			rolledNumber = rnd.Next(locationCount);
			print(rolledNumber);
			if (previosVal != rolledNumber)
			{
				sameROll = false;
			}
		}
		StartCoroutine(_collider.GetComponent<AINavigation>().DelayCoroutine(rolledNumber + 2));
		transform.position = locations[rolledNumber].position;
	}
	
}
