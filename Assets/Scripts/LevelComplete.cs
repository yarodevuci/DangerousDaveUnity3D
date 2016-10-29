using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelComplete : MonoBehaviour {


	public float speed;
	public AudioClip win;

	public Text scoreText;
	public Text currentLevel;
	public Text congratMessage;

	//Lives:
	public GameObject live1;
	public GameObject live2;
	public GameObject live3;

	Rigidbody2D rb;
	Animator anim;
	new AudioSource audio;

	 //Use this for initialization
	void Start () {
		currentLevel.text = "LEVEL 0" + GameStatus.currentLevel.ToString();
		if (GameStatus.currentLevel == 4) {
			congratMessage.text = "YES! YOU FINISHED THE GAME!";
		} else if (GameStatus.currentLevel == 3) {
			congratMessage.text = "THIS IS THE LAST LEVEL!!!";
		} else {
			congratMessage.text = "GOOD WORK! ONLY " + (4 - GameStatus.currentLevel).ToString () + " MORE TO GO!";
		}
		//Advance level #
		GameStatus.currentLevel += 1;
		//No gun at new level
		GameStatus.isGunPickedUp = false;
		//Refill jet fuel
		GameStatus.fillAmount = 1; 
		GameStatus.isJetPackOn = false; //remove jet pack from players
		GameStatus.isJetPackPickedUp = false; //no jet pack
		GameStatus.isAltButtonPressed = false;
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
		Invoke("PlayAudioClip", 1.0f);
		scoreText.text = GameStatus.score.ToString ();

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
	}

	
	// Update is called once per frame
	void Update () {
		anim.SetInteger ("State", 2);
		rb.velocity = new Vector3 (speed, rb.velocity.y, 0);
	}


	void PlayAudioClip() {
		audio.clip = win;
		audio.loop = true;
		audio.Play();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "door") {
			if (GameStatus.currentLevel == 5) {
				GameStatus.score = 0;
				GameStatus.lives = 3;
				GameStatus.currentLevel = 1;
				SceneManager.LoadScene ("Main_Menu");
			} 
			else {
				SceneManager.LoadScene ("Level" + GameStatus.currentLevel);
		  }
		}
	}
}
