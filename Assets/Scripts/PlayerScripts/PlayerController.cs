using System.Collections;
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
	}

	void Update () {
		if (!disabled) {
			isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
			remainingJumps = (isGrounded) ? maxJumps : remainingJumps;

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


			
		//Do Combat thing
	}

	void FixedUpdate(){
		if (isKnocking) {
			Knocked ();
		}
	}


	//############################################ Input Managers ###################################
	void InputManager() {
		//Lateral Movement
		direction = 0;

		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			ChangeDirection (true);
			Move (true);
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			ChangeDirection (false);
			Move (true);
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
			Move (false);
		} else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
			Move (false);
		}

		//Vertical Movement
		//Should only jump if you have remaining jumps and also if you are slow enough
		//to prevent using both jumps immediately
		if (remainingJumps > 0 && 
			rb2d.velocity.y <= 1.0f &&
			(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			doJump = true;
		}


		//Combat
		if (Input.GetKeyDown(KeyCode.Space)) {
			isAttacking = true;
		}

		//###############Testing area##################
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

		if (BeanstalkScript.instance.FullyGrown ()) 
		{
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				direction = 1;
			} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
				direction = -1; 
			} else if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
				isLeft = Input.GetKeyDown (KeyCode.LeftArrow); 
				Climb (false);

			}
		}
	}


	//#################################### State Changing Helper Functions #########################

	public void Climb(bool climb){
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

	//#################################### Triggers #########################

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Checkpoint")) {
			checkpointLocation = col.transform.position;
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		// trigger for climbing the beanstalk
		if (col.CompareTag ("Beanstalk") &&
		    Input.GetKeyDown (KeyCode.UpArrow) &&
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
			Climb (false);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Collides with Enemy");
			Health.instance.hp--;
			isKnocking = true;
			//Knocked ();
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if(col.gameObject.CompareTag("Enemy")){
			isKnocking = false;

		}
	}
		
	
	//#################################### Coroutines #########################
	IEnumerator Respawn(){
		yield return new WaitForSeconds (2.0f);
		transform.position = checkpointLocation;
		rb2d.velocity = Vector3.zero;
	}

	//#################################### Beanstalk #########################
	void PlantBeanstalk(){
		GameObject bean = Instantiate (beanstalkPrefab);
		bean.transform.position = new Vector3 (transform.position.x, transform.position.y - 1.22f,transform.position.z);
	}

	//#################################### Knocked by Enemy #########################
	void Knocked(){
		disabled = true;
		if (direction == 1) {
			rb2d.velocity = new Vector2 (-maxSpeed * 3.0f, 10.0f);
		} else if (direction == -1) {
			rb2d.velocity = new Vector2 (maxSpeed * 3.0f, 10.0f);
		}
		StartCoroutine (Wait ());
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds (1.5f);
		disabled = false;
	}
}


