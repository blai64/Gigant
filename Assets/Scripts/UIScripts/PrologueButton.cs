using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueButton : MonoBehaviour {
	public Button yourBtn;


	void Start(){
		Button btn = yourBtn.GetComponent<Button> ();
		btn.onClick.AddListener (update);
	}

	void update(){
		if (AutoFlip.instance.ControledBook.currentPage >= AutoFlip.instance.ControledBook.TotalPageCount) {
			SceneManager.LoadScene ("Tutorial");
		} else {
			AutoFlip.instance.FlipRightPage();
		}
	}
}