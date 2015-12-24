using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BasicProjectile : NetworkBehaviour
{

	public int attackDamage = 1;

	public string owner = "";

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (!isServer || other.name == owner) {
			return;
		}

		if (other.GetComponent<CharacterActions> () != null) {
			other.GetComponent<CharacterActions> ().modifyHealth (-attackDamage);
		}
			
		// if (other.tag != ConstantManager.playerTag)
		Destroy (this.gameObject);
	}
}
