using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{

	void Start (){
		OnLevelWasLoaded (0);
	}

	public void StartupHost ()
	{
		SetPort ();
		StartHost ();
	}

	public void JoinGame ()
	{
		SetIpAddress ();
		SetPort ();
		StartClient ();
	}

	public void Disconnect ()
	{
		StopHost ();
		Destroy (gameObject);
	}

	void SetIpAddress ()
	{
		string ipAddress = GameObject.Find ("InputFieldIPAddress").transform.FindChild ("IPAddressText").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort ()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded (int level) {
		if (level == 0) {
			SetupMenuSceneButtons ();
		} else {
			SetupSceneButtons ();
		}
	}

	void SetupMenuSceneButtons()
	{
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.AddListener(StartupHost);

		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.AddListener(JoinGame);
	}

	void SetupSceneButtons()
	{
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.AddListener(Disconnect);
	}
}
