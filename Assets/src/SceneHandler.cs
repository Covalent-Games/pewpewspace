using UnityEngine;
using System.Collections;

public class SceneHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
		for(int i = 0; i < ships.Length; i++) {
			ships[i].GetComponent<ShipMovement>().enabled = true;
			ships[i].GetComponent<ShipAction>().enabled = true;
		}
		Screen.showCursor = false;
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Escape)) {
			MenuHandler menuHandler = GameObject.Find("MenuObject").GetComponent<MenuHandler>();
			menuHandler.OpenEscapeMenu();
		}
	}
}
