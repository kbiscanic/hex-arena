using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onTriggerEnter(Collider other){
		if (other.tag != ConstantManager.playerTag)
			Destroy (this);
		print (other.name);
	}
}
