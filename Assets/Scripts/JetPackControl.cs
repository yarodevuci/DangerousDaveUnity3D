using UnityEngine;
using System.Collections;

public class JetPackControl : MonoBehaviour {


	public AudioClip gotJetPack;
	public AudioClip jetPackUse;

	new AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameStatus.isJetPackOn) {
			audio.clip = jetPackUse;
			audio.Play ();
		} else if (!GameStatus.isJetPackOn) {
			audio.Stop();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag == "Player") {
			print ("Hello");
			audio.PlayOneShot(gotJetPack, 1);
		}
	}
}
