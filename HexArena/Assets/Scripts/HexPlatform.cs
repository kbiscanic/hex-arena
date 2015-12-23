using UnityEngine;
using System.Collections;

public class HexPlatform : MonoBehaviour {

	enum State {
		Alive, Dying, Dead, Reviving
	}

	public Color plaguedColor = new Color(0.7f, 0, 0);

	State state = State.Alive;
	float deathTimer = 0;
	float fadeTimer = 0;
	bool permaDead = false;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = this.GetComponentInChildren<Renderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (state == State.Dying) {
			fadeTimer -= Time.deltaTime;

			Color tmpColor = rend.material.color;
			tmpColor.a = fadeTimer / ConstantManager.platformFadeTimer;
			rend.material.SetColor ("_Color", tmpColor);

			if (fadeTimer <= 0) {
				state = State.Dead;
				deathTimer = ConstantManager.platformDeathTimer;
			}
		} else if (state == State.Reviving) {
			fadeTimer -= Time.deltaTime;

			Color tmpColor = rend.material.color;
			tmpColor.a = 1 - fadeTimer / ConstantManager.platformFadeTimer;
			rend.material.SetColor ("_Color", tmpColor);

			if (fadeTimer <= 0) {
				state = State.Alive;
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

	void OnCollisionEnter(Collision col){
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
		this.GetComponentInChildren<Collider> ().enabled = false;
	}

	public IEnumerator killPlatformAfter(float timer){
		permaDead = true;
		if (state != State.Alive)
			yield break;
		rend.material.SetColor ("_Color", plaguedColor);
		yield return new WaitForSeconds (timer);
		killPlatform ();
	}

}
