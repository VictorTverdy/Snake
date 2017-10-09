using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

#if AADOTWEEN
using DG.Tweening;
#endif
 
using UnityEngine.SceneManagement;

namespace AppAdvisory.DancingSnake
{		
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private float timeBetweenObstacle = 2f;
		[SerializeField] private float gameOverDuration = 1f;
		[SerializeField] private float introDuration = 3f;

		[SerializeField] private Text txtScore;

		[SerializeField] private List<GameObject> obstaclePrefabs;

		[SerializeField] private GameObject cameraObject;

		public delegate void InitGame();
		public delegate void StartGame();
		public delegate void DoActionEvent();

		public event InitGame OnInitGame;
		public event DoActionEvent OnDoAction;

		private Action DoAction;

		public static GameManager instance = null;

		private int currentScore = 0;

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

			Application.targetFrameRate = 60;
			Time.fixedDeltaTime = 1f / 60f;
			Time.maximumDeltaTime = 5f / 60f;
		}

		void Start()
		{
			SetModeIdle ();

			currentScore = 0;

			Character.instance.InitPlayer ();

			if (OnInitGame != null)
				OnInitGame ();
			
			Character.instance.OnGameOver += OnGameOver;

			SetModeNormal ();
			StartCoroutine (WaitForIntro ());
		}

		void ManageSpawn()
		{
			int randomSpawn = UnityEngine.Random.Range(0, obstaclePrefabs.Count);
			Vector3 posToInstantiate = new Vector3 (cameraObject.transform.position.x + 20, 0f, UnityEngine.Random.Range(-3f, 2f));

			Instantiate (obstaclePrefabs [randomSpawn], posToInstantiate, Quaternion.identity);
		}

		void Update()
		{
			DoAction ();
		}

		void DoActionIdle()
		{

		}

		void DoActionNormal()
		{
			if (OnDoAction != null)
				OnDoAction ();
		}

		void DoActionSpawn()
		{
			if (OnDoAction != null)
				OnDoAction ();
			
			StartCoroutine(DoSpawn ());

			SetModeNormal ();
		}

		void SetModeNormal()
		{
			DoAction = DoActionNormal;
		}

		void SetModeSpawn()
		{
			DoAction = DoActionSpawn;
		}

		void SetModeIdle()
		{
			DoAction = DoActionIdle;
		}

		public void AddScore()
		{
			currentScore++;

			if (PlayerPrefs.GetInt (Consts.PLAYERPREFS_HIGHSCORE) < currentScore)
				PlayerPrefs.SetInt (Consts.PLAYERPREFS_HIGHSCORE, currentScore);

			txtScore.text = currentScore + "";
		}

		void OnGameOver()
		{

			print ("Gameover");
			StopAllCoroutines();
			SetModeIdle();

			#if AADOTWEEN
			DOTween.KillAll();
			#endif

			ShowAds ();
			ReportScoreToLeaderboard (PlayerPrefs.GetInt (Consts.PLAYERPREFS_LASTSCORE, 0));

			PlayerPrefs.SetInt (Consts.PLAYERPREFS_LASTSCORE, currentScore);

			StartCoroutine (GameOverCoroutine ());
		}

		private static readonly string VerySimpleAdsURL = "http://u3d.as/oWD";

		private static readonly string VerySimpleRateURL = "http://u3d.as/Dt2";

		public int numberOfPlayToShowInterstitial = 5;

		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
			count++;

			#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
			GameAnalytics.NewDesignEvent ("SHOW ADS");

			if(AppAdvisory.Ads.AdsManager.instance.IsReadyInterstitial())
			{
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
			AppAdvisory.Ads.AdsManager.instance.ShowInterstitial();
			}
			}
			else
			{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#else
			if(count >= numberOfPlayToShowInterstitial)
			{
				Debug.LogWarning("To show ads, please have a look at Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
				PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
			}
			else
			{
				PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#endif
		}

		void ReportScoreToLeaderboard(int p)
		{
			#if APPADVISORY_LEADERBOARD
			LeaderboardManager.ReportScore(p);
			#else
			if (PlayerPrefs.GetInt("VERY_SIMPLE_LEADERBOARD_COUNT", 0) > 32)
			{
				print("Get very simple Leaderboard to use it : " + VerySimpleRateURL);
				PlayerPrefs.SetInt("VERY_SIMPLE_LEADERBOARD_COUNT", 0);
				PlayerPrefs.Save();
			}
			#endif
		}

		IEnumerator WaitForIntro()
		{
			yield return new WaitForSeconds(introDuration);

			cameraObject.GetComponent<Camera> ().InitCamera ();
			SetModeSpawn ();
		}

		IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(gameOverDuration);

			SceneManager.LoadScene(Consts.SCENE_GAMEOVER);
		}

		IEnumerator DoSpawn()
		{
			ManageSpawn ();

			yield return new WaitForSeconds(timeBetweenObstacle);

			SetModeSpawn ();
		}
	}
}