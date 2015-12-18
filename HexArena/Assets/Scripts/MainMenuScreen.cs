using UnityEngine;
using System.Collections;

public class MainMenuScreen : MonoBehaviour {

	public string gameSceneName = "GameScene";

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void newGame(){
		Application.LoadLevel(gameSceneName);
	}

	public void quitGame(){
		Application.Quit();
	}
}
