using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppAdvisory.DancingSnake
{
	public class Camera : MonoBehaviour 
	{
		[SerializeField] private float speed = 2f;

		public void InitCamera()
		{
			GameManager.instance.OnDoAction += DoAction;
		}

		void DoAction () 
		{
			Vector3 localPos = transform.localPosition;
			transform.localPosition = new Vector3 (localPos.x + speed * Time.deltaTime, localPos.y, localPos.z);
		}
	}
}