using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HexPlatform : NetworkBehaviour {

	enum State {
		Alive, Dying, Dead, Reviving
	}

	enum Effect {
		None, Plagued, Muddy, Icy
	}

	public Color defaultColor;

	State state = State.Alive;
	[SyncVar] Effect effect = Effect.None;
	float deathTimer = 0;
	float fadeTimer = 0;
	float effectTimer = 0;
	[SyncVar (hook = "OnColorChange")] public Color color;
	[SyncVar (hook = "OnColliderChange")] public bool colliderOn = true;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = this.GetComponentInChildren<Renderer> ();
		this.color = rend.material.GetColor("_Color");
		defaultColor = this.color;
	}

	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}
			
		if (state == State.Dying) {
			fadeTimer -= Time.deltaTime;

			Color tmp = rend.material.color;
			tmp.a = fadeTimer / ConstantManager.platformFadeTimer;

			this.color = tmp;
			rend.material.SetColor ("_Color", this.color);

			if (fadeTimer <= 0) {
				state = State.Dead;
				deathTimer = ConstantManager.platformDeathTimer;
			}
		} else if (state == State.Reviving) {
			fadeTimer -= Time.deltaTime;

			Color tmp = rend.material.color;
			tmp.a = 1 - fadeTimer / ConstantManager.platformFadeTimer;

			this.color = tmp;
			rend.material.SetColor ("_Color", this.color);

			if (fadeTimer <= 0) {
				state = State.Alive;
				this.colliderOn = true;
				this.GetComponentInChildren<Collider> ().enabled = true;
			}
		} else if (state == State.Dead) {
			deathTimer -= Time.deltaTime;

			if (deathTimer <= 0 && effect != Effect.Plagued) {
				state = State.Reviving;
				fadeTimer = ConstantManager.platformFadeTimer;
			}
		}

		if (effect == Effect.Muddy && (effectTimer -= Time.deltaTime) <= 0) {
			effect = Effect.None;
			rend.material.SetColor ("_Color", defaultColor);
			this.color = defaultColor;
		}
		else if (effect == Effect.Icy && (effectTimer -= Time.deltaTime) <= 0) {
			effect = Effect.None;
			rend.material.SetColor ("_Color", defaultColor);
			this.color = defaultColor;
		}
	}

	void OnColorChange(Color clr){
		this.color = clr;
		rend.material.SetColor ("_Color", color);
	}

	void OnColliderChange(bool collider){
		this.colliderOn = collider;
		this.GetComponentInChildren<Collider> ().enabled = this.colliderOn;
	}

	void OnCollisionEnter(Collision col){
		Transform otherObject = col.transform;

		if (otherObject.tag == ConstantManager.playerTag) {
			if (effect == Effect.Muddy)
				otherObject.GetComponent<CharacterProperties> ().modifySpeed (ConstantManager.muddySpeedModifier, ConstantManager.muddySlowLinger);
			else if (effect == Effect.Icy)
				otherObject.GetComponent<CharacterProperties> ().modifySpeed (ConstantManager.icySpeedModifier, ConstantManager.icySpeedLinger);
			return;
		}
		else if (otherObject.tag == ConstantManager.projectileTag) {
			// example; can depend on projectile type or whatever else
			killPlatform();
		}
	}

	void OnCollisionStay(Collision col){
		Transform otherObject = col.transform;

		if (otherObject.tag == ConstantManager.playerTag) {
			if (effect == Effect.Muddy)
				otherObject.GetComponent<CharacterProperties> ().modifySpeed (ConstantManager.muddySpeedModifier, ConstantManager.muddySlowLinger);
			else if (effect == Effect.Icy)
				otherObject.GetComponent<CharacterProperties> ().modifySpeed (ConstantManager.icySpeedModifier, ConstantManager.icySpeedLinger);
			return;
		}
	}
		
	public void killPlatform(){
		if (!isServer) {
			return;
		}

		if (state != State.Alive)
			return;
		
		state = State.Dying;
		fadeTimer = ConstantManager.platformFadeTimer;
		this.colliderOn = false;
		this.GetComponentInChildren<Collider> ().enabled = false;
	}

	public IEnumerator makePlagued(float timer){
		this.effect = Effect.Plagued;
		if (state != State.Alive)
			yield break;
		Color temp = ConstantManager.ToColor (ConstantManager.plaguedColor);
		rend.material.SetColor ("_Color", temp);
		this.color = temp;
		yield return new WaitForSeconds (timer);
		killPlatform ();
	}

	public void makeMuddy(){
		if (this.effect != Effect.None || this.state != State.Alive)
			return;

		this.effect = Effect.Muddy;
		effectTimer = ConstantManager.muddyDuration;

		Color temp = ConstantManager.ToColor (ConstantManager.muddyColor);
		rend.material.SetColor ("_Color", temp);
		this.color = temp;
	}

	public void makeIcy(){
		if (this.effect != Effect.None || this.state != State.Alive)
			return;

		this.effect = Effect.Icy;
		effectTimer = ConstantManager.icyDuration;

		Color temp = ConstantManager.ToColor (ConstantManager.icyColor);
		rend.material.SetColor ("_Color", temp);
		this.color = temp;
	}
}
