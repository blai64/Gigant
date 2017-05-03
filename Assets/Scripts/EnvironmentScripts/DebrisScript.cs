using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour {

	public GameObject[] debrisZones;
 
	public GameObject dustParticleSpawner;

	void OnTriggerEnter2D (Collider2D col){
		if (CheckBounds ()) {
			if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
				GameObject ps = Instantiate (dustParticleSpawner);
				ps.transform.position = transform.position;
			}
		}


	}

	bool CheckBounds(){
		foreach (GameObject zone in debrisZones) {
			BoxCollider2D box = zone.GetComponent<BoxCollider2D> ();
			if (transform.position.x > box.bounds.center.x - box.bounds.extents.x &&
			    transform.position.x < box.bounds.center.x + box.bounds.extents.x &&
				transform.position.y > box.bounds.center.y - box.bounds.extents.y &&
			    transform.position.y < box.bounds.center.y + box.bounds.extents.y)
				return true;
		}
		return false;
	}
}
