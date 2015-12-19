using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour
{
	[SyncVar] private string playerUniqueId;
	private NetworkInstanceId playerNetId;
	private Transform myTransform;

	public override void OnStartLocalPlayer ()
	{
		tag = "Player";
		GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ().enabled = true;

		GetNetIdentity ();
		SetIdentity ();
	}

	void Awake ()
	{
		myTransform = transform;
	}

	void Update ()
	{
		if (myTransform.name == "Player(Clone)" || myTransform.name == "") {
			SetIdentity ();
		}
	}

	void SetIdentity ()
	{
		if (!isLocalPlayer) {
			myTransform.name = playerUniqueId;
		} else {
			myTransform.name = MakeUniqueId ();
		}
	}

	[Client]
	void GetNetIdentity ()
	{
		playerNetId = GetComponent<NetworkIdentity> ().netId;
		CmdSendIdToServer (MakeUniqueId ());
	}

	string MakeUniqueId ()
	{
		string uniqueId = "Player " + playerNetId.ToString ();
		return uniqueId;
	}

	[Command]
	void CmdSendIdToServer (string id)
	{
		playerUniqueId = id;
	}

}
