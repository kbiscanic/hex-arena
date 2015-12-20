using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionBar : MonoBehaviour {

	public List<Action> actions;

	void Update () {
		foreach(Action action in actions){
			if (Input.GetKeyDown (action.key)) {
					action.Execute ();
			}
		}
	}
}
