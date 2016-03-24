using UnityEngine;
using System.Collections;

[System.Serializable]
public static class ConstFile
{
	public enum AIModes
	{
		AGGRESSIVE,
		DEFENSIVE,
		NEUTRAL,
	}

	public enum ConditionOptions : int
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

	public enum Notes : int
	{
		HALF = 0,
		QUARTER = 1,
		EIGHTH = 2,
		SIXTEENTH = 3,
		DOTTED_QUARTER = 4,
		DOTTED_EIGTH = 5,
		DOTTED_SIXTEENTH = 6,
	}
	// Based on data from http://bradthemad.org/guitar/tempo_explanation.php 
	public static float[] NoteBPMCalcs = new float[] { 120, 60, 30, 15, 90, 45, 22.5f};
}
