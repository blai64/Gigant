using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	public static MainCamera instance;
	public Transform player;
	private Camera mainCamera;

	public Vector2 margin = new Vector2(1, 1); // If the player stays inside this margin, the camera won't move.
	public Vector2 smoothing = new Vector2(1, 1); // The bigger the value, the faster is the camera.

	private float dampTime = 0.2f;
	public float xOffset = 0f;
	public float yOffset = 0f;

	public float DampTime {
		get { return dampTime; } 
		set { dampTime = value; } 
	}

	private Vector3 velocity = Vector3.zero;

	public BoxCollider2D cameraBounds;

	private Vector3 min, max;

	[HideInInspector] public bool isFollowing;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (this);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
	}

	void Start()
	{
		min = cameraBounds.bounds.min;
		max = cameraBounds.bounds.max;
		isFollowing = true;
		mainCamera = GetComponent<Camera>();

	}

	void Update()
	{
		if (isFollowing)
		{
			Vector3 point = mainCamera.WorldToViewportPoint(player.position);
			Vector3 destination;
			float oldZ = transform.position.z;

			//float direction = (float) pb.GetDirection ();
			Vector3 delta = player.position - mainCamera.ViewportToWorldPoint(new Vector3(0.5f + xOffset,
				0.5f + yOffset,
				0f));
			destination = transform.position + delta;
			destination.z = oldZ;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

		float x = transform.position.x;
		float y = transform.position.y;
			

		// ortographicSize is the haldf of the height of the Camera.
		var cameraHalfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height);

		x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);

		transform.position = new Vector3(x, y, transform.position.z);


	}

	// PixelPerfectScript.
	public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return adjustedUnityUnits;
	}
	/*
	void LateUpdate()
	{
		Vector3 newPos = transform.position;
		Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, mainCamera), RoundToNearestPixel(newPos.y, mainCamera), newPos.z);
		transform.position = roundPos;
	}
	*/

	public void UpdateBounds(BoxCollider2D newBounds)
	{
		cameraBounds = newBounds;
		min = cameraBounds.bounds.min;
		max = cameraBounds.bounds.max;
	}
}
