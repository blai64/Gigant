using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public static PlayerController instance;

	private Animator anim;

	[HideInInspector] public bool isGrounded = false;
	[HideInInspector] public bool isLeft = false;

	[HideInInspector] public bool isDead;


	[HideInInspector] public bool disabled;

	// Beanstalk object
	public GameObject beanstalkPrefab;

	public float initialGravity;

	//####################################################################
	//Movement Logic 

	public float maxSpeed = 10f; 
	public float jumpForce = 10f; 
	//lateral movement
	private int horizontalDirection; // [-1,0,1], for determining direction of velocity
	private int verticalDirection;

	//vertical movement
	private int maxJumps = 2; 
	private int remainingJumps; // Remaining number of jumps
	private bool doJump; // try to start jump on current frame

	//Vertical beanstalk 
	private bool isClimbing;

	//private bool isKnocking;
	private bool hurting;

	//####################################################################
	//Combat logic

	//####################################################################
	//Checkpoint

	Vector3 checkpointLocation; 
	BoxCollider2D checkpointCameraBound;

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

		checkpointLocation = transform.position;
		checkpointCameraBound = MainCamera.instance.cameraBounds;
	}

	void Update () {
		isGrounded = Physics2D.Linecast (transform.position, groundCheck.position,
										 1 << LayerMask.NameToLayer ("Ground"));
		remainingJumps = (isGrounded) ? maxJumps : remainingJumps;

		if (Health.instance.hp <= 0 && !isDead) {
			Die ();
		}

		if (!disabled) {
			//case on whether or not currently latched onto beanstalk
			if (isClimbing) {
				ClimbingInputManager ();
				rb2d.velocity = new Vector2 (0f, verticalDirection * maxSpeed);
			} else {
				InputManager ();
				//update lateral movement
				rb2d.velocity = new Vector2 (horizontalDirection * maxSpeed, rb2d.velocity.y);

				//update vertical movememnt
				if (doJump) {
					remainingJumps--;
					rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
					doJump = false; 
				}
			}
		}

		Fall ();
		//Do Combat thing
	}

	//########################### Input Managers ##############################

	void InputManager() {
		//Lateral Movement
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A) ||
			((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (horizontalDirection == 0))) {
			ChangeDirection (true);
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D) ||
			((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (horizontalDirection == 0))) {
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
				horizontalDirection = 0;
			}

		} else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				ChangeDirection (true);
			} else {
				Move (false);
				horizontalDirection = 0;
			}
		}

		//Vertical Movement
		//Should only jump if you have remaining jumps and also if you are slow enough
		//to prevent using both jumps immediately
		if (remainingJumps > 0 && !isClimbing &&
			rb2d.velocity.y <= 1.0f &&
			(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			doJump = true;
			SoundManager.instance.PlaySound ("jump");
			Jump ();
		}
			
		//Combat
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			SoundManager.instance.PlaySound ("sword slash");
			Attack ();
		}
		if (Input.GetKeyUp (KeyCode.Space))
			isAttacking = false;

		//############### Testing area ##################
		//delete when done
		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine (Respawn ());
		}

		// Planting a beanstalk seed
		if ((Input.GetKeyDown (KeyCode.P) ||
			 Input.GetKeyDown (KeyCode.LeftShift) ||
		     Input.GetKeyDown (KeyCode.RightShift)) && isGrounded) {
			if (beanCount > 0)
				PlantBeanstalk ();
		}
	}

	void ClimbingInputManager() {
		verticalDirection = 0;

		//if (BeanstalkScript.instance.FullyGrown ())
		{
			
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				anim.enabled = true;
				verticalDirection = 1;
			} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
				anim.enabled = true;
				verticalDirection = -1; 
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow) ||
			    Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.D)) {
				isLeft = Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
				Climb (false);
			}
		}

		if (anim.GetBool("isClimbing") && (Input.GetKeyUp (KeyCode.UpArrow) ||
			Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.DownArrow) ||
			Input.GetKeyUp (KeyCode.S))) {
			anim.enabled = false;
		}
	}
		
	//################## State Changing Helper Functions ######################

	public void Climb(bool climb) {
		horizontalDirection = 0;
		isClimbing = climb;
		rb2d.gravityScale = (climb) ? 0 : initialGravity;
		anim.SetBool ("isClimbing", climb);
		anim.SetBool ("isJumping", false);
		anim.enabled = true;
	}

	private void ChangeDirection(bool left) {
		isLeft = left;
		horizontalDirection = left ? -1 : 1;
		anim.SetBool ("isLeft", left);
	}

	private void Move(bool isRunning) {
		anim.SetBool ("isRunning", isRunning);
	}

	private void Jump() {
		anim.SetBool ("isJumping", true);
		anim.SetBool ("isClimbing", false);
		doJump = true;
	}

	private void Fall() {
		if (!isGrounded) {
			if (!isClimbing) {
				float currHeight = rb2d.position.y;
				if (currHeight - prevHeight < 0f && !hurting) {
					anim.SetBool ("isJumping", false);
					anim.SetBool ("isClimbing", false);
					anim.SetBool ("isFalling", true);
				}
				prevHeight = currHeight;
			}
		} else if (anim.GetBool ("isFalling")) {
			anim.SetBool ("isFalling", false);
		}
	}

	private void Attack() {
		isAttacking = true;
		anim.SetTrigger ("isAttacking");
		anim.SetBool ("isClimbing", false);
	}

	private void Die() {
		anim.SetBool ("isClimbing", false);
		anim.SetTrigger ("isDead");
		Disable (false);
		isDead = true;
		StartCoroutine (Respawn ());
	}

	//############################### Triggers ################################

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Checkpoint")) {
			checkpointLocation = col.transform.position;
			checkpointCameraBound = MainCamera.instance.cameraBounds;
		}
		if(col.gameObject.CompareTag("Pit")){
			Die();
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		// trigger for climbing the beanstalk
		if (col.CompareTag ("Beanstalk") && 
			(Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
			&& !isClimbing && col.gameObject.GetComponent<BeanstalkScript>().FullyGrown()) {
			transform.position = new Vector3 (col.transform.position.x, 
				transform.position.y, 
				transform.position.z);
			Climb (true);
		}
		// trigger for being in range to cut the beanstalk
		else if (col.CompareTag ("Beanstalk") && isAttacking && 
			col.gameObject.GetComponent<BeanstalkScript>().FullyGrown()){
			col.gameObject.GetComponent<BeanstalkScript> ().CutBeanstalk();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag ("Beanstalk") && isClimbing) {
			//we know that player is exiting off the top
			Climb (false);

		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag ("Enemy")) {
			Health.instance.hp--;
			Knocked ();
			hurting = true;
			anim.SetTrigger ("isHurt");
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if(col.gameObject.CompareTag("Enemy")) {
			anim.SetBool ("isRunning", false);
		}
	}
	
	//############################### Coroutines ################################

	IEnumerator Respawn() {
		yield return new WaitForSeconds (1.0f);
		MosaicCameraScript.instance.SetTargetPosition (checkpointLocation, checkpointCameraBound);
		Health.instance.hp = 3;
		isDead = false; 
	}
		

	//################################ Beanstalk ################################

	void PlantBeanstalk() {
		GameObject bean = Instantiate (beanstalkPrefab);
		bean.transform.position = new Vector3 (transform.position.x,
											   transform.position.y - 1.22f,
											   transform.position.z);
		beanCount--;
	}
		
	//############################ Knocked by Enemy #############################

	void Knocked() {
		Disable (false); 
		//rb2d.velocity = new Vector2(maxSpeed, 5.0f);

		if (!isLeft) {
			rb2d.velocity = new Vector2 (-maxSpeed, 10.0f);
		} else {
			rb2d.velocity = new Vector2 (maxSpeed, 10.0f);
		}

		//rb2d.AddForce (new Vector2 (30.0f, 10.0f), ForceMode2D.Impulse);
		//anim.SetBool ("isRunning", false);
	
		StartCoroutine (Wait ());
	}


	IEnumerator Wait() {
		yield return new WaitForSeconds (1.2f);
		hurting = false;
		Enable (true);
	}


	//#####################################33
	public void Disable(bool loseVelocity){
		disabled = true;
		anim.SetBool ("isRunning", false);
		if (loseVelocity) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			horizontalDirection = 0;
		}
	}
		
	public void Enable(bool loseVelocity){
		disabled = false;
		if (loseVelocity) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			horizontalDirection = 0;
		}
	}
}


