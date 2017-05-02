using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public static PlayerController instance;

	private Animator anim;

	[HideInInspector] public bool isGrounded = false;
	[HideInInspector] public bool isLeft;

	[HideInInspector] public bool isDead;

	[HideInInspector] public bool disabled;

	// Beanstalk object
	public GameObject beanstalkPrefab;

	public float initialGravity;

	private int megaGolemsLeft = 2;

	//####################################################################
	//Movement Logic 

	public float maxSpeed = 10f; 
	public float jumpForce = 20f; 															
	//lateral movement
	private int horizontalDirection; // [-1,0,1], for determining direction of velocity
	private int verticalDirection;

	//vertical movement
	private int maxJumps = 1; 																// Alex) Changed maxJumps to 1 4/23
	private int remainingJumps; // Remaining number of jumps
	private bool doJump; // try to start jump on current frame

	//Vertical beanstalk 
	private bool canClimb; // this stops him from immediately climbing after k=jumping off
	private bool isClimbing;
	private bool atTopOfStalk;
	private CapsuleCollider2D beanstalkCollider;

	//private bool isKnocking;
	private bool hurting;

	private bool inFrontOfTunnel;

	//####################################################################
	//Combat logic
	private float attackCooldown = 0;
	public GameObject swordHitbox;
	private float damageDisableTime = 0.8f;

	//####################################################################
	//Checkpoint

	Vector3 checkpointLocation; 
	BoxCollider2D checkpointCameraBound;

	//####################################################################
	//Bean Logic

	[HideInInspector] public int beanCount;
	public int maxBeans = 5;
	private bool canPlantBean;


	public GameObject weapon; //Sword gameObject
	[HideInInspector] public bool isAttacking;

	public Transform[] groundChecks;
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

		isLeft = true;
	}

	void DoGroundCheck(){
		foreach (Transform groundCheck in groundChecks) {
			canPlantBean = Physics2D.Linecast (transform.position, groundCheck.position,
				1 << LayerMask.NameToLayer ("Ground"));
			isGrounded = Physics2D.Linecast (transform.position, groundCheck.position,
				1 << LayerMask.NameToLayer ("Ground") | 1 << LayerMask.NameToLayer("Enemy"));
			if (isGrounded) {
				break;
			}
		}
	}

	public void KilledMegaGolem(){
		megaGolemsLeft -= 1;
	}

	public int MegaGolemsLeft(){
		return megaGolemsLeft; 
	}

	void Update () {
		DoGroundCheck ();
		if (isGrounded && isClimbing && !(Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W))) {
			Climb (false);
		}

		canClimb = (isGrounded && !isClimbing) ? true : canClimb;
		remainingJumps = (isGrounded || isClimbing) ? maxJumps : remainingJumps;

