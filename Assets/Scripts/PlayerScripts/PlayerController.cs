using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[HideInInspector] public bool isGrounded = false;
	[HideInInspector] public bool isLeft = false;

	public float maxSpeed = 10f; 
	public float jumpForce = 10f; 

	//lateral movement
	private int direction; // [-1,0,1], for determining direction of velocity

	//vertical movement
	private int maxJumps = 2; 
	private int remainingJumps; // Remaining number of jumps
	private bool doJump; // try to start jump on current frame


	public Transform groundCheck;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		remainingJumps = (isGrounded) ? maxJumps : remainingJumps;

		InputManager ();

		//update lateral movement
		rb2d.velocity = new Vector2 (direction * maxSpeed, rb2d.velocity.y);

		//update vertical movememnt
		if (doJump){
			remainingJumps--;
			rb2d.velocity =  new Vector2 (rb2d.velocity.x, jumpForce);
			doJump = false; 
		}
	}

	void InputManager(){
		//Lateral Movement
		direction = 0;
		if (Input.GetKey (KeyCode.LeftArrow)) {
			isLeft = true;
			direction = -1;
		} 
		else if (Input.GetKey (KeyCode.RightArrow)) {
			isLeft = false; 
			direction = 1; 
		}

		//Vertical Movement
		//Should only jump if you have remaining jumps and also if you are slow enough
		//to prevent using both jumps immediately
		if (remainingJumps > 0 && 
			rb2d.velocity.y <= 1.0f &&
			Input.GetKeyDown(KeyCode.UpArrow)){
			doJump = true;
		}
	}
}
