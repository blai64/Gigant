using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static CameraManager instance;


	private Camera cam;

	private float cinematicStartDuration = 1.25f;

	private float blackBarInactiveOffset;
	private float blackBarActiveOffset = 7.0f;

	private bool myLock;
	private bool isActive;


	public GameObject TopBlack;
	public GameObject BottomBlack;


	void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}

	void Start () {
		cam = GetComponent<Camera> ();
		Debug.Log (cam.orthographicSize); 
		blackBarInactiveOffset = TopBlack.transform.localPosition.y;
		myLock = true;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.Z) && myLock && !isActive) {
			myLock = false;
			StartCoroutine (Zoom (true));
			StartCoroutine (MoveCinematic (true));
		} else if (Input.GetKeyDown (KeyCode.X) && myLock && isActive) {
			myLock = false;
			StartCoroutine (Zoom (false));
			StartCoroutine (MoveCinematic (false));
		}
*/
	}

	public IEnumerator Zoom(bool zoomIn){
		float t = 0f;
		float curCameraSize = cam.orthographicSize;

		float newCameraSize = (zoomIn) ? 5.0f : 10.0f;

		while (t < 1.0) {
			t += Time.deltaTime / cinematicStartDuration;
			cam.orthographicSize = Mathf.Lerp (curCameraSize, newCameraSize, t);
			yield return 0;
		}
	
		//StartCoroutine (StartCinematic ());
	}

	public IEnumerator MoveCinematic(bool starting){
		float t = 0f;

		Vector3 oldTopPosition = TopBlack.transform.localPosition;
		Vector3 oldBottomPosition = BottomBlack.transform.localPosition;

		int dir = (starting) ? 1 : -1; 

		Vector3 newTopPosition = TopBlack.transform.localPosition + dir * (Vector3.down * blackBarActiveOffset);
		Vector3 newBottomPosition = BottomBlack.transform.localPosition + dir * (Vector3.up * blackBarActiveOffset);


		while (t < 1.0) {
			t += Time.deltaTime / cinematicStartDuration;

			TopBlack.transform.localPosition = Vector3.Lerp (oldTopPosition, newTopPosition, t);
			BottomBlack.transform.localPosition = Vector3.Lerp (oldBottomPosition, newBottomPosition, t);

			yield return 0;
		}

		isActive = starting;
		myLock = true;
	}


}
