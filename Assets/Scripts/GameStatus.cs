using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {

	///Turn this off for PC/Mac Build
	public static bool isDeployedToMobile = false;

	public static int score = 0;
	public static int currentLevel = 1;
	public static int lives = 3;
	///Jet Pack Turned on/off
	public static bool isJetPackOn = false;
	public static bool isJetPackPickedUp = false;
	public static bool isGunPickedUp = false;
	/// Amount of gas in JetPack.
	public static float fillAmount = 1;

	public static float fireRate = 0.5f; 
	public static float nextFire = 0.1f;

	public static bool allowToFire = false;

	//UIButtonsControls for JetPack
	public static bool isJetUpButtonPressed = false;
	public static bool isJetDownButtonPressed = false;

	public static bool isJetLeftButtonPressed = false;
	public static bool isJetRightButtonPressed = false;

	public static bool isAltButtonPressed = false;
	///Check if we on the ground to prevent double jump
	public static bool isGrounded = false;
	public static bool isEnemyFrozen = false;
	public static bool isRichedTrigger = false;

	//Game version 4.2 
	//Last changes 10/28/2016

	public void NewGameButtonPressed() {
		ApplySettingsForLevel (1);
	}

	public void GoToLevel_2() {
		ApplySettingsForLevel (2);
	}

	public void GoToLevel_3() {
		ApplySettingsForLevel (3);
	}

	public void GoToLevel_4() { 
		ApplySettingsForLevel (4);
	}

	public void GoToMainMenuScreen() { 
		SceneManager.LoadScene ("Main_Menu");
	}
		
	///apply settings for chosen level
	private void ApplySettingsForLevel(int level) {
		GameStatus.lives = 3;
		GameStatus.currentLevel = level;
		GameStatus.fillAmount = 1;
		GameStatus.isGunPickedUp = false;
		GameStatus.score = 0;
		SceneManager.LoadScene ("Level" + level.ToString());
	}

	public void GoToLevelSelectionScreen() {
		SceneManager.LoadScene ("LevelChoiceScreen");
	}
	/// Quit Game
	public void ExitGame() {
		Application.Quit();
	}



}
