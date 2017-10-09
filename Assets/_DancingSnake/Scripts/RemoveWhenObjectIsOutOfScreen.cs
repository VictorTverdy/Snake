﻿
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com
 * Facebook: https://facebook.com/appadvisory
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace AppAdvisory.DancingSnake
{
	public class RemoveWhenObjectIsOutOfScreen : MonoBehaviour 
	{
		private GameObject objectToWatch;
		[SerializeField] private float xOFFSET = 15f;

		void Start()
		{
			objectToWatch = Character.instance.gameObject;
		}

		void Update()
		{
			if (transform.position.x < objectToWatch.transform.position.x - (transform.lossyScale.x + transform.lossyScale.z + xOFFSET)) 
			{
				Destroy (gameObject);
			}
		}
	}
}