using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	float velocity;
	public int damage;
	public Camera camera;

	// Use this for initialization
	void Start () {
	
		this.velocity = 25;
	}
	
	// Update is called once per frame
	void Update () {
	
		ErrorHandling();
	
		// TODO: Needs to work with all angles.
		transform.position += new Vector3(0f, 0f, this.velocity * Time.deltaTime);
		
		Vector3 positionToCamera = this.camera.WorldToViewportPoint(transform.position);
		
		if (positionToCamera.x > 1.5f | positionToCamera.x < -0.5f){
			Destroy(gameObject);
		} else if (positionToCamera.y > 1.5f | positionToCamera.y < -0.5){
			Destroy(gameObject);
		}
	}
	void ErrorHandling(){
	
		if (this.camera == null){
			Debug.LogError("No camera info set to projectile!");
		}
	}
	
	void OnTriggerEnter(Collider collider){
		
		Destructable destructable = collider.GetComponent<Destructable>();
		
		if (destructable != null){
			destructable.Health -= this.damage;
			Destroy(gameObject);
		}
	}
}
