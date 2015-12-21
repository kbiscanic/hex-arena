using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public Vector3 speed;
	[HideInInspector]
	public Vector3 target;

	protected Vector3 direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setTarget(Vector3 _target){
		target = _target;
		direction = (target - this.transform.position).normalized;
	}
}
