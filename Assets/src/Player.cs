using UnityEngine;
using System.Collections;

public class Player {

	public InputCode Controller;
	public GameObject SelectedPrefab;
	
	public Player(int playerNumber){
	
		Controller = new InputCode(playerNumber);
		
	}
}
