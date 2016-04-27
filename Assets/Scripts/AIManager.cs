using UnityEngine;
using System.Collections.Generic;

public static class AIManager
{
	const string BASS_AGGR_LIST = "BassAggressiveAI";
	const string BASS_DEF_LIST = "BassDefensiveAI";
	const string BASS_NEU_LIST = "BassNeutralAI";

	const string TREBLE_AGGR_LIST = "TrebleAggressiveAI";
	const string TREBLE_DEF_LIST = "TrebleDefensiveAI";
	const string TREBLE_NEU_LIST = "TrebleNeutralAI";

	const string ENEMY_BASS_AGGR_LIST = "EnemyBassAggressiveAI";
	const string ENEMY_BASS_DEF_LIST = "EnemyBassDefensiveAI";
	const string ENEMY_BASS_NEU_LIST = "EnemyBassNeutralAI";

	const string ENEMY_TREBLE_AGGR_LIST = "EnemyTrebleAggressiveAI";
	const string ENEMY_TREBLE_DEF_LIST = "EnemyTrebleDefensiveAI";
	const string ENEMY_TREBLE_NEU_LIST = "EnemyTrebleNeutralAI";

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

		public UnitAIs(string aggrKey, string defKey, string neuKey)
		{
			AggressiveAI = SaveUtil.LoadSafeList<ConditionalItem>(aggrKey);
			DefensiveAI = SaveUtil.LoadSafeList<ConditionalItem>(defKey);
			NeutralAI = SaveUtil.LoadSafeList<ConditionalItem>(neuKey);
		}
	}


	static UnitAIs bassUnit;
	static UnitAIs trebleUnit;
	static UnitAIs enemyBassUnit;
	static UnitAIs enemyTrebleUnit;

	public static UnitAIs BassUnit
	{
		get
		{
			if (bassUnit == null)
			{
				bassUnit = new UnitAIs(BASS_AGGR_LIST, BASS_DEF_LIST, BASS_NEU_LIST);
			}
			return bassUnit;
		}
		set
		{
			bassUnit = value;
		}
	}
	public static UnitAIs TrebleUnit
	{
		get
		{
			if (trebleUnit == null)
			{
				trebleUnit = new UnitAIs(TREBLE_AGGR_LIST, TREBLE_DEF_LIST, TREBLE_NEU_LIST);
			}
			return trebleUnit;
		}
		set
		{
			trebleUnit = value;
		}
	}

	public static UnitAIs EnemyBassUnit
	{
		get
		{
			if (enemyBassUnit == null)
			{
				enemyBassUnit = new UnitAIs(ENEMY_BASS_AGGR_LIST, ENEMY_BASS_DEF_LIST, ENEMY_BASS_NEU_LIST);
			}
			return enemyBassUnit;
		}
		set
		{
			enemyBassUnit = value;
		}
	}
	public static UnitAIs EnemyTrebleUnit
	{
		get
		{
			if (enemyTrebleUnit == null)
			{
				enemyTrebleUnit = new UnitAIs(ENEMY_TREBLE_AGGR_LIST, ENEMY_TREBLE_DEF_LIST, ENEMY_TREBLE_NEU_LIST);
			}
			return enemyTrebleUnit;
		}
		set
		{
			enemyTrebleUnit = value;
		}
	}
}
