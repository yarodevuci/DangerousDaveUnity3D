using UnityEngine;
using System.Collections;

public class Ai_Attack : MonoBehaviour {

	public GameObject enemy_Bullet;
	public GameObject enemy_Explosion;
	public GameObject monster;
	public AudioClip enemyKillSoundClip;
	public bool allowToShoot;
	private GameObject instantiateExplosion;

	Transform firePos;
	Transform enemyPosition;

	// Use this for initialization
	void Start () {
		if (GameStatus.currentLevel == 3) {
			GameStatus.allowToFire = false;
		} else {
			GameStatus.allowToFire = true;
		}
		GameStatus.isEnemyFrozen = false;
		firePos = transform.FindChild ("Fire_Position");
	}
	
	// Update is called once per frame
	void Update () {
		enemyPosition = transform.FindChild ("Enemy_Position");
		if (GameStatus.isRichedTrigger) {
			allowToShoot = true;
		}

//		if (Time.time > nextFire && GameStatus.allowToFire) {
//			Fire ();
//			nextFire = Time.time + 0.3f;//Random.Range(1f, 2.0f);
//		}
	}

	void Fire() {
		Instantiate (enemy_Bullet, firePos.position, Quaternion.identity);
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.tag == "fire" && GameStatus.allowToFire && allowToShoot) {
			Fire ();
		}

		if (c.gameObject.tag == "player_bullet") {
			Destroy (c.gameObject);
			GameStatus.isEnemyFrozen = true;
			playExplosion ();
		}
	}

	void enableEnemyMoves() {
		GameStatus.isEnemyFrozen = false;
	}

	void hideEnemy() {
		monster.SetActive (false);
	}
		
	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag == "Player") {
			GameStatus.isEnemyFrozen = true;
			playExplosion ();
		}
	}

	void playExplosion() {
		GetComponent<AudioSource> ().Stop ();
		GetComponent<AudioSource> ().PlayOneShot (enemyKillSoundClip, 1);
		monster.transform.localScale = new Vector3 (0.1f, 0.1f, 0); //hides monster behind explosion
		instantiateExplosion = (GameObject) Instantiate (enemy_Explosion, enemyPosition.position, Quaternion.identity);
		//monster.SetActive (false); //Hide Spider
		Invoke("enableEnemyMoves", 2.2f);
		Invoke("hideEnemy", 1.8f);
		//Destroy (gameObject, 1.5f);
		Destroy (instantiateExplosion, 2);
	}
}
