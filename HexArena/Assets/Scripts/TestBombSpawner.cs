using UnityEngine;
using System.Collections;

public class TestBombSpawner : MonoBehaviour {

	public GameObject bombPrefab;
	[Range(0.5f, 10)]
	public float spawnDelay = 2.0f;
	[Range(0, 25)]
	public float xSpread = 10;
	[Range(0, 25)]
	public float zSpread = 10;

	float timeElapsed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= spawnDelay) {
			timeElapsed = 0;
			Vector3 relativePos = new Vector3 (Random.Range (-xSpread / 2, xSpread / 2), 0, Random.Range (-zSpread / 2, zSpread / 2));
			Instantiate (bombPrefab, this.transform.position + relativePos, Quaternion.identity);
		}
	}
}
