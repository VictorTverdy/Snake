using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppAdvisory.DancingSnake
{
	public class Obstacle : MonoBehaviour 
	{
		[SerializeField] private float delayBeforeDestroy = 10f;

		void Start()
		{
			StartCoroutine (KillAfterDelay ());
		}

		IEnumerator KillAfterDelay()
		{
			yield return new WaitForSeconds (delayBeforeDestroy);

			Destroy (gameObject);
		}
	}
}