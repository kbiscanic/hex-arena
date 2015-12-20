using UnityEngine;
using UnityEngine.UI;

public class Action : MonoBehaviour
{

	public float cooldown = 0f;
	[HideInInspector]
	public float currentCooldown = 0f;
	public string description;
	public Image cdIcon;
	public KeyCode key;

	public void Execute ()
	{
		if (currentCooldown <= 10e-6) {
			// nesto poput player.actions[desc].exec() ?
			Debug.Log (description + " cast.");

			currentCooldown = cooldown;
		} else {
			Debug.Log (description + " on cooldown.");
		}
	}

	void Update ()
	{
		if (currentCooldown > 0 && cooldown > 0) {
			currentCooldown -= Time.deltaTime;
			cdIcon.fillAmount = 1 - currentCooldown / cooldown;
		}
	}
}
