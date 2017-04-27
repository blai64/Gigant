using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

	private bool initialized;

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


	//cutscene boss
	public GameObject bossSpeechBubble;
	private SpeechBubble bossSb;

	void Start(){
		anim = player.GetComponentInChildren<Animator> ();
		hermitSb = hermitSpeechBubble.GetComponent<SpeechBubble> ();
		playerSb = playerSpeechBubble.GetComponent<SpeechBubble> ();
		//bossSb = bossSpeechBubble.GetComponent<SpeechBubble> ();

		//cutscene1
		tutorialText.Add ("Why, if it isn't <color=brown>Jack</color>!\nMy have you grown.");
		tutorialText2.Add ("I remember the first time I gave you some <color=green>magic beans</color>\nfor this here cow of yers.");
		tutorialText3.Add ("I hear you've come to <color=red>slay the giant</color>!\nYou'll need to climb to the top and reach his head.");
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !initialized) {
			initialized = true; 
			//TODO: case on which scene this is, start the correct cutscene
			StartCoroutine (StartCutscene1 ());
		}
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

	public IEnumerator EndCutscene(){
		//TODO: case on which scene this is, end the correct cutscene
		StartCoroutine(EndCutscene1());
		yield return 0;
	}


	IEnumerator StartCutscene1() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(tutorialText);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (playerSpeechBubble, playerSb);
		activeSb.Play(tutorialText2);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking

		SetActiveBubble (hermitSpeechBubble, hermitSb);
		activeSb.Play(tutorialText3);

		yield return StartCoroutine (Wait ()); // wait for person to be done with hermit speaking
		//yield return 0;

		StartCoroutine (EndCutscene1());
	}

	public IEnumerator EndCutscene1() {

		SetActiveBubble (null,null);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));
		Debug.Log ("???");
		PlayerController.instance.Enable (true);
		Debug.Log ("???");
		yield return 0;
	}



	public IEnumerator StartCutsceneBossP1(){
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		bossSpeechBubble.SetActive (true);
		bossSb.Play(tutorialText);
		yield return 0;
	
	}

	public IEnumerator EndCutsceneBossP1() {

		bossSpeechBubble.SetActive (false);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));
		Debug.Log ("???");
		PlayerController.instance.Enable (true);
		Debug.Log ("???");
		yield return 0;
	}
}
