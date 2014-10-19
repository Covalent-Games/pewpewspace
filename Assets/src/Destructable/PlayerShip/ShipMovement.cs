using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	float verticalMove;
	float horizontalMove;
	[SerializeField]
	float rotation;
	public float moveSpeed;
	public float currentMovingSpeed;
	public float rotationSpeed;
	public Player player;


	void MoveShip(){
	
		Destructable ship = gameObject.GetComponent<Destructable>();

		// Calculate total modifier
		float moveSpeedModifier = this.moveSpeed * ship.Speed * Time.deltaTime;
		
		//Get input from player
		this.horizontalMove = Input.GetAxis(player.Controller.LeftStickX) * moveSpeedModifier;
		this.verticalMove = Input.GetAxis(player.Controller.LeftStickY) * moveSpeedModifier;
		
		// Calculate vectors and move the ship
		Vector3 moveVector = new Vector3(this.horizontalMove, 0f, this.verticalMove);
		Vector3 clampedMoveVector = Vector3.ClampMagnitude(moveVector, moveSpeedModifier);
		currentMovingSpeed = new Vector3(clampedMoveVector.x, 0, clampedMoveVector.z).magnitude;

		Vector3 oldPosition = transform.position;
		transform.position = oldPosition + clampedMoveVector;
		
		// Prevent players from moving off screen.
		ClampToScreen(oldPosition);
	}
	
	void RotateTurret(){
	
		float axisX = Input.GetAxis(player.Controller.RightStickX);
		float axisY = Input.GetAxis(player.Controller.RightSticky);
		// Modify the thumbstick sensitivity
		//TODO: I'm still not happy with how this controls. It works well, but *looks* jittery.
		// The sensitiy in Edit>Project Settings>Input might need to be adjusted.
		float positiveThreshold = InputCode.AxisThresholdPositive - 0.1f;
		float negativeThreshold = InputCode.AxisThresholdNegative + 0.1f;
	
		// If the player isn't touching the joystick.
		if (axisX > negativeThreshold && axisX < positiveThreshold &&
		    axisY > negativeThreshold && axisY < positiveThreshold) {
			axisX = 0f;
			axisY = 1f;
		}
	
		Transform turret = transform.FindChild("Turret");
		turret.LookAt(transform.position + new Vector3(axisX, 0, axisY));
	}
	
	void ClampToScreen(Vector3 oldPosition){
	
		Vector3 positionToCamera = Camera.main.WorldToViewportPoint(transform.position);

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
		RotateTurret();
	}
}
