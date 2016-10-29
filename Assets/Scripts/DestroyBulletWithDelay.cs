using UnityEngine;
using System.Collections;

public class DestroyBulletWithDelay : MonoBehaviour {

	public float delay;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, delay);
	}
}
