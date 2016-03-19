using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AISetupManager : MonoBehaviour
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

	public UnitAIManager Bass;
	public UnitAIManager Treble;

	public UnitAIManager EnemyBass;
	public UnitAIManager EnemyTreble;


	void Awake()
	{
		Bass.AggressiveList = SaveUtil.LoadList<ConditionalItem>(BASS_AGGR_LIST);
		Bass.NeutralList = SaveUtil.LoadList<ConditionalItem>(BASS_NEU_LIST);
		Bass.DefensiveList = SaveUtil.LoadList<ConditionalItem>(BASS_DEF_LIST);

		Treble.AggressiveList = SaveUtil.LoadList<ConditionalItem>(TREBLE_AGGR_LIST);
		Treble.NeutralList = SaveUtil.LoadList<ConditionalItem>(TREBLE_NEU_LIST);
		Treble.DefensiveList = SaveUtil.LoadList<ConditionalItem>(TREBLE_DEF_LIST);

		EnemyBass.AggressiveList = SaveUtil.LoadList<ConditionalItem>(ENEMY_BASS_AGGR_LIST);
		EnemyBass.NeutralList = SaveUtil.LoadList<ConditionalItem>(ENEMY_BASS_NEU_LIST);
		EnemyBass.DefensiveList = SaveUtil.LoadList<ConditionalItem>(ENEMY_BASS_DEF_LIST);

		EnemyTreble.AggressiveList = SaveUtil.LoadList<ConditionalItem>(ENEMY_TREBLE_AGGR_LIST);
		EnemyTreble.NeutralList = SaveUtil.LoadList<ConditionalItem>(ENEMY_TREBLE_NEU_LIST);
		EnemyTreble.DefensiveList = SaveUtil.LoadList<ConditionalItem>(ENEMY_TREBLE_DEF_LIST);
	}

	public void StartGame()
	{
		Bass.ActivateAggressive();
		Bass.ActivateDefensive();
		Bass.ActivateNeutral();

		EnemyBass.ActivateAggressive();
		EnemyBass.ActivateDefensive();
		EnemyBass.ActivateNeutral();

		Treble.ActivateAggressive();
		Treble.ActivateDefensive();
		Treble.ActivateNeutral();

		EnemyTreble.ActivateAggressive();
		EnemyTreble.ActivateDefensive();
		EnemyTreble.ActivateNeutral();

		AIManager.BassUnit.AggressiveAI = Bass.AggressiveList;
		AIManager.BassUnit.DefensiveAI = Bass.DefensiveList;
		AIManager.BassUnit.NeutralAI = Bass.NeutralList;

		AIManager.TrebleUnit.AggressiveAI = Treble.AggressiveList;
		AIManager.TrebleUnit.DefensiveAI = Treble.DefensiveList;
		AIManager.TrebleUnit.NeutralAI = Treble.NeutralList;

		SaveUtil.SaveList(Bass.AggressiveList, BASS_AGGR_LIST);
		SaveUtil.SaveList(Bass.DefensiveList, BASS_DEF_LIST);
		SaveUtil.SaveList(Bass.NeutralList, BASS_NEU_LIST);

		SaveUtil.SaveList(Treble.AggressiveList, TREBLE_AGGR_LIST);
		SaveUtil.SaveList(Treble.DefensiveList, TREBLE_DEF_LIST);
		SaveUtil.SaveList(Treble.NeutralList, TREBLE_NEU_LIST);

		AIManager.EnemyBassUnit.AggressiveAI = EnemyBass.AggressiveList;
		AIManager.EnemyBassUnit.DefensiveAI = EnemyBass.DefensiveList;
		AIManager.EnemyBassUnit.NeutralAI = EnemyBass.NeutralList;

		AIManager.EnemyTrebleUnit.AggressiveAI = EnemyTreble.AggressiveList;
		AIManager.EnemyTrebleUnit.DefensiveAI = EnemyTreble.DefensiveList;
		AIManager.EnemyTrebleUnit.NeutralAI = EnemyTreble.NeutralList;

		SaveUtil.SaveList(EnemyBass.AggressiveList, ENEMY_BASS_AGGR_LIST);
		SaveUtil.SaveList(EnemyBass.DefensiveList, ENEMY_BASS_DEF_LIST);
		SaveUtil.SaveList(EnemyBass.NeutralList, ENEMY_BASS_NEU_LIST);

		SaveUtil.SaveList(EnemyTreble.AggressiveList, ENEMY_TREBLE_AGGR_LIST);
		SaveUtil.SaveList(EnemyTreble.DefensiveList, ENEMY_TREBLE_DEF_LIST);
		SaveUtil.SaveList(EnemyTreble.NeutralList, ENEMY_TREBLE_NEU_LIST);


		SceneManager.LoadScene("Test");
	}
}
