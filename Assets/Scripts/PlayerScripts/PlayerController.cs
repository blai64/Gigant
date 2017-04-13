﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public static PlayerController instance;

	private Animator anim;

	[HideInInspector] public bool isGrounded = false;
	[HideInInspector] public bool isLeft = false;


	[HideInInspector] public bool disabled;

	// Beanstalk object
	public GameObject beanstalkPrefab;

	public float initialGravity;

	//####################################################################
	//Movement Logic 

	public float maxSpeed = 10f; 
	public float jumpForce = 10f; 
	//lateral movement
	private int direction; // [-1,0,1], for determining direction of velocity

	//vertical movement
	private int maxJumps = 2; 
	private int remainingJumps; // Remaining number of jumps
	private bool doJump; // try to start jump on current frame

	//Vertical beanstalk 
	private bool isClimbing;

	private bool isKnocking;

	//####################################################################
	//Combat logic

	//####################################################################
	//Checkpoint

	Vector3 checkpointLocation; 

	//####################################################################
	//Bean Logic

	[HideInInspector] public int beanCount;
	public int maxBeans = 5;

	public GameObject weapon; //Sword gameObject
	[HideInInspector] public bool isAttacking;

	public Transform groundCheck;
	private Rigidbody2D rb2d;

	private float prevHeight;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);
	}

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<Animator> ();
		initialGravity = rb2d.gravityScale;

		prevHeight = rb2d.position.y;
	}

	void Update () {
		isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		remainingJumps = (isGrounded) ? maxJumps : remainingJumps;


		if (!disabled) {
			//case on whether or not currently latched onto beanstalk
			if (isClimbing) {
				ClimbingInputManager ();
				rb2d.velocity = new Vector2 (0f, direction * maxSpeed);
			}
			else {
				InputManager ();
				//update lateral movement
				rb2d.velocity = new Vector2 (direction * maxSpeed, rb2d.velocity.y);

				//update vertical movememnt
				if (doJump) {
					remainingJumps--;
					rb2d.velocity =  new Vector2 (rb2d.velocity.x, jumpForce);
					doJump = false; 
				}
			}
		}

		Fall ();
		//Do Combat thing
	}

	void FixedUpdate(){
		if (isKnocking) {
			Knocked ();
		}
	}

	//########################### Input Managers ##############################

	void InputManager() {
		//Lateral Movement

		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A) ||
			((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (direction == 0))) {
			ChangeDirection (true);
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D) ||
			((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (direction == 0))) {
			ChangeDirection (false);
		}

		//move if key held down
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			Move (true);
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {			
			Move (true);
		}

		//only stop moving if key in other direction is not held
		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				ChangeDirection (false);
			} else {
				Move (false);
				direction = 0;
			}

		} else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				ChangeDirection (true);
			} else {
				Move (false);
				direction = 0;
			}
		}

		//Vertical Movement
		//Should only jump if you have remaining jumps and also if you are slow enough
		//to prevent using both jumps immediately
		if (remainingJumps > 0 && 
			rb2d.velocity.y <= 1.0f &&
			(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			doJump = true;
			SoundManager.instance.PlaySound ("jump");
			Jump ();
		}
			
		//Combat
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			isAttacking = true;
			SoundManager.instance.PlaySound ("sword slash");
			Attack ();
		}
			

		if (Input.GetKeyUp (KeyCode.Space)) {
			isAttacking = false;
		}

		//############### Testing area ##################
		//delete when done
		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine (Respawn ());
		}

		// Planting a beanstalk seed
		if (Input.GetKeyDown (KeyCode.P) && isGrounded) {
			PlantBeanstalk ();
		}
	}

	void ClimbingInputManager() {
		direction = 0;

		if (BeanstalkScript.instance.FullyGrown ()) {
			
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				direction = 1;
			} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
				direction = -1; 
			} else if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
				isLeft = Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);

				Climb (false);
			}
		}
	}
		
	//################## State Changing Helper Functions ######################

	public void Climb(bool climb) {
		isClimbing = climb;
		rb2d.gravityScale = (climb) ? 0 : initialGravity;
	}

	private void ChangeDirection(bool left) {
		isLeft = left;
		direction = left ? -1 : 1;
		anim.SetBool ("isLeft", left);
	}

	private void Move(bool isRunning) {
		anim.SetBool ("isRunning", isRunning);
	}

	private void Jump() {
		anim.SetTrigger ("isJumping");
		doJump = true;
	}

	private void Fall() {
		if (!isGrounded) {
			float currHeight = rb2d.position.y;
			if (currHeight - prevHeight < 0f) {
				anim.SetBool ("isFalling", true);
			}
			prevHeight = currHeight;
		} else if (anim.GetBool ("isFalling")) {
			anim.SetBool ("isFalling", false);
		}
	}

	private void Attack() {
		isAttacking = true;
		anim.SetTrigger ("isAttacking");
	}

	//############################### Triggers ################################

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Checkpoint")) {
			checkpointLocation = col.transform.position;
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		// trigger for climbing the beanstalk
		if (col.CompareTag ("Beanstalk") &&
			(Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) &&
		    !isClimbing) {
			transform.position = new Vector3 (col.transform.position.x, 
				transform.position.y, 
				transform.position.z);
			Climb (true);
		}
		// trigger for being in range to cut the beanstalk
		else if (col.CompareTag ("Beanstalk") && isAttacking && 
			BeanstalkScript.instance.FullyGrown()) {
			BeanstalkScript.instance.CutBeanstalk ();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag ("Beanstalk") && isClimbing) {
			//we know that player is exiting off the top
			Climb (false);
			direction = 0;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Collides with Enemy");
			Health.instance.hp--;
			isKnocking = true;
			//Knocked ();
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if(col.gameObject.CompareTag("Enemy")) {
			isKnocking = false;
			anim.SetBool ("isRunning", false);
		}
	}
	
	//############################### Coroutines ################################

	IEnumerator Respawn() {
		yield return new WaitForSeconds (2.0f);
		transform.position = checkpointLocation;
		rb2d.velocity = Vector3.zero;
	}
		

	//################################ Beanstalk ################################

	void PlantBeanstalk() {
		GameObject bean = Instantiate (beanstalkPrefab);
		bean.transform.position = new Vector3 (transform.position.x,
											   transform.position.y - 1.22f,
											   transform.position.z);
	}
		
	//############################ Knocked by Enemy #############################

	void Knocked() {
		Disable (false); 
		//rb2d.velocity = new Vector2(maxSpeed, 5.0f);
		if (direction == 1) {
			rb2d.velocity = new Vector2 (-maxSpeed, 10.0f);
		} else if (direction == -1) {
			rb2d.velocity = new Vector2 (maxSpeed, 10.0f);
		}

		//rb2d.AddForce (new Vector2 (30.0f, 10.0f), ForceMode2D.Impulse);
		//anim.SetBool ("isRunning", false);
	
		StartCoroutine (Wait ());
	}


	IEnumerator Wait() {
		yield return new WaitForSeconds (1.5f);
		Enable (true);
	}


	//#####################################33
	public void Disable(bool loseVelocity){
		disabled = true;
		anim.SetBool ("isRunning", false);
		if (loseVelocity) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			direction = 0;
		}
	}
		
	public void Enable(bool loseVelocity){
		disabled = false;
		if (loseVelocity) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			direction = 0;
		}
	}
}


