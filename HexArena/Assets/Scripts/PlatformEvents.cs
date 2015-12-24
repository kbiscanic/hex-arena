using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlatformEvents : NetworkBehaviour {
	[Tooltip("All platforms will be renamed to this")]
	public string platformGenericName = "HexPlatform"; // + platform index

	[Header("Skill constants")] // TODO move these fields to ConstantManager
	public float autoActivateSuddenDeath = 30f;
	public float spreadInterval = 2f;
	public int spreadCountPerTick = 2;
	public float platformDeathTime = 1f;

	GameObject[] platforms;
	bool spreadingDeath = false;
	float spreadTime = 0;
	float mudTime = 8f; // initial spawning offset
	float iceTime = 4f; // initial spawning offset
	int spreadCount = 0;

	// Use this for initialization
	void Start () {
		if (!isServer) {
			return;
		}
			platforms = GameObject.FindGameObjectsWithTag (ConstantManager.platformTag);
			for (int i = 0; i < platforms.Length; i++)
				platforms [i].name = platformGenericName + " " + i;

			print (platforms.Length + " platforms found in total. Renaming.");
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		if (!spreadingDeath && (autoActivateSuddenDeath -= Time.deltaTime) <= 0)
			activateDeathSpread (Vector3.zero);
		else if (spreadingDeath && (spreadTime += Time.deltaTime) >= spreadInterval) {
			spreadTime = 0;
			for (int i = spreadCount; i < spreadCount + spreadCountPerTick && i < platforms.Length; i++)
				StartCoroutine(platforms[i].GetComponent<HexPlatform>().makePlagued (platformDeathTime));
			spreadCount += spreadCountPerTick;
		}

		if ((mudTime += Time.deltaTime) >= ConstantManager.muddySpawnInterval) {
			mudTime = 0;
			for (int i = 0; i < ConstantManager.muddyCountPerTick; i++) {
				int platformIndex = Random.Range (0, platforms.Length);
				platforms [platformIndex].GetComponent<HexPlatform>().makeMuddy ();
			}
		}
		if ((iceTime += Time.deltaTime) >= ConstantManager.icySpawnInterval) {
			iceTime = 0;
			for (int i = 0; i < ConstantManager.icyCountPerTick; i++) {
				int platformIndex = Random.Range (0, platforms.Length);
				platforms [platformIndex].GetComponent<HexPlatform>().makeIcy ();
			}
		}
	}

	public void activateDeathSpread(Vector3 origin){
		spreadingDeath = true;
		spreadCount = 0;
		spreadTime = 0;

		List<GameObject> temp = new List<GameObject> ();
		foreach (GameObject go in platforms)
			temp.Add (go);
		temp.Sort(delegate(GameObject c1, GameObject c2){
			return Vector3.Distance(origin, c1.transform.position).CompareTo
				((Vector3.Distance(origin, c2.transform.position)));   
		});

		platforms = temp.ToArray ();
		if (platforms.Length > 0) {
			StartCoroutine(platforms [0].GetComponent<HexPlatform> ().makePlagued (platformDeathTime));
			spreadCount++;
		}
	}
}
