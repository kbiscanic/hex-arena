using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LaunchDirectProjectile : Action
{

	public GameObject projectilePrefab;

	GameObject player;

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
	}

	public override void Execute ()
	{
		if (currentCooldown <= 0/* && enemy != null*/) {
			Debug.Log (description + " cast.");

			CmdSpawnProjectile (player);

			currentCooldown = cooldown;
			//} else if (enemy == null) {
			//	Debug.Log (description + " no target.");
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	[ClientRpc]
	void RpcAnimate (string playerName, string triggerName)
	{
		Debug.Log ("Animate");
		GameObject.Find (playerName).GetComponent<Animator> ().SetTrigger (triggerName);
	}

	[Command]
	void CmdSpawnProjectile (GameObject player)
	{
		Transform spawnLocation = findFirstDescendantWithName (player.transform, "LeftHand"); // TODO modify?
		GameObject projectile = Instantiate (projectilePrefab, spawnLocation.position, Quaternion.identity) as GameObject;
		projectile.transform.SetParent (spawnLocation);
		NetworkServer.Spawn (projectile);
		projectile.GetComponent<BasicProjectile> ().owner = player.name;
		StartCoroutine (launch (projectile, player));

		RpcAnimate (player.name, "CastDirectProjectile");

		Debug.Log (projectile);

		Destroy (projectile, 5.0f);
	}

	IEnumerator launch (GameObject projectile, GameObject player)
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
				movement.setDirection (player.transform.forward); // forward launch version
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
