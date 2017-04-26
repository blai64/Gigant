using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBush : MonoBehaviour {
	
	public float beanRate = 2f;
	private float progress;

	private int maxBeans; 

	private Object beanLock;

	[HideInInspector] int beanCount;

	void Start () {
		beanLock = new Object ();
		maxBeans = transform.childCount;
		progress = 0f;
		beanCount = 0;
	}

	void Update () {
		if (beanCount < maxBeans)
			progress += Time.deltaTime;

		if (progress >= beanRate) {
			lock (beanLock) {
				transform.GetChild (beanCount).gameObject.SetActive (true);
				//transform.GetChild (beanCount).gameObject.GetComponent<Bean>().Grow();
				beanCount++;
				progress = 0f;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")) {
			lock (beanLock) {
				PlayerController.instance.beanCount = Mathf.Min (PlayerController.instance.beanCount + beanCount,
					PlayerController.instance.maxBeans);
				beanCount = 0; 
				progress = 0;
				for (int i = 0; i < maxBeans; i++) {
					transform.GetChild (i).gameObject.SetActive (false);
				}
			}

		}
	}
}