//		if (Health.instance.hp <= 0 && !isDead) {
//			Die (false);
//		}

		if (!disabled) {
			if (attackCooldown >= 0)														// Alex) puts a cooldown on attacking	4/23
				attackCooldown -= Time.deltaTime;
			//case on whether or not currently latched onto beanstalk
			if (isClimbing) {
				ClimbingInputManager ();
				rb2d.velocity = new Vector2 (0f, verticalDirection * maxSpeed);
				/*
				if (atTopOfStalk && verticalDirection < 0) {
					atTopOfStalk = false; 
				} else if (atTopOfStalk) {
					rb2d.velocity = new Vector2 (0f, 0f);
				}*/

				float curY = transform.position.y; 
				if (beanstalkCollider != null){
					curY = Mathf.Clamp (curY, Mathf.NegativeInfinity, beanstalkCollider.bounds.center.y + beanstalkCollider.bounds.extents.y);
					transform.position = new Vector3 (transform.position.x, curY, transform.position.z);
				}
					
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
		} else if (isGrounded) {
			rb2d.velocity = new Vector2 (0f, 0f);
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
			rb2d.velocity.y <= 1.0f && !inFrontOfTunnel && 
			(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			doJump = true;
			SoundManager.instance.PlaySound ("jump");
			Jump ();
		}
			
		//Combat
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded && attackCooldown < 0) {
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
			if (beanCount > 0 && canPlantBean)
				PlantBeanstalk ();
		}
	}

	void ClimbingInputManager() {
		verticalDirection = 0;
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
				ChangeDirection (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A));
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
		if (isLeft ^ left)
			swordHitbox.transform.Rotate (0f, 0f, 180f);
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
		float currHeight = rb2d.position.y;
		if (!isGrounded) {
			if (!isClimbing) {
				if (currHeight - prevHeight < 0f && !hurting) {
					anim.SetBool ("isJumping", false);
					anim.SetBool ("isClimbing", false);
					anim.SetBool ("isFalling", true);
				}

			}
		} else if (anim.GetBool ("isFalling")) {
			anim.SetBool ("isJumping", false);
			anim.SetBool ("isFalling", false);
		} else {
			anim.SetBool ("isJumping", false);
		}
		prevHeight = currHeight;
	}

	private void Attack() {
		isAttacking = true;
		attackCooldown = .5f;
		anim.SetTrigger ("isAttacking");
		anim.SetBool ("isClimbing", false);
	}

	private void Die(bool fell) {
		SoundManager.instance.PlaySound ("death");

		anim.SetBool ("isClimbing", false);
		if (!fell)
			anim.SetTrigger ("isDead");
		Disable (false);
		isDead = true;

		CutsceneManager.instance.playerRespawning = true;
		CutsceneManager.instance.causeOfDeath = (fell) ? "fall" : "enemy";
		rb2d.velocity = Vector2.zero;

		StartCoroutine (Respawn ());
	}

	//############################### Triggers ################################

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Checkpoint")) {
			checkpointLocation = col.transform.position;
			checkpointCameraBound = MainCamera.instance.cameraBounds;
		}
		if(col.gameObject.CompareTag("Pit")) {
			Die(true);
		}
		if(col.gameObject.CompareTag("Boulder")) {
			Health.instance.hp--;
			Knocked ((col.transform.position.x < transform.position.x));
			hurting = true;
			anim.SetTrigger ("isHurt");
		}
		if (col.gameObject.CompareTag ("Beanstalk")) {
			canClimb = true;
		}
		if (col.gameObject.CompareTag ("SceneChangeTrigger") && col.gameObject.GetComponent<SceneChangeTrigger>().isTunnel) {
			inFrontOfTunnel = true;
		}
		if (col.gameObject.CompareTag ("BoulderStart")) {
			BoulderManager.instance.startFalling = true;
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		// trigger for climbing the beanstalk
		if (col.CompareTag ("Beanstalk") && canClimb &&
			(Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
			&& !isClimbing && col.gameObject.GetComponent<BeanstalkScript>().FullyGrown()) {
			transform.position = new Vector3 (col.transform.position.x, 
				transform.position.y, 
				transform.position.z);
			beanstalkCollider = col.gameObject.GetComponent<CapsuleCollider2D> ();
			Climb (true);
			canClimb = false;
		}
							// Alex) Put beanstalk cutting script
							//       on sword itself 4/23
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag ("Beanstalk") && isClimbing) {
			//we know that player is exiting off the top
			atTopOfStalk = true;
		}
		if (col.gameObject.CompareTag ("SceneChangeTrigger") && col.gameObject.GetComponent<SceneChangeTrigger>().isTunnel) {
			inFrontOfTunnel = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag ("Damage") && !hurting) {
			Health.instance.hp--;
			if (Health.instance.hp <= 0 && !isDead) {
				Die (false);
			} else {
				SoundManager.instance.PlaySound ("death");
				Knocked ((col.transform.position.x < transform.position.x));
				hurting = true;
				anim.SetTrigger ("isHurt");
			}
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
		anim.ResetTrigger ("isHurt");
		anim.ResetTrigger ("isDead");
		anim.ResetTrigger ("isAttacking");
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

	void Knocked(bool hitFromLeft) {
		Disable (false); 
		//rb2d.velocity = new Vector2(maxSpeed, 5.0f);

		ChangeDirection (hitFromLeft);

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
		yield return new WaitForSeconds (damageDisableTime);
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


