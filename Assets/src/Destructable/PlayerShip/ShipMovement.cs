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
	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		
		this.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
	}
	
	void MoveShip(){
	
		Destructable ship = gameObject.GetComponent<Destructable>();

		// Calculate total modifier
		float moveSpeedModifier = this.moveSpeed * ship.speed * Time.deltaTime;
		float rotationSpeedModifier = this.rotationSpeed * ship.speed * Time.deltaTime;
		
		//Get input from player
		this.verticalMove = Input.GetAxis(InputCode.Vertical) * moveSpeedModifier;
		this.horizontalMove = Input.GetAxis(InputCode.Horizontal) * moveSpeedModifier;
		this.rotation = Input.GetAxis(InputCode.AltHorizontal) * rotationSpeedModifier;
		
		// Calculate vectors and move the ship
		Vector3 moveVector = new Vector3(this.horizontalMove, 0f, this.verticalMove);
		Vector3 clampedMoveVector = Vector3.ClampMagnitude(moveVector, moveSpeedModifier);
		currentMovingSpeed = new Vector3(clampedMoveVector.x, 0, clampedMoveVector.z).magnitude;

		Vector3 oldPosition = transform.position;
		transform.position = oldPosition + clampedMoveVector;

		Vector3 currentRotation = transform.rotation.eulerAngles;
		Vector3 newRotation = currentRotation + new Vector3(0, this.rotation, 0);
		transform.rotation = Quaternion.Euler(newRotation);
		
		// Prevent players from moving off screen.
		ClampToScreen(oldPosition);
	}
	
	void ClampToScreen(Vector3 oldPosition){
	
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
