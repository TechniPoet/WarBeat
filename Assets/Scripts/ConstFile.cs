using UnityEngine;
using System.Collections;

public static class ConstFile
{
	public enum AIModes
	{
		AGGRESSIVE,
		DEFENSIVE,
		NEUTRAL,
	}

	public enum ConditionOptions
	{
		ENERGY = 0,
		ENEMY_DISTANCE = 1,
		VALUE = 2,
	}

	public enum Actions
	{
		ATTACK,
		MOVE_FORWARD,
		MOVE_ENEMY,
		MOVE_BACK,
		REST,
	}
}
