using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour {

	public int attackDamage = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == ConstantManager.enemyTag)
			other.GetComponent<CharacterActions> ().modifyHealth (-attackDamage);
			
		if (other.tag != ConstantManager.playerTag)
			Destroy (this.gameObject);
	}
}
