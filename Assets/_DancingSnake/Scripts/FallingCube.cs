using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if AADOTWEEN
using DG.Tweening;
#endif

namespace AppAdvisory.DancingSnake
{
	public class FallingCube : MonoBehaviour 
	{
		[SerializeField] private List<GameObject> cubes;
		[SerializeField] private float delayBeforeFalling = 3f;
		[SerializeField] private float fallingSpeed = 1f;

		void Start()
		{
			StartCoroutine (WaitBeforeFalling ());
		}

		IEnumerator WaitBeforeFalling()
		{
			yield return new WaitForSeconds (delayBeforeFalling);

			float count = 0;
			foreach (GameObject cube in cubes) 
			{
				#if AADOTWEEN
				cube.transform.DOMoveY (-100, fallingSpeed + count).SetEase(Ease.Linear);
				#endif

				count += 0.4f;
			}
		}
	}
}