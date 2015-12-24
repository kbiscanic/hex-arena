using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlantObject : Action {

	public GameObject objectPrefab;

	GameObject player;
	NetworkAnimator playerAnimator;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		if (objectPrefab == null) {
			Debug.LogWarning (
				"The " + description + " ability is missing an object prefab.");
		}
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if (player == null) 
			player = GameObject.FindGameObjectWithTag (ConstantManager.playerTag);
		if (playerAnimator == null && player != null)
			playerAnimator = player.GetComponent<NetworkAnimator> ();
	}

	public override void Execute(){
		if (currentCooldown <= 0) {
			Debug.Log (description + " cast.");

			playerAnimator.SetTrigger ("PlantObject");

			CmdPlant (player);

			currentCooldown = cooldown;
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	[Command]
	void CmdPlant(GameObject player){
		Transform spawnLocation = findFirstDescendantWithName (player.transform, "RightHand"); // TODO modify?

		GameObject plantedObject = Instantiate (objectPrefab, spawnLocation.position, Quaternion.identity) as GameObject;
		plantedObject.transform.SetParent (spawnLocation);

		NetworkServer.Spawn (plantedObject);
		StartCoroutine (plant (plantedObject));

		Destroy(plantedObject, 15.0f);
	}

	IEnumerator plant(GameObject plantedObject){
		yield return new WaitForSeconds (castingTime);
		if (plantedObject != null){
			plantedObject.transform.SetParent (null);
			plantedObject.GetComponent<Rigidbody> ().useGravity = true;
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
