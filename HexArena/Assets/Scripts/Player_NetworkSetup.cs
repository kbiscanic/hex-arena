using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_NetworkSetup : NetworkBehaviour
{
	[SyncVar] private string playerUniqueId;
	private NetworkInstanceId playerNetId;
	private Transform myTransform;

	private NetworkClient netClient;
	private float latency;
	private Text latencyText;

	public override void OnStartLocalPlayer ()
	{
		//tag = "Player";
		GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ().enabled = true;

		GetNetIdentity ();
		SetIdentity ();

		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);
	}

	public override void PreStartClient ()
	{
		base.PreStartClient ();
		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);
	}

	void Awake ()
	{
		myTransform = transform;
	}

	void Start () 
	{
		netClient = GameObject.Find ("Network Manager").GetComponent<NetworkManager> ().client;
		latencyText = GameObject.Find ("LatencyText").GetComponent<Text> ();
	}

	void Update ()
	{
		if (myTransform.name.Contains("Clone") || myTransform.name == "") {
			SetIdentity ();
		}

		ShowLatency ();
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

	void ShowLatency() 
	{
		if (isLocalPlayer) {
			latency = netClient.GetRTT ();
			latencyText.text = latency.ToString () + "ms";
		}
	}
}
