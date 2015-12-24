using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathCollider : MonoBehaviour
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
		if (other.tag == ConstantManager.playerTag) {
			StartCoroutine (killPlayer (other.gameObject));
		} else if (other.tag == ConstantManager.enemyTag) {
			Destroy (other.gameObject);
		} else if (other.tag == ConstantManager.projectileTag) {
			Destroy (other.gameObject);
		}
	}
}
