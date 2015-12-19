using UnityEngine;
using System.Collections;

public class DeathCollider : MonoBehaviour {

	public string playerDeathScene = "GameOverScene";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator killPlayer(GameObject player){
		Camera characterCamera = player.GetComponentInChildren<Camera> ();
		if (characterCamera != null && characterCamera.enabled) {
			characterCamera.enabled = false;
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>().enabled = true;
		}

		yield return new WaitForSeconds (3);

		Application.LoadLevel (playerDeathScene);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == ConstantManager.playerTag) {
			StartCoroutine (killPlayer (other.gameObject));
		}

		if (other.tag == ConstantManager.projectileTag) {
			Destroy(other.gameObject);
		}
	}
}
