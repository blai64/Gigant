using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	Text textBox;

	private string text = ".........................";

	void Start(){
		textBox = transform.Find ("Panel/Text").GetComponent<Text> ();
		Debug.Log (textBox);
	}

	public IEnumerator AnimateText(){
		for (int i = 0; i < text.Length; i++) {
			textBox.text = text.Substring (0, i);
			yield return new WaitForSeconds (0.1f);
		}
	}
}

