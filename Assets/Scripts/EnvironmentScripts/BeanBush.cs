using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBush : MonoBehaviour {

	public float beanRate = 2f;
	private float progress = 0f;

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
				transform.GetChild (beanCount).gameObject.GetComponent<Bean> ().Grow ();
				beanCount++;
				progress = 0f;
			}
		}
	}

	public void PickBean() {
		beanCount = 0;
		progress = 0f;
	}
}
