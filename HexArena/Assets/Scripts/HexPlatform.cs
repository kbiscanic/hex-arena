using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HexPlatform : NetworkBehaviour {

	enum State {
		Alive, Dying, Dead, Reviving
	}

	public Color plaguedColor = new Color(0.7f, 0, 0);

	State state = State.Alive;
	float deathTimer = 0;
	float fadeTimer = 0;
	bool permaDead = false;
	[SyncVar (hook = "OnColorChange")] public Color color;
	[SyncVar (hook = "OnColliderChange")] public bool colliderOn = true;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = this.GetComponentInChildren<Renderer> ();
		this.color = rend.material.GetColor("_Color");
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

			if (deathTimer <= 0 && !permaDead) {
				state = State.Reviving;
				fadeTimer = ConstantManager.platformFadeTimer;
			}
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
		if (!isServer) {
			return;
		}

		Transform otherObject = col.transform;

		if (otherObject.tag == ConstantManager.playerTag)
			return;
		
		if (otherObject.tag == ConstantManager.projectileTag) {
			// example; can depend on projectile type or whatever else
			killPlatform();
		}
	}

	public void killPlatform(){
		if (state != State.Alive)
			return;
		
		state = State.Dying;
		fadeTimer = ConstantManager.platformFadeTimer;
		this.colliderOn = false;
		this.GetComponentInChildren<Collider> ().enabled = false;
	}

	public IEnumerator killPlatformAfter(float timer){
		permaDead = true;
		if (state != State.Alive)
			yield break;
		rend.material.SetColor ("_Color", plaguedColor);
		this.color = plaguedColor;
		yield return new WaitForSeconds (timer);
		killPlatform ();
	}

}
