using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour {

	public GameObject dustParticleSpawner;

	void OnTriggerEnter2D (Collider2D col){
		
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			GameObject ps = Instantiate (dustParticleSpawner);
			ps.transform.position = transform.position;
			/*
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams ();

			emitParams.position = transform.position;
			dustParticleSpawner.GetComponent<ParticleSystem> ().Emit (emitParams,2);
			//dustParticleSpawner.GetComponent<ParticleSystem> ().Play ();*/
		}

	}
}
