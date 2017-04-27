using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

	private bool initialized;

	public GameObject speechBubble;
	private SpeechBubble sb;
	public GameObject player;
	private Animator anim;

	void Start(){
		anim = player.GetComponentInChildren<Animator> ();
		sb = speechBubble.GetComponent<SpeechBubble> ();
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !initialized) {
			initialized = true; 
			StartCoroutine (StartCutscene ());
		}
	}

	IEnumerator StartCutscene() {
		PlayerController.instance.Disable (true);

		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		yield return new WaitForSeconds (0.3f);

		speechBubble.SetActive (true);
		sb.Play();
		yield return 0;
	}

	public IEnumerator EndCutscene() {
		
		speechBubble.SetActive (false);

		StartCoroutine (CameraManager.instance.Zoom (false));
		StartCoroutine (CameraManager.instance.MoveCinematic (false));
		Debug.Log ("???");
		PlayerController.instance.Enable (true);
		Debug.Log ("???");
		yield return 0;
	}
}
