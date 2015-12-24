using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Blink : Action {
	GameObject player;

	// Use this for initialization
	public override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if (player == null) 
			player = GameObject.FindGameObjectWithTag (ConstantManager.playerTag);
	}

	public override void Execute(){
		if (currentCooldown <= 0) {
			Debug.Log (description + " cast.");

			CmdBlink (player);

			currentCooldown = cooldown;
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	[ClientRpc]
	void RpcAnimate (string playerName, string triggerName) {
		Debug.Log ("Animate");
		GameObject.Find (playerName).GetComponent<Animator> ().SetTrigger (triggerName);
	}

	[Command]
	void CmdBlink (GameObject player) {
		RpcAnimate (player.name, "Blink");
		StartCoroutine (blinkPlayer ());
	}

	IEnumerator blinkPlayer(){
		yield return new WaitForSeconds (castingTime);
		player.transform.position += player.transform.forward * ConstantManager.blinkDistance;
	}
}
