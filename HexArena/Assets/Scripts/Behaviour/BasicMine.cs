using UnityEngine;
using System.Collections;

public class BasicMine : MonoBehaviour {

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
		timer += Time.deltaTime;
		if (timer >= tickingDuration) {
			StartCoroutine (explode ());
		}
	}

	void OnTriggerEnter(Collider other){
		if (!exploded)
			return;
	
		if (other.tag == ConstantManager.platformTag)
			other.GetComponent<HexPlatform> ().killPlatform ();

		print ("DEBUG: Mine collided with " + other.name);
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
