using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeanCount : MonoBehaviour {

	public GameObject beanIndicator;
	private Text txt;
	private int beanCnt;
	// Use this for initialization
	void Start () {
		txt = beanIndicator.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		beanCnt = PlayerController.instance.beanCount;
		switch (beanCnt) {
		case 0:
			txt.text = "x0";
			break;
		case 1:
			txt.text = "x1";
			break;
		case 2:
			txt.text = "x2";
			break;
		case 3:
			txt.text = "x3";
			break;
		case 4:
			txt.text = "x4";
			break;
		case 5:
			txt.text = "x5";
			break;
		
		}
	}
}
