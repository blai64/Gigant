using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanstalkScript : MonoBehaviour {
	//public static BeanstalkScript instance;

	public GameObject pivotPointPrefab;
	private Rigidbody2D pivotBody;

	SpriteRenderer sr;

	// Time variables
	public float waitTillGrowth = 3;
	public float growthSpeed = .001f;
	public float decayCountdown = 10f;
	private float decayTimer;
	private float colorChangeRate;

	// Bean state variables
	private bool grown;
	private bool cut = false;
	private int direction;
	private float fullyRotated = 2;
	public float growSize = .5f;

	private bool isPlaying = false;

	void Start () {
		sr = this.GetComponent<SpriteRenderer> ();
		colorChangeRate = .001f * (10 / decayCountdown);
		// Place the pivot gameobject on the top of the beanstalk
		GameObject pivotPoint = Instantiate (pivotPointPrefab);
		pivotPoint.transform.position = new Vector2 (transform.position.x,transform.position.y + .5f);
		pivotPoint.transform.parent = this.transform;
		pivotBody = pivotPoint.GetComponent<Rigidbody2D> ();
		grown = false;
		decayTimer = decayCountdown;

		SoundManager.instance.PlaySound ("stalk grow");
	}

	// Allows player to climb only if the beanstalk is sufficiently large
	public bool FullyGrown() {
		return grown;
	}

	public bool isCut() {
		return cut;
	}

	// Begins to tilt over the beanstalk if the player cuts it down
	public void PlayerCutBeanstalk(){
		if (transform.position.x < PlayerController.instance.transform.position.x && PlayerController.instance.isLeft)				// Alex) Made it so you have to be facing beanstalk
			direction = 1;																											//       to cut it 4/23
		else if(transform.position.x > PlayerController.instance.transform.position.x && !PlayerController.instance.isLeft)
			direction = -1;
		cut = true;
	}

	// Begins to tilt over the beanstalk if the enemy cuts it down
	public void EnemyCutBeanstalk(GameObject enemy){
	/*	if (transform.position.x < enemy.transform.position.x )			
			direction = 1;																											
		else if(transform.position.x > enemy.transform.position.x )
			direction = -1;
		cut = true;  */
		grown = true;

	}

	void Update () {
		// Time period between planting seed and growth start
		if (waitTillGrowth > 0) {
			waitTillGrowth -= Time.deltaTime;
		} 
		// Growing the beanstalk
		else {
			if (!grown) {
				transform.localScale += new Vector3 (growthSpeed, growthSpeed, 0);
				if(growSize >= .8f)
					transform.Translate (new Vector2 (0, growthSpeed * (10 - growSize * 3/2)));
				else
					transform.Translate (new Vector2 (0, growthSpeed * (10 - growSize * growSize)));
			}
			if (transform.localScale.y > growSize)
				grown = true;
		}

		// Decay beanstalk after a while
		if (grown) {
			Decay ();
		}

		// Makes beanstalk fall when it's cut
		if (cut) {
			transform.gameObject.tag = "Untagged";
			if (fullyRotated > 0) {
				transform.Rotate (0, 0, .1f * direction);
				fullyRotated -= .1f;
			} else {
				this.transform.GetComponent<Rigidbody2D>().gravityScale = 1f;
				transform.gameObject.GetComponent<CapsuleCollider2D> ().isTrigger = false;
			}
		}
	}

	// Makes the beanstalk decay over time
	void Decay() {
		decayCountdown -= Time.deltaTime;

		if (decayCountdown > decayTimer * 0.5f) {
			sr.color -= new Color(colorChangeRate / 2,
				colorChangeRate, colorChangeRate,0);
		} else if (decayCountdown > decayTimer * 0.3f) {
			sr.color -= new Color(colorChangeRate / 2,
										colorChangeRate, colorChangeRate,0);
			transform.localScale -= new Vector3 (0.01f * growthSpeed, 0.01f * growthSpeed, 0);
			transform.Translate (new Vector2 (0, -1.5f * growthSpeed));
			if(transform.localScale.y < .2f)
				Destroy (this.gameObject);

			if (!isPlaying) {
				if (sr.isVisible) {
					SoundManager.instance.PlaySound ("stalk die");
					isPlaying = true;
				}
			}
		} else if (decayCountdown > decayTimer * 0.05f) {
			sr.color -= new Color(colorChangeRate / 2,
										colorChangeRate, colorChangeRate, 10f *  colorChangeRate);
			transform.localScale -= new Vector3 (2f * growthSpeed, 2f * growthSpeed, 0);
			transform.Translate (new Vector2 (0, -20f * growthSpeed));
			if(transform.localScale.y < .2f)
				Destroy (this.gameObject);
		}  else {
			Destroy (this.gameObject);
			PlayerController.instance.Climb (false);
		}

		if (isPlaying && !sr.isVisible) {
			SoundManager.instance.StopSound ("stalk die");
		}
	}
}
