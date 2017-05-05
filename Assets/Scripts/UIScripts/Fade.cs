using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour {

	public static Fade instance;

	public GameObject white;
	SpriteRenderer renderer;
	private Color rendererAlpha;

	public bool isFading;

	void Start() {
		isFading = false;
		rendererAlpha = white.GetComponent<SpriteRenderer> ().color;
		rendererAlpha.a = 0.0f;
		white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
	}

	void Awake(){
		if (instance == null)
			instance = this;
	}


	void Update() {

		if (isFading) {
			rendererAlpha.a += 0.025f;
			white.GetComponent<SpriteRenderer> ().color = rendererAlpha;
		}

		if (rendererAlpha.a >= 2.0f) {
			SceneManager.LoadScene ("Epilogue");
		}
	}
}