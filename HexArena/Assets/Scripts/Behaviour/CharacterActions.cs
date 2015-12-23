using UnityEngine;
using System.Collections;

public class CharacterActions : MonoBehaviour {

	public int maxHealth = 1;
	int health;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public float getHealthPercent(){
		return health / maxHealth;
	}


	public void modifyHealth(int change){
		health += change;
		if (health <= 0) {
			killCharacter ();
		}
	}

	public void killCharacter(){
		// animate death
		Destroy(this.gameObject);
	}
}
