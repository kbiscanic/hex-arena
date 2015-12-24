using UnityEngine;
using System.Collections;

public class ConstantManager : MonoBehaviour {

	public const int platformRowCount = 10;
	public const int platformColCount = 10;

	// object tags
	public const string playerTag = "Player";
	public const string enemyTag = "Enemy";
	public const string projectileTag = "Projectile";
	public const string platformTag = "Platform";
	public const string platformGridTag = "Platform Grid";

	// duration constants
	public const float platformDeathTimer = 4.0f;
	public const float platformFadeTimer = 1.0f;

	// platform events
	public const int muddyColor = 0xFFFF00;
	public const float muddySpeedModifier = 0.25f;
	public const float muddySlowLinger = 0.3f;
	public const float muddyDuration = 9.98f;
	public const float muddySpawnInterval = 10f;
	public const int muddyCountPerTick = 3;

	public const int icyColor = 0x368BC1;
	public const float icySpeedModifier = 5f;
	public const float icySpeedLinger = 0.2f;
	public const float icyDuration = 9.98f;
	public const float icySpawnInterval = 10f;
	public const int icyCountPerTick = 2;

	public const int explosiveColor = 0xC80000;
	public const float explosiveDuration = 2f;
	public const int explosiveAdjacentCount = 6;


	public const int plaguedColor = 0xC80000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public Color ToColor(int HexVal) {
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color(R, G, B);
	}
}
