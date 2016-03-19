using UnityEngine;
using System.Collections.Generic;

public static class AIManager
{
	public class UnitAIs
	{
		public List<ConditionalItem> AggressiveAI;
		public List<ConditionalItem> DefensiveAI;
		public List<ConditionalItem> NeutralAI;

		public UnitAIs()
		{
			AggressiveAI = new List<ConditionalItem>();
			DefensiveAI = new List<ConditionalItem>();
			NeutralAI = new List<ConditionalItem>();
		}
	}

	public static UnitAIs BassUnit = new UnitAIs();
	public static UnitAIs TrebleUnit = new UnitAIs();

	public static UnitAIs EnemyBassUnit = new UnitAIs();
	public static UnitAIs EnemyTrebleUnit = new UnitAIs();
}
