using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	
	public static CameraManager instance;

	private Camera cam;

	private float cinematicStartDuration = 0.75f;

	private float blackBarInactiveOffset;
	private float blackBarActiveOffset = 7.0f;


	private bool isActive;

	public GameObject TopBlack;
	public GameObject BottomBlack;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}

	void Start () {
		cam = GetComponent<Camera> ();
		blackBarInactiveOffset = TopBlack.transform.localPosition.y;
	}

	public IEnumerator Zoom(bool zoomIn) {
		float t = 0f;
		float curCameraSize = cam.orthographicSize;

		float newCameraSize = (zoomIn) ? curCameraSize - 2.5f : curCameraSize + 2.5f;

		while (t < 1.0) {
			t += Time.deltaTime / cinematicStartDuration;
			cam.orthographicSize = Mathf.Lerp (curCameraSize, newCameraSize, t);
			yield return 0;
		}
	}

	public IEnumerator MoveCinematic(bool starting, bool customOffset = false, float barOffset = 0f) {
		float t = 0f;

		float blackBarOffset = (customOffset) ? barOffset : blackBarActiveOffset;

		Vector3 oldTopPosition = TopBlack.transform.localPosition;
		Vector3 oldBottomPosition = BottomBlack.transform.localPosition;

		int dir = (starting) ? 1 : -1; 

		Vector3 newTopPosition = TopBlack.transform.localPosition + dir * (Vector3.down * blackBarOffset);
		Vector3 newBottomPosition = BottomBlack.transform.localPosition + dir * (Vector3.up * blackBarOffset);


		while (t < 1.0) {
			t += Time.deltaTime / cinematicStartDuration;

			TopBlack.transform.localPosition = Vector3.Lerp (oldTopPosition, newTopPosition, t);
			BottomBlack.transform.localPosition = Vector3.Lerp (oldBottomPosition, newBottomPosition, t);

			yield return 0;
		}

		isActive = starting;
	}
}
