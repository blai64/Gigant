using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanstalkScript : MonoBehaviour {
	public static BeanstalkScript instance;
	public BoxCollider2D swordCollider;
	public GameObject pivotPoint;

	SpriteRenderer renderer;

	// Time variables
	public float waitTillGrowth = 3;
	public float growthSpeed = .001f;
	public float decayCountdown = 10;
	private float colorChangeRate;

	// Color variables
	float scale;

	private bool grown;
	private bool cut = false;

	void Start () {
		renderer = this.GetComponent<SpriteRenderer> ();
		colorChangeRate = .001f * (10 / decayCountdown);
		grown = false;
		if (instance == null)
			instance = this;
	}

	// Allows player to climb only if the beanstalk is sufficiently large
	public bool ReadyToClimb(){
		return grown;
	}

	// Begins to tilt over the beanstalk if the player cuts it down
	public void cutBeanstalk(){
		cut = true;
	}

	void Update () {
		// Time period between planting seed and growth start
		if (waitTillGrowth > 0) {
			waitTillGrowth -= Time.deltaTime;
		} 
		// Growing the beanstalk
		else{
			if (!grown) {
				transform.localScale += new Vector3 (growthSpeed, growthSpeed, 0);
				transform.Translate (new Vector2 (0, growthSpeed * 10f));
			}
			if (transform.localScale.y > .5f)
				grown = true;
		}

		// Decay beanstalk after a while
		if (grown) {
			Decay ();
		}

		// Makes beanstalk fall when it's cut
		if (cut) {
			pivotPoint.GetComponent<Rigidbody2D> ().AddForce (new Vector2(100, 0));
			//transform.RotateAround (new Vector3(transform.position.x,transform.position.y - 5 ,0) , new Vector3 (0, 0, -1), 1);
			//transform.Translate (new Vector3 (.1f, 0, 0));
		}
	}

	// Makes the beanstalk decay over time
	void Decay(){
		decayCountdown -= Time.deltaTime;
		renderer.color -= new Color(colorChangeRate / 2,colorChangeRate,colorChangeRate,0);
		if (decayCountdown < 0) {
			Destroy (this.gameObject);
			PlayerController.instance.Climb (false);
		}
	}
}
