using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour {

	public int maxHealth;
	[SerializeField]
	int health;
	public int Health {
		get { return this.health; }
		set {
			if (this.health + value <= 0){
				End();
			} else if (this.health + value > this.maxHealth){
				this.health = this.maxHealth;
			} else {
				this.health += value;
			}
		}
	}

	void Start () {
		
		this.health = this.maxHealth;
	}

	void End(){
		
		Destroy(gameObject);
	}	

	void Update () {
	
	}
}
