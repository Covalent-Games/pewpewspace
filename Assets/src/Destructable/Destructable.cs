using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Destructable : MonoBehaviour {

	#region Members

	public List<ShipObject> Container = new List<ShipObject>();

	public int MaxArmor;
	public float MaxHeat;
	public float baseSpeed;
	
	[SerializeField]
	int armor;
	[SerializeField]
	float heat;
	public float Speed;
	public bool CanBeTargetted = true;
		
	[SerializeField]
	public bool Invulnerable = false;
	[SerializeField]
	public bool InvulnerableArmor = false;
	[SerializeField]
	public bool InvulnerableDissipation = false;
	
	
	#endregion
	
	#region Properties	
	public int Armor {
		get {
			return this.armor;
		}
		set {
			this.armor = value;
			if(this.armor <= 0) {
				End ();
			}
			if (this.armor > this.MaxArmor){
				this.armor = this.MaxArmor;
			}
		}
	}
	public float Heat {
		get {
			return this.heat;
		}
		set {
			this.heat = value;
			if (this.heat < 0){
				this.heat = 0f;
			}
		}
	}

	#endregion
	
	public int DamageArmor(int damage){

		if (!this.Invulnerable){
			this.Armor -= damage;

            DisplayFloatingDamage(damage);

			return this.Armor;
		}

		return this.Armor;
	}

	public int DamageArmor(int damage, ShipObject offender) {

		int armor = DamageArmor(damage);
		BaseShipAI ai = GetComponent<BaseShipAI>();
		if (ai != null) {
			if (ai.ThreatTable.ContainsKey(offender)) {
				ai.ThreatTable[offender] += damage;
			} else {
				ai.ThreatTable.Add(offender, damage);
			}
		}

		return armor;
	}

	public int RestoreArmor(int restoreAmount){
	
		this.Armor += restoreAmount;
		return this.Armor;
	}

	public float RestoreDissipation(float cooldown) {

		this.Heat -= cooldown;
		return this.Heat;
	}

	protected float DissipationCooldown() {

		float cooldown = this.MaxHeat / 10f * Time.deltaTime;
		return RestoreDissipation(cooldown);
	}


    private void DisplayFloatingDamage(int damage) {

        GameObject guiElement = (GameObject)Instantiate(Resources.Load("GUIPrefabs/FloatingDamage"), transform.position, Quaternion.identity);
        Transform textTransform = guiElement.transform.FindChild("Text");
        textTransform.GetComponent<Text>().text = damage.ToString();
    }

	void Start () {
		
		this.armor = this.MaxArmor;
	}

	public virtual void End(){
		
		//FIXME The comment below is a lie, and I don't currently know the fix. It's a rare bug. 'gameObject'
		// itself is a member of the physical GameObject and so referencing gameObject raises an error.
		// The object sometimes tries to be destroyed twice in one frame. This check prevents that.
		if (gameObject != null){
			Container.Remove(GetComponent<ShipObject>());
			var explosions = GameObject.FindObjectOfType<SceneHandler>().Explosions;
			var explosion = explosions[Random.Range(1, explosions.Count) - 1];
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	
	public void SetUpBaseAttributes(){
	
		this.Armor = this.MaxArmor;
		this.Heat = 0f;
		//this.Shields = this.maxShields;
		this.Speed = this.baseSpeed;
	}

	public void Update () {

		DissipationCooldown();
	}
}
