using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AspectScreen : MonoBehaviour {

	public int targerWidth = 640;
	public float pixelsToUnits = 100;

	// Use this for initialization
	void Update () {
		int height = Mathf.RoundToInt (targerWidth / (float)Screen.width * Screen.height);
		Camera.main.orthographicSize = height / pixelsToUnits / 2;
	}
}