using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanstalkScript : MonoBehaviour {
	public static BeanstalkScript instance;

	public GameObject pivotPointPrefab;
	private Rigidbody2D pivotBody;

	SpriteRenderer renderer;

	// Time variables
	public float waitTillGrowth = 3;
	public float growthSpeed = .001f;
	public float decayCountdown = 10;
	private float colorChangeRate;

	// Bean state variables
	private bool grown;
	private bool cut = false;
	private bool leftOfPlayer;

	void Start () {
		renderer = this.GetComponent<SpriteRenderer> ();
		colorChangeRate = .001f * (10 / decayCountdown);
		// Place the pivot gameobject on the top of the beanstalk
		GameObject pivotPoint = Instantiate (pivotPointPrefab);
		pivotPoint.transform.position = new Vector2 (transform.position.x,transform.position.y + .5f);
		pivotPoint.transform.parent = this.transform;
		pivotBody = pivotPoint.GetComponent<Rigidbody2D> ();
		grown = false;
		if (instance == null)
			instance = this;
	}

	// Allows player to climb only if the beanstalk is sufficiently large
	public bool FullyGrown(){
		return grown;
	}

	// Begins to tilt over the beanstalk if the player cuts it down
	public void CutBeanstalk(){
		if (transform.position.x < PlayerController.instance.transform.position.x)
			leftOfPlayer = true;
		else
			leftOfPlayer = false;
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
			this.transform.GetComponent<Rigidbody2D>().gravityScale = 1f;
			if(leftOfPlayer)
				transform.Rotate(0,0,.1f);
			else if(!leftOfPlayer)
				transform.Rotate(0,0,-.1f);
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
