using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppAdvisory.DancingSnake
{
	public class SoundManager : MonoBehaviour 
	{
		public AudioSource explosionSource;
		public AudioSource turnSource;
		public AudioSource coinSource;

		public static SoundManager instance = null;

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
	}

}