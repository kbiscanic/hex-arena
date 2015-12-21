using UnityEngine;
using System.Collections;

public class LaunchDirectProjectile : Action {

	public GameObject projectilePrefab;

	GameObject player;
	GameObject enemy;

	// Use this for initialization
	void Start () {
		if (projectilePrefab == null) {
			Debug.LogWarning (
				"The " + description + " ability is missing a projectile prefab.");
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if (player == null) 
			player = GameObject.FindGameObjectWithTag (ConstantManager.playerTag);
		if (enemy == null)
			enemy = GameObject.FindGameObjectWithTag (ConstantManager.enemyTag);
	}

	public override void Execute(){
		if (currentCooldown <= 0 && enemy != null) {
			Debug.Log (description + " cast.");

			Transform spawnLocation = findFirstDescendantWithName (player.transform, "LeftHand"); // TODO modify?
			GameObject projectile = Instantiate (projectilePrefab, spawnLocation.position, Quaternion.identity) as GameObject;
			projectile.transform.SetParent (spawnLocation);
			StartCoroutine (launch (projectile));

			currentCooldown = cooldown;
		} else if (enemy == null) {
			Debug.Log (description + " no target.");
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	IEnumerator launch(GameObject projectile){
		yield return new WaitForSeconds (castingTime);
		projectile.transform.SetParent (null);
		Movement movement = projectile.GetComponentInChildren<Movement> ();
		if (movement == null) {
			Debug.LogWarning (
				"The projectile prefab supplied for " + description + " is missing a movement script.");
		} else {
			movement.setTarget(findFirstDescendantWithName(enemy.transform, "Neck").position); // TODO modify?
		}
	}

	// TODO move to utility or static or ...
	private Transform findFirstDescendantWithName(Transform current, string name){
		foreach (Transform child in current) {
			if (child.name.Contains (name)) {
				return child;
			}
		}

		foreach (Transform child in current) {
			Transform result = findFirstDescendantWithName (child, name);
			if (result != null)
				return result;
		}

		return null;
	}
}
