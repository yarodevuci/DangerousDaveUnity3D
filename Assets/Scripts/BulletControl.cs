using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {

	public Vector2 bulletSpeed;
	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = bulletSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = bulletSpeed;
	}

	void OnTriggerEnter2D(Collider2D c) {

		if (c.gameObject.tag == "wall" || c.gameObject.tag == "ground") {
			Destroy (gameObject);
		}
	}
		
}
