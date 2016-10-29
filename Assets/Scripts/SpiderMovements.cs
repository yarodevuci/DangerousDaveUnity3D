using UnityEngine;
using System.Collections;

public class SpiderMovements : MonoBehaviour {

	public GameObject spider_Bullet;
	public GameObject spider_Explosion;
	public AudioClip enemykill;

	Rigidbody2D rb;

	private GameObject instantiateExplosion;

	Transform firePos;
	Transform enemyPosition;

	public float nextFire = 0.1f;

	//Specify maxX and maxY for Spider borders to move
	public float minX = 0;
	public float maxX = 0;
	public float minY = 0;
	public float maxY = 0;

	bool movingDown = true;
	Vector3 v3;

	// Use this for initialization
	void Start () {
		GameStatus.allowToFire = false;
		rb = GetComponent<Rigidbody2D>();
		firePos = transform.FindChild ("Spider_FirePostion");
		//All values are 0
		Vector3 v3 = rb.velocity;
	}


	// Update is called once per frames
	void Update () {
		enemyPosition = transform.FindChild ("Spider_Position");

		if (movingDown) {
			v3.y = Random.Range(2f, 6.0f);
			v3.x = Random.Range(2f, 8.0f);
			rb.velocity = v3;  //move spider right and up;
		}

		if (rb.position.x <= minX) {
			movingDown = false;
			v3.x = Random.Range(2f, 8.0f);
			rb.velocity = v3;
		}

		if (rb.position.x >= maxX) {
			movingDown = false;
			v3.x = Random.Range(-2f, -8.0f); //tell spider to go back
			rb.velocity = v3;
		}

		if (rb.position.y >= maxY) {
			movingDown = false;
			v3.y = Random.Range(-2f, -6.0f);
			rb.velocity = v3;
		}
		if (rb.position.y <= minY) {
			movingDown = false;
			v3.y = Random.Range(2f, 6.0f); //go up
			rb.velocity = v3;
		}

		if (Time.time > nextFire && GameStatus.allowToFire) {
			Fire ();
			nextFire = Time.time + Random.Range(1f, 3.0f);
		}
	}

	void Fire() {
		Instantiate (spider_Bullet, firePos.position, Quaternion.identity);
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Kill Monster if Player Shot at him
		if (other.gameObject.tag == "player_bullet") {
			movingDown = false;
			v3.y = 0;
			v3.x = 0;
			rb.velocity = v3;
			playExplosion ();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag == "Player") {
			playExplosion ();
		}
	}

	void playExplosion() {
		GetComponent<AudioSource> ().clip = enemykill;
		GetComponent<AudioSource>().Play();
		v3.y = 0;
		v3.x = 0;
		rb.velocity = v3;
		instantiateExplosion = (GameObject) Instantiate (spider_Explosion, enemyPosition.position, Quaternion.identity);
		Destroy (gameObject, 1.5f);
		Destroy (instantiateExplosion, 2);
	}
}
