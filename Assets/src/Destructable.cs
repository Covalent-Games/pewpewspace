using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour {

	public int maxHealth;
	[SerializeField]
	int health;
	public int Health {
		get { 
			return this.health;
			}
		set {
			this.health = value;

			if (this.health <= 0){
				End();
			} else if (this.health > this.maxHealth){
				this.health = this.maxHealth;
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
