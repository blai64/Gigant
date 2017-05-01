using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeanCount : MonoBehaviour {

	public GameObject beanIndicator;
	private Text txt;
	private int beanCnt;
	private int maxBeans;

	void Start () {
		txt = beanIndicator.GetComponent<Text> ();
		maxBeans = PlayerController.instance.maxBeans;
	}

	void Update () {
		beanCnt = PlayerController.instance.beanCount;
		txt.text = beanCnt.ToString () + "/" + maxBeans.ToString ();
	}
}
