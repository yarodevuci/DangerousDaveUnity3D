using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviour {

	public BoxCollider2D playerColliderBox;
	public float speedX;
	public float jumpHeight;

	public Text scoreLabel;
	public Text currentLevel;

	public GameObject player_Explosion;
	private GameObject instantiateExplosion;
	Transform playerPosition;


	public GameObject bottomExitText;
	public GameObject JetFuelBar;
	public GameObject PlayerDave;
	//Bullet
	public GameObject rightBullet;
	public GameObject leftBullet;

	Transform firePos;

	//public float jumpspeedY;

	bool facingRight, hasKey;
	float speed;

	bool leftKeyPressed;
	bool leftKeyReleased;

	bool jumpButtonPressed;
	bool jumpButtonReleased;

	bool rightKeyPressed;
	bool rightKeyReleased;

	bool isPlayerFrozen;
	bool needToZeroSpeed;

	Animator anim;
	Rigidbody2D rb;

	//Lives:
	public GameObject live1;
	public GameObject live2;
	public GameObject live3;
	//Sounds 
	public AudioClip jetIdle_Clip;
	public AudioClip jet_IsOn_Clip;
	public AudioClip jumpSound;
	public AudioClip gemPickedUp;
	public AudioClip gotSpecial;
	public AudioClip gotKey;
	public AudioClip walking;
	public AudioClip falling;
	public AudioClip dead;
	new AudioSource audio;

	private Vector3 spown;

	// Use this for initialization
	void Start () {
		
		if (GameStatus.lives == 2) {
			live3.SetActive (false);
		}
		else if (GameStatus.lives == 1) {
			live3.SetActive (false);
			live2.SetActive (false);
		}
		else if (GameStatus.lives == 0) {
			live3.SetActive (false);
			live2.SetActive (false);
			live1.SetActive (false);
		}
		GameStatus.isRichedTrigger = false;
		currentLevel.text = "LEVEL 0" + GameStatus.currentLevel.ToString();
		scoreLabel.text = GameStatus.score.ToString ();
		firePos = transform.FindChild ("FirePosition");
		spown = transform.position;
		GameStatus.isJetPackOn = false;
		GameStatus.isJetPackPickedUp = false; 
		//Hide UI Mobile Buttons when not deployed for mobile
		if (!GameStatus.isDeployedToMobile) {
			GameObject.Find ("Jump_Button").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("RightWalk_Button").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("LeftWalk_Button").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("Home_Button").transform.localScale = new Vector3 (0, 0, 0);
		}

		audio = GetComponent<AudioSource>();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D>();

		facingRight = true;
		hasKey = false;
		bottomExitText.SetActive(false);
		if (GameStatus.currentLevel != 1) {
			JetFuelBar.SetActive (false);
		}

		isPlayerFrozen = false;
		needToZeroSpeed = false;

	}
		
	// Update is called once per frame
	void Update () {
		Jet();
		Flip ();
		MovePlayer (speed);
		//playWalkingAnim ();

		leftKeyPressed = Input.GetKey(KeyCode.LeftArrow);
		leftKeyReleased = Input.GetKeyUp(KeyCode.LeftArrow);

		rightKeyPressed = Input.GetKey(KeyCode.RightArrow);
		rightKeyReleased = Input.GetKeyUp(KeyCode.RightArrow);

		jumpButtonPressed = Input.GetKeyDown(KeyCode.UpArrow);
		jumpButtonReleased = Input.GetKeyUp(KeyCode.UpArrow);

		playerPosition = PlayerDave.transform;

		if (Input.GetKeyDown (KeyCode.LeftControl) && Time.time > GameStatus.nextFire && GameStatus.isGunPickedUp) {
			Fire();
		}

		if (GameStatus.fillAmount <= 0 && GameStatus.isJetPackOn) {
			print("Fuel is empty");
			GameStatus.isAltButtonPressed = false;
			GameStatus.isJetPackOn = false;
			JetFuelBar.SetActive (false);
			speed = 0;
			rb.gravityScale = 1;
			anim.SetInteger ("State", 0);
		}
			
		//JetPackControls
		if (GameStatus.isJetPackOn && GameStatus.isJetPackPickedUp && !isPlayerFrozen) {
			//<-----
			if (Input.GetKey(KeyCode.LeftArrow) || GameStatus.isJetLeftButtonPressed) {
				JetWillFlyLeft ();
			}
			//----->
			if (Input.GetKey(KeyCode.RightArrow) || GameStatus.isJetRightButtonPressed) {
				JetWillFlyRight ();
			}

			if (Input.GetKey(KeyCode.UpArrow) || GameStatus.isJetUpButtonPressed) {
				JetWillFlyUp();
			}
			if (Input.GetKey(KeyCode.DownArrow) || GameStatus.isJetDownButtonPressed) {
				JetWillFlyDown();
			}
		}
		//Walking 
		if (leftKeyPressed && !GameStatus.isJetPackOn) {
			WalkLeft();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			GoToMainMenu ();
		}

		if (rightKeyPressed && !GameStatus.isJetPackOn) {
			WalkRight();
		}

		if ((leftKeyReleased || rightKeyReleased) && !GameStatus.isJetPackOn) {
			LeftOrRightButtonGotReleased ();
		}
			
		if (jumpButtonPressed && !GameStatus.isJetPackOn) {
			JumpButtonPressed ();
		}

		if (jumpButtonReleased) {
			JumpButtonReleased();
		}
	}
	//Jet Controls
	void JetWillFlyUp() {
		GameStatus.fillAmount -= Time.deltaTime / 12;//0.0003f;
		transform.position += Vector3.up * 4 * Time.deltaTime;
	}

	void JetWillFlyDown() {
		GameStatus.fillAmount -= Time.deltaTime / 20;
		transform.position += Vector3.down * 4 * Time.deltaTime;
	}

	void JetWillFlyLeft() {
		speed = -speedX;
		GameStatus.fillAmount -= Time.deltaTime / 15;
		transform.position += Vector3.left * 4 * Time.deltaTime;
	}

	void JetWillFlyRight() {
		speed = speedX;
		GameStatus.fillAmount -= Time.deltaTime / 15;
		transform.position += Vector3.right * 4 * Time.deltaTime;
	}

	void MovePlayer(float playerSpeed) {
		if (GameStatus.isJetPackOn) {
			speed = 0;
		}
		if (GameStatus.isAltButtonPressed) {
			anim.SetInteger ("State", 99);

		} else if (!GameStatus.isAltButtonPressed) {
			if ((speed > 0 || speed < 0) && GameStatus.isGrounded) {
				anim.SetInteger ("State", 2); //walk
			} else if (!GameStatus.isGrounded && (speed > 0 || speed < 0)) {
				anim.SetInteger ("State", 1); //jump
			} else if (!GameStatus.isGrounded && speed == 0) {
				anim.SetInteger ("State", 1);
			} else if (GameStatus.isGrounded && (leftKeyPressed || rightKeyPressed)) {
				anim.SetInteger ("State", 2); //walk
			} else {
				anim.SetInteger ("State", 0);
			}
		}
		rb.velocity = new Vector3 (speed, rb.velocity.y, 0);
	}
	//Flip Player when Left arrow pressed
	void Flip() {
		if (speed > 0 && !facingRight || speed < 0 && facingRight) {
			if (needToZeroSpeed) {
				speed = 0;
				needToZeroSpeed = false;
			}
			facingRight = !facingRight;
			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
	}
		
	void GoToNextLevel() {
		SceneManager.LoadScene ("Level_Complete");
	}

	void RestartCurrentLevel() {

		PlayerDave.SetActive (false);
		speed = 0;
		GameStatus.isAltButtonPressed = false;
		GameStatus.isJetPackOn = false;
		rb.gravityScale = 1;
		rb.constraints = RigidbodyConstraints2D.None;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		isPlayerFrozen = false;
		playerColliderBox.enabled = true;
		transform.position = spown;
		PlayerDave.SetActive (true);

		//Spawn player with facing right
		Vector3 temp = transform.localScale;
		if (temp.x < 0) {
			needToZeroSpeed = true;
			speed = 1;
		}

	}

	void OnTriggerEnter2D(Collider2D c) {

		if (c.gameObject.tag == "key") {
			bottomExitText.SetActive (true);
			audio.PlayOneShot(gotKey, 0.9F);
			hasKey = true;
		}

		if (c.gameObject.tag == "startShooting") {
			GameStatus.isRichedTrigger = true;
			print ("Oh yea shooting now");
		}

		if (c.gameObject.tag == "monster_bullet") {
			actionIfDead ();
			Destroy (c.gameObject);
		}
		//Player picks up Jetpack
		if (c.gameObject.tag == "jet") {
			JetFuelBar.SetActive (true);
			GameStatus.isJetPackPickedUp = true;
			audio.Stop ();
			audio.PlayOneShot(gotSpecial, 1);
			Destroy(c.gameObject);
		}

		if (c.gameObject.tag == "gem") {
			audio.Stop ();
			audio.PlayOneShot(gemPickedUp, 0.7f);
		}
	}

	//If we touched ground allow jump and set the player to Idle mode
	void OnCollisionEnter2D(Collision2D other) {

		if(other.gameObject.tag == "gun") {
			audio.PlayOneShot(gotSpecial, 0.8f);
			Destroy(other.gameObject);
			GameStatus.allowToFire = true;
			GameStatus.isGunPickedUp = true;
		}
			
		if (other.gameObject.tag == "enemy" || other.gameObject.tag == "hazard") {
			actionIfDead ();
		}

		if (other.gameObject.tag == "door" && hasKey == true) {
			GoToNextLevel ();
		}
	}

	void actionIfDead() {
		playerColliderBox.enabled = false;
		isPlayerFrozen = true;
		GameStatus.lives -= 1;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

		audio.PlayOneShot(dead, 0.8F);
		instantiateExplosion = (GameObject)	Instantiate (player_Explosion, playerPosition.position, Quaternion.identity);
		Destroy (instantiateExplosion, 2f);

		if (GameStatus.lives == 2) {
			live3.SetActive (false);
			Invoke("RestartCurrentLevel", 2f );
		}
		else if (GameStatus.lives == 1) {
			live2.SetActive (false);
			Invoke("RestartCurrentLevel", 2f );
		}
		else if (GameStatus.lives == 0) {
			live1.SetActive (false);
			Invoke("RestartCurrentLevel", 2f );
		}
		else {
			StartCoroutine (GameOver());			
		}
	}
		

	IEnumerator GameOver() {
		yield return new WaitForSeconds (2);
		SceneManager.LoadScene ("GameOver");
	}

	//Mobile UI Stuff
	public void WalkLeft() {
		if (!isPlayerFrozen && !GameStatus.isJetPackOn) {
			speed = -speedX;
			if (GameStatus.isGrounded) { anim.SetInteger ("State", 2); }

		}
	}

	public void WalkRight() {
		if (!isPlayerFrozen && !GameStatus.isJetPackOn) {
			speed = speedX;
			if (GameStatus.isGrounded) { anim.SetInteger ("State", 2); }
		}
	}

	public void LeftOrRightButtonGotReleased() {
		if (!isPlayerFrozen) {
			speed = 0;
		}
	}

	public void JumpButtonReleased() {
		if (!isPlayerFrozen && !GameStatus.isJetPackOn) {
		}
	}

	public void JumpButtonPressed() {
		if (GameStatus.isGrounded && !isPlayerFrozen && !GameStatus.isJetPackOn) {
			rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
			//rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse); //Old Way
			//rb.AddForce (new Vector2 (rb.velocity.x, jumpspeedY)); //another old way
			audio.Stop ();
			anim.SetInteger ("State", 1); //Jump
			audio.PlayOneShot(jumpSound, 0.7F);
		}
	}

	public void GoToMainMenu() {
		GameStatus.score = 0;
		SceneManager.LoadScene ("Main_Menu");
	}
		
	public void Fire() {
		if (Time.time > GameStatus.nextFire && GameStatus.isGunPickedUp && !isPlayerFrozen) {
			if (facingRight) {
				Instantiate (rightBullet, firePos.position, Quaternion.identity);
			}
			if (!facingRight) {
				Instantiate (leftBullet, firePos.position, Quaternion.identity);
			}
			GameStatus.nextFire = Time.time + GameStatus.fireRate;
		}
	}

	void  Jet() {
		if (Input.GetKeyDown(KeyCode.LeftAlt) && GameStatus.isJetPackPickedUp) {
			ActivateJetPackButtonPressed ();
		}
	}
	//Call this function when UI Button pressed
	public void ActivateJetPackButtonPressed() {
		if (!GameStatus.isAltButtonPressed) {
			GameStatus.isJetLeftButtonPressed = false;
			GameStatus.isJetDownButtonPressed = false;
			GameStatus.isJetRightButtonPressed = false;
			GameStatus.isJetUpButtonPressed = false;
			speed = 0;
			anim.SetInteger ("State", 99); //Jet mode
			rb.angularVelocity = 0;
			rb.velocity = Vector3.zero; //magic line :) Stops player
			GameStatus.isAltButtonPressed = true;
			GameStatus.isJetPackOn = true;
			audio.Stop ();
			if (GameStatus.fillAmount > 0) {
				audio.PlayOneShot (jet_IsOn_Clip, 0.3f);
			}
			rb.gravityScale = 0;

		} else {
			speed = 0;
			rb.angularVelocity = 0;
			rb.velocity = Vector3.zero;
			anim.SetInteger ("State", 0);
			GameStatus.isAltButtonPressed = false;
			GameStatus.isJetPackOn = false;
			rb.gravityScale = 1;
		}
	}
	//Jet Go Up
	public void JetPackGoUpButtonPressed() {
		GameStatus.isJetUpButtonPressed = true;
	}
	public void JetPackGoUpButtonReleased() {
		GameStatus.isJetUpButtonPressed = false;
	}
	//Jet Go Down
	public void JetPackGoDownButtonPressed() {
		GameStatus.isJetDownButtonPressed = true;
	}
	public void JetPackGoDownButtonReleased() {
		GameStatus.isJetDownButtonPressed = false;
	}
	//Jet Go Left
	public void JetPackGoLeftButtonPressed() {
		GameStatus.isJetLeftButtonPressed = true;
	}
	public void JetPackGoLeftButtonReleased() {
		GameStatus.isJetLeftButtonPressed = false;
	}
	//Jet Go Right
	public void JetPackGoRightButtonPressed() {
		GameStatus.isJetRightButtonPressed = true;
	}
	public void JetPackGoRightButtonReleased() {
		GameStatus.isJetRightButtonPressed = false;
	}
}