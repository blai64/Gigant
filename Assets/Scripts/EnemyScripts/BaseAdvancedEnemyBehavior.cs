using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAdvancedEnemyBehavior : MonoBehaviour {

	[HideInInspector] public bool isActive; //flag for detecting player
	[HideInInspector] public bool isAttacking; //flag for attacking state
	[HideInInspector] public float direction;
	[HideInInspector] public bool isAttacked; //flag for attacked by Jack

	private bool canBeActivated;

	private float moveSpeed = 1.0f;

	public int health;

	private Rigidbody2D rb2d; 


	protected Animator anim;

	private Vector3 originalPosition;

	public GameObject psystemPrefab;
	public Transform dustSpawnFeet;
	public Transform dustSpawnArmRight;
	public Transform dustSpawnArmLeft;

	// Color changing parameters
	private SpriteRenderer renderer;
	private Color originalColor;
	private List<GameObject> childList = new List<GameObject>();
	public BoxCollider2D bounds; 
	public GameObject VictoryVine;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		canBeActivated = true;
		health = 5;
		anim.SetBool ("isLeft", true);
		renderer = this.GetComponentInChildren<SpriteRenderer> ();
		originalColor = this.GetComponentInChildren<SpriteRenderer> ().color;
		for (int i = 0; i < this.gameObject.transform.GetChild (0).childCount; i++) {
			childList.Add (this.gameObject.transform.GetChild (0).GetChild (i).gameObject);
		}

		originalPosition = transform.position;
	}

	// turns golem red when hit
	public void RedFlash(){
		renderer.color = new Color (1, 0, 0, 1);
		foreach (GameObject child in childList) {
			child.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
		}
	}

	// turns golem back to grey after being hit
	public void RevertFromRed(){
		renderer.color = originalColor;
		foreach (GameObject child in childList) {
			child.GetComponent<SpriteRenderer> ().color = originalColor;
		}
	}

	// Update is called once per frame
	virtual protected void Update () {
		// Turns boxColliders on arms on and off depending on whether the enemy is attacking or not
		if (isAttacking) {
			this.gameObject.transform.GetChild (0).GetChild (1).gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			this.gameObject.transform.GetChild (0).GetChild (2).gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		} else {
			this.gameObject.transform.GetChild (0).GetChild (1).gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			this.gameObject.transform.GetChild (0).GetChild (2).gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
		//only move when not dead or attacking
		if (isActive  && !isAttacking) {
			direction = Mathf.Sign (PlayerController.instance.transform.position.x - transform.position.x);
			anim.SetBool ("isLeft", (direction < 0));
			rb2d.velocity = new Vector2 (direction * moveSpeed, rb2d.velocity.y);
			if (bounds != null) {
				float x = transform.position.x;
				float y = transform.position.y;


				x = Mathf.Clamp(x, bounds.bounds.center.x - bounds.bounds.extents.x, bounds.bounds.center.x + bounds.bounds.extents.x);
				y = Mathf.Clamp(y, bounds.bounds.center.y - bounds.bounds.extents.y, bounds.bounds.center.y + bounds.bounds.extents.y);

				transform.position = new Vector3(x, y, transform.position.z);
			}

		}else {
			rb2d.velocity = Vector2.zero;
		}

	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !isActive && canBeActivated){
			anim.SetTrigger("isActivated");
		}

		if (col.CompareTag ("Weapon") && PlayerController.instance.isAttacking) {
			//		GetDamaged (1);
		}
	}
	void OnTriggerStay2D (Collider2D col){
		if (col.CompareTag("Player") && !isActive && canBeActivated){
			anim.SetTrigger("isActivated");
		}		
	}
	public void EndAttack(){
		isAttacking = false;
		anim.SetTrigger ("isWalking");
	}

	virtual public void DoAttack(){
	}


	public void GetDamaged (int damage) {

		float rand = Random.value;
		if (rand < 0.5) {
			SoundManager.instance.PlaySound ("sword hit 1");
		} else {
			SoundManager.instance.PlaySound ("sword hit 1");
		}

		health -= damage;

		if (health <= 0 && isActive) {
			Die ();
		}
	}

	void Die(){
		//Destroy (gameObject);//temporary...

		SoundManager.instance.PlaySound ("enemy crumble");

		//start Death animation
		this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
		this.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
		this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y,-2);
		isActive = false;
		isAttacking = false;
		canBeActivated = false;
		anim.SetTrigger ("isDeactivated");
		StartCoroutine (DisableForTime (3.0f));
		PlayerController.instance.KilledMegaGolem ();
		if(PlayerController.instance.MegaGolemsLeft() == 0)
		{
			GameObject vine = Instantiate (VictoryVine);
			vine.transform.position = new Vector3 (PlayerController.instance.transform.position.x, 
												   PlayerController.instance.transform.position.y - 1.22f,
												   -2);
		}
	}


	IEnumerator DisableForTime(float seconds){
		yield return new WaitForSeconds (seconds);
		//canBeActivated = true;
	}

	public void Activate() {

		SoundManager.instance.PlaySound ("enemy crumble");

		isActive = true;
		anim.ResetTrigger ("isActivated");

		anim.SetTrigger ("isWalking");
		health = 5;
	}

	public void Reset(){
		if (isActive) {
			anim.SetTrigger ("isDeactivated");
		}

		transform.position = originalPosition;
		isActive = false; 
		isAttacked = false; 
		isAttacking = false;
	}

	public void DoEmit(string method) {

		print ("should emit cloud");
		if (psystemPrefab != null) {
			for (int i = -1; i < 2; i++) {
				GameObject newParticleSystem = Instantiate (psystemPrefab);
				if (method == "feet")
					newParticleSystem.transform.position = dustSpawnFeet.position + new Vector3(i * 0.5f, 0,0);
				else {
					if (direction < 0)
						newParticleSystem.transform.position = dustSpawnArmLeft.position + new Vector3(i * 0.5f, 0,0);
					else
						newParticleSystem.transform.position = dustSpawnArmRight.position + new Vector3(i * 0.5f, 0,0);
				}
			}



		}
	}

	public void StartDamaging(){
		isAttacking = true;
	}
}
