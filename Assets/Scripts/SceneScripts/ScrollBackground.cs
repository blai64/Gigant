using UnityEngine;
using System.Collections;

public class ScrollBackground : MonoBehaviour {

	Material mat;
	public Vector2 phase = Vector2.zero;
	public Vector2 Speed = Vector2.one;

	void Start () {
		mat = GetComponent< Renderer >().material;
		phase = Vector2.zero;
	}

	void Update () {
		phase += Speed * Time.deltaTime;
		phase.x -= Mathf.Floor(phase.x);
		phase.y -= Mathf.Floor(phase.y);
		mat.SetTextureOffset("_MainTex", phase);
	}
}
