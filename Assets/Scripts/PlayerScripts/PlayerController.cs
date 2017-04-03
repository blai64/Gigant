using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[HideInInspector] public bool isGrounded = false;
	[HideInInspector] public bool isLeft = false;

	public float maxSpeed = 10f; 

	private int direction;

	public Transform groundCheck;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		InputManager ();

		rb2d.velocity = new Vector2 (direction * maxSpeed, rb2d.velocity.y);
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
	}
}
