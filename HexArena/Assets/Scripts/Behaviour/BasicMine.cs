using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BasicMine : NetworkBehaviour {

	public float tickingDuration = 3;
	public float explosionDuration;
	public float explosionStepCount;
	public float explosionMaxRadius;
	float timer = 0;
	bool exploded = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		timer += Time.deltaTime;
		if (timer >= tickingDuration) {
			StartCoroutine (explode ());
		}
	}

	void OnTriggerEnter(Collider other){
		if (!isServer) {
			return;
		}

		if (!exploded)
			return;
	
		if (other.tag == ConstantManager.platformTag)
			other.GetComponent<HexPlatform> ().killPlatform ();
	}

	IEnumerator explode(){
		exploded = true;

		this.GetComponent<Rigidbody> ().useGravity = false;
		SphereCollider collider = this.GetComponent<SphereCollider> ();
		collider.isTrigger = true;

		float step = (explosionMaxRadius - collider.radius) / explosionStepCount;
		while (collider.radius <= explosionMaxRadius) {
			collider.radius += step;
			yield return new WaitForSeconds (explosionDuration / explosionStepCount);
		}

		Destroy (this.gameObject);
	}
}
