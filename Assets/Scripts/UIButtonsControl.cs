using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonsControl : MonoBehaviour {

	public GameObject jetPackUIButton;
	public GameObject jumpUIButton;
	public GameObject jetFlyUpUIButton;

	public GameObject jetFlyDownButton;
	public GameObject shootButton;

	public GameObject jetFlyLeftButton;
	public GameObject jetFlyRightButton;


	// Use this for initialization
	void Start () {
		jetFlyUpUIButton.SetActive (false);
		jetPackUIButton.SetActive (false);
		jetFlyDownButton.SetActive (false);
		shootButton.SetActive (false);

		jetFlyLeftButton.SetActive (false);
		jetFlyRightButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameStatus.isDeployedToMobile) {
			if (GameStatus.isGunPickedUp) {
				shootButton.SetActive (true);
			}
			//When UI Jet Button is Pressed
			if (GameStatus.isAltButtonPressed) {
				jumpUIButton.SetActive (false);
				jetFlyUpUIButton.SetActive (true);
				shootButton.SetActive (false);
				jetFlyDownButton.SetActive (true);
				jetFlyLeftButton.SetActive (true);
				jetFlyRightButton.SetActive (true);

			} //When UI Jet Button is Pressed again to deactivate Jet
			if (!GameStatus.isAltButtonPressed) {
				jumpUIButton.SetActive (true);
				jetFlyUpUIButton.SetActive (false);
				jetFlyDownButton.SetActive (false);
				jetFlyLeftButton.SetActive (false);
				jetFlyRightButton.SetActive (false);
			}

			if (GameStatus.isJetPackPickedUp) {
				jetPackUIButton.SetActive (true);
			}
			if (GameStatus.fillAmount <= 0) {
				jetPackUIButton.SetActive (false);
			}
		}
	}
}
