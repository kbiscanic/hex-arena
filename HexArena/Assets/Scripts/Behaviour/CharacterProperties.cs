using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterProperties : NetworkBehaviour
{

	public List<Action> actions;

	public Sprite[] hpImages;

	public int maxHealth = 1;
	[SyncVar (hook = "OnHealthChanged")] int health;

	public float speedModifier = 1;
	float speedModifierDuration;

	private Image playerHp;
	private Image enemyHp;
//	private Text playerHealth;
//	private Text enemyHealth;

	private GameOptionsManager gom; 
	private GameObject loserPanel;

	private float globalCooldown = 0.0f;

	private ThirdPersonCharacter tpc;

	// Use this for initialization
	void Start ()
	{
	//	playerHealth = GameObject.Find ("PlayerHealth").GetComponent<Text> ();
	//	enemyHealth = GameObject.Find ("EnemyHealth").GetComponent<Text> ();

		playerHp = GameObject.Find ("PlayerHP").GetComponent<Image> ();
		enemyHp = GameObject.Find ("EnemyHP").GetComponent<Image> ();

		gom = GameObject.Find ("_Game Options Manager_").GetComponent<GameOptionsManager> ();

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

		if (this.globalCooldown > 0) {
			this.globalCooldown -= Time.deltaTime;
		}

		if (tpc == null || !tpc.m_IsGrounded || tpc.m_Crouching || this.globalCooldown > 0) {
			return;
		}

		foreach(Action action in actions){
			if (Input.GetKeyDown (action.key)) {
				action.Execute ();
				this.globalCooldown += action.castingTime * 2;
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
	}

	public void killCharacter ()
	{
		Time.timeScale = 0.0f;
		if (isLocalPlayer) {
			gom.loserPanel.SetActive (true);
		} else {
			gom.winnerPanel.SetActive (true);
		}
		// animate death
		// Destroy (this.gameObject);
	}

	void OnHealthChanged (int hp)
	{
		health = hp;
		if (health <= 0) {
			health = 0;
			killCharacter ();
		}
		ShowHP ();
	}

	void ShowHP ()
	{
		//if (playerHealth == null || enemyHealth == null)
		//	return;

		if (isLocalPlayer) {
			playerHp.overrideSprite = hpImages[health];
		//	playerHealth.text = health.ToString () + "HP";
		} else {
			enemyHp.overrideSprite = hpImages[health];
			//enemyHealth.text = health.ToString () + "HP";
		}
	}
}
