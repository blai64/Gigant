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
	public GameObject cowSpeechBubble;
	private SpeechBubble cowSb;

	public GameObject player;
	private Animator anim;
	private List<string> tutorialText1 = new List<string> ();
	private List<string> tutorialText2 = new List<string> ();
	private List<string> tutorialText3 = new List<string> ();

	private List<string> level1Text1 = new List<string> ();
	private List<string> level1Text2 = new List<string> ();
	private List<string> level1Text3 = new List<string> ();
	private List<string> level1Text4 = new List<string> ();
	private List<string> level1Text5 = new List<string> ();
	private List<string> level1TextMoo = new List<string> ();


	private List<string> level2Text1 = new List<string> ();
	private List<string> level2Text2 = new List<string> ();
	private List<string> level2Text3 = new List<string> ();

	private List<string> level3Text1 = new List<string> ();
	private List<string> level3Text2 = new List<string> ();
	private List<string> level3Text3 = new List<string> ();
	private List<string> level3Text4 = new List<string> ();
	private List<string> level3Text5 = new List<string> ();
	private List<string> level3TextMoo = new List<string> ();

	private List<string> bossText1 = new List<string> ();
	private List<string> bossText2 = new List<string> ();
	private List<string> bossText3 = new List<string> ();
	private List<string> bossText4 = new List<string> ();
	private List<string> bossText5 = new List<string> ();

	private List<string> enemyRespawnText1 = new List<string> ();
	private List<string> fallRespawnText1 = new List<string> ();
	private List<string> boulderRespawnText1 = new List<string> ();

	//cutscene boss
	public GameObject boss;
	public GameObject bossSpeechBubble;
	private SpeechBubble bossSb;

	// Animator objects
	private Animator cowAnim;
	private Animator hermitAnim;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}

	void Start(){

		cowAnim = GameObject.Find("Cow").GetComponent<Animator>();
		hermitAnim = GameObject.Find("Hermit Char").GetComponent<Animator>();

		anim = player.GetComponentInChildren<Animator> ();
		hermitSb = (hermitSpeechBubble != null) ? hermitSpeechBubble.GetComponent<SpeechBubble> () : null;
		playerSb = (playerSpeechBubble != null) ? playerSpeechBubble.GetComponent<SpeechBubble> () : null;
		cowSb = (cowSpeechBubble != null) ? cowSpeechBubble.GetComponent<SpeechBubble> () : null;
		bossSb = (bossSpeechBubble != null) ? bossSpeechBubble.GetComponent<SpeechBubble> () : null;

		//cutscene1
		tutorialText1.Add ("Why, if it isn't <color=#B1883F>Jack</color>!\nMy have you grown.");
		tutorialText1.Add ("I remember the first time I gave you some <color=#5FC96A>magic</color> <color=#5FC96A>beans</color>\nfor this here cow of yers.");
		tutorialText2.Add ("Moooo");
		tutorialText3.Add ("I hear you've come to\n<color=#AC4744>slay</color> <color=#AC4744>the</color> <color=#AC4744>giant</color>!\nYou'll need to climb to the top and reach his head.");

		level1Text1.Add ("Why if it isn't <color=#B1883F>Jack!</color>\nYou're a slow fella aren't ya?");
		level1Text2.Add ("How did you even get here?");
		level1Text3.Add ("I just rode <color=#FF94D1>ol' Bessie</color> here ");
		level1Text4.Add ("So you're saying my <color=#FF94D1>cow</color> got you here faster than the <color=#5FC96A>beans</color> you gave me before?");
		level1TextMoo.Add ("Moo...");
		level1Text5.Add ("Ehhh.... enough chit chat. Get to the top of this giant already!\nI thought you were a famous <color=#AC4744>giant</color> <color=#AC4744>slayer</color> or somethin'");

		level2Text1.Add ("Why if it isn't <color=#B1883F>Jack</color> again!");
		level2Text2.Add ("......");
		level2Text3.Add ("Mind if I check yer\n<color=#5FC96A>bean</color> <color=#5FC96A>collection</color>?");
		level2Text3.Add ("......");
		level2Text3.Add ("Whoops. I've dropped all your <color=#5FC96A>beans</color> off the mountain.\nDon't worry, I think there's a <color=#5FC96A>bean</color> <color=#5FC96A>bushes</color> somewhere around here.");

		level3Text1.Add ("Why if it isn't <color=#B1883F>Jack</color>!\nHow's it going bud?");
		level3Text2.Add ("......\n......");
		level3Text2.Add ("Why don't you just <color=#AC4744>slay</color> <color=#AC4744>the</color> <color=#AC4744>giant</color>?");
		level3Text3.Add ("Aww I would but I'm a <color=#FF94D1>lover</color> not a <color=#AC4744>killer</color>.\nAin't that right <color=#FF94D1>Bessie</color>?");
		level3TextMoo.Add ("Moooooo.");
		level3Text4.Add ("......");
		level3Text5.Add ("Anywhoo, there are a couple of <color=grey>guardians</color> up ahead that you'll neeed to <color=#AC4744>defeat</color> to unlock the way to the top.\nNow get goin'!");

		bossText1.Add ("At last, the time has come to <color=#AC4744>slay</color> <color=#AC4744>the</color> <color=#AC4744>giant</color>.");
		bossText2.Add ("Prepare yourself <color=#AC4744>monster</color>!!!");
		bossText2.Add ("In the name of the people,\nI will give you your <color=#AC4744>permanent</color> <color=#AC4744>slumber</color>!");
		bossText3.Add ("**Yawn**");
		bossText3.Add ("That was a good walk. Burned a whole 500 calories - my New Millenium's Resolution is coming along nicely.");
		bossText3.Add ("But what's that buzzing sound I hear?");
		bossText4.Add ("<color=#AC4744>RAHHHHHHH!!!!!</color>");
		bossText5.Add ("Ewww I've always hated bugs... Well that was enough exercise for the century. Time for another nap!");

		//############################# RESPAWNING DIALOGUE
		enemyRespawnText1.Add ("They’re trying to kill ya, not hug ya");

		fallRespawnText1.Add ("Good thinking. Maybe if you fall enough, gravity will magically change directions and bring ya to the top of the giant.");

		boulderRespawnText1.Add ("Ya know why them rocks fall?");
		boulderRespawnText1.Add ("It’s the giant crying from yer stupidity");
	}

	public IEnumerator MoveOn() {
		moveOn = true;
		yield return 0;
	}

	public IEnumerator Wait() {
		while (!moveOn) {
			yield return 0;
		}
		moveOn = false;
		yield return 0;
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player") && !initialized) {
			initialized = true; 

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
			StartCoroutine (StartCutsceneRespawn ());
			
		}
	}
		
	void SetActiveBubble(GameObject newSpeechBubble, SpeechBubble newSb) {

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

	private void Talking(bool isTalking) {
		hermitAnim.SetBool("isTalking", isTalking);
	}

	private void Moo() {
		SoundManager.instance.PlaySound ("moo");
		cowAnim.SetTrigger ("Moo");
	}

	IEnumerator StartCutsceneTutorial() {

		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(tutorialText1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
		SetActiveBubble (cowSpeechBubble, cowSb);
		Moo ();
		activeSb.Play(tutorialText2);

		yield return StartCoroutine (Wait ()); 

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(tutorialText3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
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

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level1Text2);

		yield return StartCoroutine (Wait ()); 

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking
		//yield return 0;
		Talking(false);
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level1Text4);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (cowSpeechBubble, cowSb);
		Moo ();
		activeSb.Play(level1TextMoo);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level1Text5);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
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

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level2Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level2Text2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level2Text3);

		PlayerController.instance.beanCount = 0;

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
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

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text1);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level3Text2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
		SetActiveBubble (cowSpeechBubble, cowSb);
		Moo ();
		activeSb.Play(level3TextMoo);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(level3Text4);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(true);
		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(level3Text5);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		Talking(false);
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

	public IEnumerator StartCutsceneBossP2() {
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


	IEnumerator StartCutsceneRespawn() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);

		switch (causeOfDeath) {
		case "fall":
			activeSb.Play(fallRespawnText1);
			break;
		case "enemy":
			activeSb.Play(enemyRespawnText1);
			break;
		case "boulder":
			activeSb.Play(boulderRespawnText1);
			break;
		default:
			activeSb.Play(fallRespawnText1);
			break; 
		}

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		StartCoroutine (EndCutsceneTutorial());
	}
}
