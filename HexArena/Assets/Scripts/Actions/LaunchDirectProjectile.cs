using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LaunchDirectProjectile : Action
{

	public GameObject projectilePrefab;

	GameObject player;
	//GameObject enemy;
	NetworkAnimator playerAnimator;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		if (projectilePrefab == null) {
			Debug.LogWarning (
				"The " + description + " ability is missing a projectile prefab.");
		}
	}
	
	// Update is called once per frame
	public override void Update ()
	{

		base.Update ();

		if (player == null)
			player = GameObject.FindGameObjectWithTag (ConstantManager.playerTag);
		//	if (enemy == null)
		//		enemy = GameObject.FindGameObjectWithTag (ConstantManager.enemyTag);
		if (playerAnimator == null && player != null)
			playerAnimator = player.GetComponent<NetworkAnimator> ();
	}

	public override void Execute ()
	{
		if (currentCooldown <= 0/* && enemy != null*/) {
			Debug.Log (description + " cast.");

			playerAnimator.SetTrigger ("CastDirectProjectile");

			CmdSpawnProjectile (player, player.transform.forward);

			currentCooldown = cooldown;
			//} else if (enemy == null) {
			//	Debug.Log (description + " no target.");
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	[Command]
	void CmdSpawnProjectile (GameObject player, Vector3 direction)
	{
		Transform spawnLocation = findFirstDescendantWithName (player.transform, "LeftHand"); // TODO modify?
		GameObject projectile = Instantiate (projectilePrefab, spawnLocation.position, Quaternion.identity) as GameObject;
		projectile.transform.SetParent (spawnLocation);
		NetworkServer.Spawn (projectile);
		projectile.GetComponent<BasicProjectile> ().owner = player.name;
		StartCoroutine (launch (projectile, direction));

		Debug.Log (projectile);

		Destroy (projectile, 5.0f);
	}

	IEnumerator launch (GameObject projectile, Vector3 direction)
	{
		yield return new WaitForSeconds (castingTime);
		if (projectile != null) {
			projectile.transform.SetParent (null);
			Movement movement = projectile.GetComponentInChildren<Movement> ();
			if (movement == null) {
				Debug.LogWarning (
					"The projectile prefab supplied for " + description + " is missing a movement script.");
			} else {
				//movement.setTarget (findFirstDescendantWithName (enemy.transform, "Neck").position); // enemy target version
				//movement.setTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10))); // mouse aim version
				movement.setDirection (direction); // forward launch version
			}
		}
	}

	// TODO move to utility or static or ...
	private Transform findFirstDescendantWithName (Transform current, string name)
	{
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
