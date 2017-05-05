using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySide : MonoBehaviour {

	public void EnemyPlay(GameObject Obj, string soundName) {
		if (Obj.GetComponent<SpriteRenderer>().isVisible) {
			SoundManager.instance.PlaySound (soundName);
		}
	}
}
