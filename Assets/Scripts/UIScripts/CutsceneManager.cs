using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour {
	public static CutsceneManager instance;

	[HideInInspector] public bool initialized;
	[HideInInspector] public bool playerRespawning;
	[HideInInspector] public string causeOfDeath;

	private GameObject activeSpeechBubble;
	private SpeechBubble activeSb;

	private bool moveOn;

	//cutscene 1
	public GameObject hermitSpeechBubble;
	private SpeechBubble hermitSb;
	public GameObject playerSpeechBubble;
	private SpeechBubble playerSb;

	public GameObject player;
	private Animator anim;
	private List<string> tutorialText = new List<string> ();
	private List<string> tutorialText2 = new List<string> ();
	private List<string> tutorialText3 = new List<string> ();

	private List<string> level1Text1 = new List<string> ();
	private List<string> level1Text2 = new List<string> ();
	private List<string> level1Text3 = new List<string> ();
	private List<string> level1Text4 = new List<string> ();
	private List<string> level1Text5 = new List<string> ();

	private List<string> level2Text1 = new List<string> ();
	private List<string> level2Text2 = new List<string> ();
	private List<string> level2Text3 = new List<string> ();

	private List<string> level3Text1 = new List<string> ();
	private List<string> level3Text2 = new List<string> ();
	private List<string> level3Text3 = new List<string> ();
	private List<string> level3Text4 = new List<string> ();
	private List<string> level3Text5 = new List<string> ();

	private List<string> bossText1 = new List<string> ();
	private List<string> bossText2 = new List<string> ();
	private List<string> bossText3 = new List<string> ();
	private List<string> bossText4 = new List<string> ();
	private List<string> bossText5 = new List<string> ();


	private List<string> fallRespawnText1 = new List<string> ();


	//cutscene boss
	public GameObject boss;
	public GameObject bossSpeechBubble;
	private SpeechBubble bossSb;

	void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}

	void Start(){
		anim = player.GetComponentInChildren<Animator> ();
		hermitSb = (hermitSpeechBubble != null) ? hermitSpeechBubble.GetComponent<SpeechBubble> () : null;
		playerSb = (playerSpeechBubble != null) ? playerSpeechBubble.GetComponent<SpeechBubble> () : null;
		bossSb = (bossSpeechBubble != null) ? bossSpeechBubble.GetComponent<SpeechBubble> () : null;

		//cutscene1
		tutorialText.Add ("Why, if it isn't <color=brown>Jack</color>!\nMy have you grown.");
		tutorialText.Add ("I remember the first time I gave you some <color=green>magic beans</color>\nfor this here cow of yers.");
		tutorialText.Add ("I hear you've come to\n<color=red>slay the giant</color>!\nYou'll need to climb to the top and reach his head.");

		level1Text1.Add ("Why if it isn't Jack!\nYou're a slow fella aren't ya?");
		level1Text2.Add ("How did you even get here?");
		level1Text3.Add ("I just rode ol' Bessie here");
		level1Text4.Add ("So you're saying my cow got you here faster than the beans you gave me before?");
		level1Text5.Add ("Ehhh.... enough chit chat. Get to the top of this giant already!\nI thought you were a famous giant slayer or somethin'");

		level2Text1.Add ("Why if it isn't Jack again!");
		level2Text2.Add ("......");
		level2Text3.Add ("What you said earlier got me thinkin'.\nCan I check your magic beans?");
		level2Text3.Add ("......");
		level2Text3.Add ("Whoops. I've dropped all our beans off the moutain.\nDon't worry, I think there's a bean bush somewhere around here.");

		level3Text1.Add ("Why if it isn't Jack!\nHow's it going bud?");
		level3Text2.Add ("......\n......");
		level3Text2.Add ("Why don't you just slay the giant?");
		level3Text3.Add ("Aww I would but I'm a lover not a killer.\nAin't that right Bessie~?");
		level3Text4.Add ("......");
		level3Text5.Add ("Anywhoo, I hear there are a couple of guardians up ahead that you'll neeed to defeat to unlock the way to the top. Now get goin'!");

		bossText1.Add ("At last, the time has come to slay the giant.");
		bossText2.Add ("Prepare yourself monster!!!");
		bossText2.Add ("In the name of the people ,I will give you your permanent slumber!");
		bossText3.Add ("**Yawn**");
		bossText3.Add ("That was a good walk. Burned a whole 500 calories - my New Millenium's Resolution is coming along nicely.");
		bossText3.Add ("But what's that buzzing sound I hear?");
		bossText4.Add ("RAHHHHHHH!!!!!");
		bossText5.Add ("Ewww I've always hated bugs... Well that was enough eercise for the century. Time for another nap!");


		//############################# RESPAWNING DIALOGUE

		fallRespawnText1.Add ("Jump better, scrubb."); 

	}

	public IEnumerator MoveOn(){
		moveOn = true;
		yield return 0;
	}

	public IEnumerator Wait(){
		while (!moveOn) {
			yield return 0;
		}
		moveOn = false;
		yield return 0;
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag ("Player") && !initialized) {
			initialized = true; 
			//TODO: case on which scene this is, start the correct cutscene
			switch (SceneManager.GetActiveScene ().name) {
			case "Level1":
				StartCoroutine (StartCutsceneLevel1 ());
				break;
			case "Level2":
				StartCoroutine (StartCutsceneLevel2 ());
				break;
			case "Level3":
				StartCoroutine (StartCutsceneLevel3 ());
				break;
			case "Zao_BossLevel":
			case "Blai_Beta":
			case "BossLevel":
				StartCoroutine (StartCutsceneBoss ());
				break;
			default:
				StartCoroutine (StartCutsceneTutorial ());
				break;
			}
		} else if (col.CompareTag ("Player") && playerRespawning) {
			playerRespawning = false;
			switch (causeOfDeath) {
			case "fall":
				StartCoroutine (StartCutsceneFallRespawn ());
				break;
			case "enemy":
				break;
			case "boulder":
				break;
			default:
				break;
			}

		}
	}
		
	void SetActiveBubble(GameObject newSpeechBubble, SpeechBubble newSb){
		if (activeSpeechBubble != null) {
			activeSpeechBubble.SetActive (false);
			activeSb = null;
		}


		if (newSpeechBubble != null) {
			newSpeechBubble.SetActive (true);
			activeSb = newSb;
		}
			

		activeSpeechBubble = newSpeechBubble;
	}


	IEnumerator StartCutsceneTutorial() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(tutorialText);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneTutorial());
	}

	public IEnumerator EndCutsceneTutorial() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));

		PlayerController.instance.Enable (true);

		yield return 0;
	}

	//###################################################################################

	IEnumerator StartCutsceneLevel1() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level1Text2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking
		//yield return 0;
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level1Text4);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text5);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneLevel1());
	}

	public IEnumerator EndCutsceneLevel1() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));

		PlayerController.instance.Enable (true);

		yield return 0;
	}

	//###################################################################################

	IEnumerator StartCutsceneLevel2() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level2Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level2Text2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level2Text3);

		PlayerController.instance.beanCount = 0;

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneLevel1());
	}

	public IEnumerator EndCutsceneLevel2() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));

		PlayerController.instance.Enable (true);

		yield return 0;
	}


	//###################################################################################

	IEnumerator StartCutsceneLevel3() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level3Text2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking
		//yield return 0;
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level3Text4);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text5);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneLevel1());
	}

	public IEnumerator EndCutsceneLevel3() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));

		PlayerController.instance.Enable (true);

		yield return 0;
	}



	//###################################################################################

	IEnumerator StartCutsceneBoss() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(bossText1);

		yield return StartCoroutine (Wait ());

		activeSb.Play(bossText2);

		yield return StartCoroutine (Wait ());

		StartCoroutine (EndCutsceneBoss());
	}

	public IEnumerator EndCutsceneBoss() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));

		PlayerController.instance.Enable (true);

		yield return 0;
	}

	//###################################################################################

	public IEnumerator StartCutsceneBossP2(){
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		MainCamera.instance.player = boss.transform;

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (bossSpeechBubble, bossSb);
		activeSb.Play(bossText3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneBossP2());
	

	}

	public IEnumerator EndCutsceneBossP2() {
		MainCamera.instance.player = player.transform;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));


		PlayerController.instance.Enable (true);



		StartCoroutine (StartCutsceneBossP3 ());
	}

	//###################################################################################

	IEnumerator StartCutsceneBossP3() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(bossText4);

		yield return StartCoroutine (Wait ());

		StartCoroutine (EndCutsceneBoss());
	}

	//###################################################################################

	public IEnumerator StartCutsceneBossP4(){
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true,true, -0.05f));

		MainCamera.instance.player = boss.transform;

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (bossSpeechBubble, bossSb);
		activeSb.Play(bossText5);

		yield return StartCoroutine (Wait ());

		StartCoroutine (EndCutsceneBossP4());


	}

	public IEnumerator EndCutsceneBossP4() {
		MainCamera.instance.player = player.transform;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false,true, 0.05f));


		PlayerController.instance.Enable (true);



		yield return 0;
	}


	IEnumerator StartCutsceneFallRespawn() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(fallRespawnText1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneTutorial());
	}
}
