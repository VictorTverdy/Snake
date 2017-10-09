using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace AppAdvisory.DancingSnake
{
	public class Scores : MonoBehaviour 
	{
		[SerializeField] private Text lastScoreTxt;
		[SerializeField] private Text highScoreTxt;

		void Start () 
		{
			lastScoreTxt.text = "SCORE : " + PlayerPrefs.GetInt (Consts.PLAYERPREFS_LASTSCORE);
			highScoreTxt.text = "BEST : " + PlayerPrefs.GetInt (Consts.PLAYERPREFS_HIGHSCORE);
		}

		public void OnButtonPlay()
		{
			SceneManager.LoadScene (Consts.SCENE_GAME);
		}
	}
}
