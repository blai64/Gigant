using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlipper : MonoBehaviour {

	public GameObject swordHitbox;

	public void Flip(){
		swordHitbox.transform.Rotate (0f, 0f, 180f);
	}
}
