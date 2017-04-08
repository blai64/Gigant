using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBush : MonoBehaviour {
	private float beanRate = 2f;
	private float progress;

	private int maxBeans; 

	[HideInInspector] int beanCount;
	// Use this for initialization
	void Start () {
		maxBeans = transform.childCount;
		progress = 0f;
		beanCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (beanCount < maxBeans)
			progress += Time.deltaTime;

		if (progress >= beanRate) {
			transform.GetChild (beanCount).gameObject.SetActive (true);
			transform.GetChild (beanCount).gameObject.GetComponent<Bean>().Grow();
			beanCount++;
			progress = 0f;

		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")) {
			PlayerController.instance.beanCount = Mathf.Min (PlayerController.instance.beanCount + beanCount,
				PlayerController.instance.maxBeans);
			Debug.Log (PlayerController.instance.beanCount);
			beanCount = 0; 
			for (int i = 0; i < maxBeans; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}
}
