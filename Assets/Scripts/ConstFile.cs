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

	public static string[] ConditionOptionsText = new string[] { "Energy", "Enemy Distance", "Value" };

	public enum Actions
	{
		ATTACK = 0,
		MOVE_FORWARD = 1,
		MOVE_ENEMY = 2,
		MOVE_BACK = 3,
		REST = 4,
	}

	public static string[] ActionsText = new string[] { "Attack", "Move Forward", "Move to Enemy", "Move Back", "Rest" };
}
