using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class DeathCollider : NetworkBehaviour
{

	public int playerDeathScene = 2;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator killPlayer (GameObject player)
	{
		Destroy (player);

		yield return new WaitForSeconds (3);

		SceneManager.LoadScene (playerDeathScene);
	}

	void OnTriggerEnter (Collider other)
	{
		if (!isServer) {
			return;
		}

		if (other.GetComponent<CharacterProperties> () != null) {
			other.GetComponent<CharacterProperties> ().modifyHealth (-100);
		}
	}
}
