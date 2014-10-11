using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	float verticalMove;
	float horizontalMove;
	public float moveSpeed;
	public float currentMovingSpeed;
	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		
		this.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
	}
	
	void MoveShip(){
	
		// Calculate total modifier
		float moveSpeedModifier = this.moveSpeed * Time.deltaTime;
		
		//Get input from player
		this.verticalMove = Input.GetAxis("Vertical") * moveSpeedModifier;
		this.horizontalMove = Input.GetAxis("Horizontal") * moveSpeedModifier;
		
		// Calculate vectors and move the ship
		Vector3 moveVector = new Vector3(this.horizontalMove, 0f, this.verticalMove);
		Vector3 clampedMoveVector = Vector3.ClampMagnitude(moveVector, moveSpeedModifier);
		currentMovingSpeed = new Vector3(clampedMoveVector.x, 0, clampedMoveVector.z).magnitude;
		Vector3 oldPosition = transform.position;
		transform.position += clampedMoveVector;
		
		// Clamp the player to the screen so they can't fly off out of view
		Vector3 positionToCamera = this.mainCamera.WorldToViewportPoint(transform.position);
		//TODO: Incorporate width of ship
		if (positionToCamera.x > 1f | positionToCamera.x < 0f){
			transform.position -= new Vector3(transform.position.x - oldPosition.x, 0f, 0f);
		} else if (positionToCamera.y > 1f | positionToCamera.y < 0){
			transform.position -= new Vector3(0f, 0f, transform.position.z - oldPosition.z);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		MoveShip();
	}
}
