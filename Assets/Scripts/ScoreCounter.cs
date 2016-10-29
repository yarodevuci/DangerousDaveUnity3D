using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScoreCounter : MonoBehaviour {

	public int score = 0;
	public Text scoreLabel;

	// Use this for initialization
	void Start () {
		scoreLabel.text = GameStatus.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.tag == "Player") {
			Destroy (gameObject);
			GameStatus.score += score;
			scoreLabel.text = GameStatus.score.ToString ();
		}
	}
}
