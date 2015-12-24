﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterProperties : NetworkBehaviour
{

	public List<Action> actions;

	public int maxHealth = 1;
	[SyncVar (hook = "OnHealthChanged")] int health;

	public float speedModifier = 1;
	float speedModifierDuration;

	private Text playerHealth;
	private Text enemyHealth;

	private ThirdPersonCharacter tpc;

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

		if ((speedModifierDuration -= Time.deltaTime) <= 0) {
			speedModifier = 1; // TODO
		}

		if (tpc == null) {
			tpc = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter> ();
		}

		if (tpc == null || !tpc.m_IsGrounded || tpc.m_Crouching) {
			return;
		}

		foreach(Action action in actions){
			if (Input.GetKeyDown (action.key)) {
				action.Execute ();
			}
		}
	}

	public void modifySpeed(float modifier, float duration){
		speedModifierDuration = duration;
		speedModifier = modifier;
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
