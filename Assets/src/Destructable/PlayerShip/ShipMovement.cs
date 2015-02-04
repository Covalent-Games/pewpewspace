using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	float VerticalMove;
	float HorizontalMove;
	[SerializeField]
	float Rotation;
	Vector2 PreviousRotation;
	public bool AimingTurret;
	public float moveSpeed;
	public bool moveEnabled = true;
	public float currentMovingSpeed;
	public float rotationSpeed;
	public Player player;


	void MoveShip() {

		if (!moveEnabled) { return; }

		Destructible ship = gameObject.GetComponent<Destructible>();

		// Calculate total modifier
		float moveSpeedModifier = this.moveSpeed * ship.Speed * Time.deltaTime;

		//Get input from player
		this.HorizontalMove = Input.GetAxis(player.Controller.LeftStickX) * moveSpeedModifier;
		this.VerticalMove = Input.GetAxis(player.Controller.LeftStickY) * moveSpeedModifier;

		// Calculate vectors and move the ship
		Vector3 moveVector = new Vector3(this.HorizontalMove, 0f, this.VerticalMove);
		Vector3 clampedMoveVector = Vector3.ClampMagnitude(moveVector, moveSpeedModifier);
		currentMovingSpeed = new Vector3(clampedMoveVector.x, 0, clampedMoveVector.z).magnitude;

		Vector3 oldPosition = transform.position;
		transform.position = oldPosition + clampedMoveVector;

		// Prevent players from moving off screen.
		ClampToScreen(oldPosition);
	}

	public void MoveShip(Vector3 newMove) {

		Vector3 oldPosition = transform.position;
		transform.position = newMove;
		ClampToScreen(oldPosition);
	}

	void RotateTurret() {

		Vector2 thisRotation = new Vector2(Input.GetAxis(player.Controller.RightStickX), Input.GetAxis(player.Controller.RightSticky));
		ShipObject ship = GetComponent<ShipObject>();
		Transform turret = ship.Turret;

		thisRotation = Vector2.Lerp(PreviousRotation, thisRotation, Time.deltaTime * 7);

		if (ship.Target != null) {
			if (AbilityUtils.IsPlayer(ship.Target.GetComponent<ShipObject>())) {
				UpdateTargetingLine(turret, Vector3.Distance(transform.position, ship.Target.position), Color.green);
			} else {
				UpdateTargetingLine(turret, Vector3.Distance(transform.position, ship.Target.position), Color.red);
			}
		} else {
			// In this case the player has no target and is aiming to empty space, so give them a moderate line to aim with.
			UpdateTargetingLine(turret, 25f, Color.white);
		}

		if (turret != null) {
			if (thisRotation.magnitude < 0.55f) {
				AimingTurret = false;
				if (ship.Target != null) {
					turret.LookAt(ship.Target.position);
				}
			} else {
				AimingTurret = true;
				turret.LookAt(transform.position + new Vector3(thisRotation.x, 0, thisRotation.y));
			}
		}

		PreviousRotation = thisRotation;
	}

	private void UpdateTargetingLine(Transform turret, float length, Color color) {

		LineRenderer line = turret.GetComponent<LineRenderer>();

		line.SetPosition(0, new Vector3(0f, 0f, length));
		line.SetColors(Color.white, color);
	}


	void ClampToScreen(Vector3 oldPosition) {

		Vector3 positionToCamera = Camera.main.WorldToViewportPoint(transform.position);

		if (positionToCamera.x > 1f | positionToCamera.x < 0f) {
			transform.position -= new Vector3(transform.position.x - oldPosition.x, 0f, 0f);
		} else if (positionToCamera.y > 1f | positionToCamera.y < 0) {
			transform.position -= new Vector3(0f, 0f, transform.position.z - oldPosition.z);
		}
	}

	// Update is called once per frame
	void Update() {

		MoveShip();
		RotateTurret();
	}
}