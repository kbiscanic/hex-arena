using UnityEngine;
using System.Collections;

public class DirectMovement : Movement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.Scale (direction, speed) * Time.deltaTime;
	}
}
