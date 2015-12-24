using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterActions : NetworkBehaviour
{

	public List<Action> actions;

	public int maxHealth = 1;
	[SyncVar (hook = "OnHealthChanged")] int health;

	private Text playerHealth;
	private Text enemyHealth;

	// Use this for initialization
	void Start ()
	{
		playerHealth = GameObject.Find ("PlayerHealth").GetComponent<Text> ();
		enemyHealth = GameObject.Find ("EnemyHealth").GetComponent<Text> ();

		if (isServer) {
			health = maxHealth;
		}

		ShowHP ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		foreach(Action action in actions){
			if (Input.GetKeyDown (action.key)) {
				action.Execute ();
			}
		}
	}

	public float getHealthPercent ()
	{
		return health / maxHealth;
	}


	public void modifyHealth (int change)
	{
		health += change;
		if (health <= 0) {
			killCharacter ();
		}
		ShowHP ();
	}

	public void killCharacter ()
	{
		// animate death
		Destroy (this.gameObject);
	}

	void OnHealthChanged (int hp)
	{
		health = hp;
		ShowHP ();
	}

	void ShowHP ()
	{
		if (playerHealth == null || enemyHealth == null)
			return;

		if (isLocalPlayer) {
			playerHealth.text = health.ToString () + "HP";
		} else {
			enemyHealth.text = health.ToString () + "HP";
		}
	}
}
