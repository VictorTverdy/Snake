using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if AADOTWEEN
using DG.Tweening;
#endif

namespace AppAdvisory.DancingSnake
{
	public class Character : MonoBehaviour 
	{
		public delegate void OnGameOverEvent();
		public event OnGameOverEvent OnGameOver;

		public static Character instance = null;

		bool isGoingLeft = true;

		[SerializeField] private GameObject snakePrefab;

		[SerializeField] private Vector3 goingLeftPosition;
		[SerializeField] private Vector3 goingRightPosition;

		[SerializeField] private float speed = 5f;
		GameObject currentSnake;

		void Awake()
		{
			if (instance == null) 
			{
				instance = this;
			}
			else if (instance != this) 
			{
				Destroy (gameObject);    
			}
		}

		public void InitPlayer()
		{
			GameManager.instance.OnInitGame += SetupPlayer;
			GameManager.instance.OnDoAction += DoAction;
		}

		void SetupPlayer()
		{		
			isGoingLeft = true;
			currentSnake = Instantiate (snakePrefab, parent:transform);
		}

		public void DoAction()
		{
			DoMovements ();
			GetInput ();
		}

		public void DoMovements()
		{
			Vector3 v3Scale = transform.localScale;
			if (isGoingLeft) 
			{
				transform.localScale = new Vector3 (v3Scale.x + speed * Time.deltaTime, 1, 1);
			} 
			else 
			{
				transform.localScale = new Vector3 (1, 1, v3Scale.z + speed * Time.deltaTime);
			}
		}

		void GetInput()
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				SoundManager.instance.turnSource.Play ();

				Vector3 nextPos;

				if (!isGoingLeft) 
				{
					Vector3 childWorldPos = currentSnake.transform.GetChild (0).transform.localPosition;
					currentSnake.transform.GetChild (0).transform.localPosition = new Vector3 (childWorldPos.x * -1f, childWorldPos.y, (childWorldPos.z) * -1f);
				}
				nextPos = currentSnake.transform.GetChild (0).transform.position;

				currentSnake.transform.parent = null;
				currentSnake = null;

				if (isGoingLeft) 
				{
					isGoingLeft = false;
					currentSnake = Instantiate (snakePrefab, goingRightPosition, Quaternion.identity, transform);
					currentSnake.transform.localPosition = goingRightPosition;
				} 
				else
				{
					isGoingLeft = true;
					currentSnake = Instantiate (snakePrefab, goingLeftPosition, Quaternion.identity, transform);
					currentSnake.transform.localPosition = goingLeftPosition;
				}

				currentSnake.transform.rotation = new Quaternion (0, 0, 0, 0);

				transform.localPosition = nextPos;
				transform.localScale = Vector3.one;

			}
		}

		void OnTriggerEnter(Collider other) 
		{

			if (other.CompareTag (Consts.TAG_OBSTACLE)) 
			{
				if (OnGameOver != null)
					OnGameOver ();

				SoundManager.instance.explosionSource.Play ();
			} 
			else if (other.CompareTag (Consts.TAG_SCORE)) 
			{
				other.gameObject.SetActive (false);
				GameManager.instance.AddScore ();

				SoundManager.instance.coinSource.Play ();
			}
		}

		void OnDestroy()
		{
			GameManager.instance.OnInitGame -= SetupPlayer;
			GameManager.instance.OnDoAction -= DoAction;
		}


	}
}