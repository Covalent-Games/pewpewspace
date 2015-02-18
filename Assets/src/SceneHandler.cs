using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour {

	public GameObject Mission;
	public Canvas DialogueCanvas;
	BaseMission CurrentMission;
	public static List<ShipObject> PlayerShips = new List<ShipObject>();
	public static List<ShipObject> Enemies = new List<ShipObject>();
	public LayerMask TargetingLayerMask;

	public List<GameObject> Explosions;
	public List<GameObject> RewardUI;

	void Awake() {

		PlayerShips = new List<ShipObject>();
		Enemies = new List<ShipObject>();
	}

	// Use this for initialization
	void Start() {

		SpawnPlayer();
		CreateMission();
	}

	void SpawnPlayer() {

		// A value of 0 to 1 representing the left and right side of the screen respectively.
		float xPos = 0f;
		// Same as xPos but the bottom and top, respectively.
		float yPos = 0.15f;
		for (int playerNum = 0; playerNum < GameValues.NumberOfPlayers; playerNum++) {

			GameObject hudGO = GameObject.Find(string.Format("Player{0}HUD", playerNum + 1));
			hudGO.GetComponent<Canvas>().enabled = true;

			// Offset the player's starting position.
			xPos += 1f / (GameValues.NumberOfPlayers + 1);
			var viewpoint = new Vector3(xPos, yPos, Camera.main.transform.position.y);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(viewpoint);

			GameObject prefabToLoad = GameValues.Players[playerNum + 1].SelectedPrefab;
			if (prefabToLoad == null) {
				Debug.LogError("Prefab is null. Wa wa waaaaa");
			}
			GameObject newShipGO = (GameObject)Instantiate(prefabToLoad, spawnPosition, Quaternion.identity);

			ShipObject newShip = newShipGO.GetComponent<ShipObject>();

			newShip.SetupPlayer(playerNum + 1);
			newShip.AddContainers(SceneHandler.PlayerShips);

		}
	}

	void CreateMission() {

		GameObject mission = (GameObject)Instantiate(Mission);
		CurrentMission = mission.GetComponent<BaseMission>();
		CurrentMission.DialogueCanvas = DialogueCanvas;
		CurrentMission.DialogueText = DialogueCanvas.transform.GetComponentInChildren<Text>();
	}


	void Update() {

		if (CurrentMission.Ended) {
			foreach (ShipObject ship in PlayerShips) {
				if (Input.GetButtonDown(ship.PlayerObject.Controller.ButtonA)) {
					UnloadScene();
				}
			}
		}
	}

	void UnloadScene() {

		foreach (ShipObject ship in PlayerShips) {
			ship.HealthBar.fillAmount = 0;
			ship.DissipationBar.fillAmount = 0;
			//HACK? Canvas is 2 object up the hierarchy... this works but is super hacky.
			ship.transform.parent.parent.GetComponent<Canvas>().enabled = false;
		}
		Application.LoadLevel("GalaxyMenu");
	}

	public static void LoadScene(string SceneToLoad, bool ShowLoadingScreen = false, string LoadingScreen = "") {

		if (ShowLoadingScreen) {
			Debug.LogWarning("Loading screen not implemented!");
		}

		GameValues.PreviousScene = Application.loadedLevelName;
		Application.LoadLevel(SceneToLoad);
	}

	/// <summary>
	/// Cleanly loads the next scene.
	/// </summary>
	/// <param name="ShowLoadingScreen">Default: false. True to display loading screen.</param>
	/// <param name="LoadingScreen">Which loading screen to display. Only visible if ShowLoadingScreen
	/// is true</param>
	public static void LoadNextScene(bool ShowLoadingScreen = false, string LoadingScreen = "") {

		if (ShowLoadingScreen) {
			Debug.LogWarning("Loading screen not implemented!");
		}

		if (GameValues.NextScene != null) {
			GameValues.PreviousScene = Application.loadedLevelName;
			Application.LoadLevel(GameValues.NextScene);
			GameValues.NextScene = null;
		} else {
			Debug.LogError("GameValues.NextScene is not set. Try using SceneHandler.LoadScene().");
		}
	}

	/// <summary>
	/// Cleanly loads the previous scene.
	/// </summary>
	/// <param name="ShowLoadingScreen">Default: false. True to display loading screen.</param>
	/// <param name="LoadingScreen">Which loading screen to display. Only visible if ShowLoadingScreen
	/// is true</param>
	public static void LoadPreviousScene(bool ShowLoadingScreen = false, string LoadingScreen = "") {

		Debug.LogError("Not implemented yet.");
	}

}
