using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;
using PuppetType = ConstFile.PuppetType;

public class UnitCommander : MonoBehaviour
{
    public BeatSwitches[] switches;
    UnitScript.Strategies[] actionPatternZeroBass = new UnitScript.Strategies[1];
	UnitScript.Strategies[] actionPatternZeroTreble = new UnitScript.Strategies[1];
	UnitScript.Strategies[] actionPatternOneBass = new UnitScript.Strategies[1] { UnitScript.Strategies.AGGRESSIVE };
	UnitScript.Strategies[] actionPatternOneTreble = new UnitScript.Strategies[1] { UnitScript.Strategies.AGGRESSIVE};
	//bool init = false;

    // Use this for initialization
    void Awake()
    {
        foreach (BeatSwitches b in switches)
        {
            b.SwitchChanged += ActionChange;
        }
		UpdateBassUnits(0);
		UpdateBassUnits(1);
		UpdateTrebleUnits(0);
		UpdateTrebleUnits(1);
		GameManager.AddUnit += NewUnit;
	}


    // Update is called once per frame
    void Update()
    {
	}

	void NewUnit(int team, GameObject unit, PuppetType type)
	{
		UpdateBassUnits(team);
		UpdateTrebleUnits(team);
	}

	void ActionChange(int team, int beatNum, UnitScript.Strategies newAction, PuppetType type)
	{
		//Debug.Log(string.Format("Action Change\nTeam: {0}\nBeatNum: {1}\nAction: {2}",team,beatNum,newAction));
		switch (team)
		{
			case 0:
				switch (type)
				{
					case PuppetType.BASS:
						actionPatternZeroBass[0] = newAction;
						break;
					case PuppetType.TREBLE:
						actionPatternZeroTreble[0] = newAction;
						break;
				}
				break;
			case 1:
				switch (type)
				{
					case PuppetType.BASS:
						actionPatternOneBass[0] = newAction;
						break;
					case PuppetType.TREBLE:
						actionPatternOneTreble[0] = newAction;
						break;
				}
				break;
		}
		switch (type)
		{
			case PuppetType.BASS:
				UpdateBassUnits(team);
				break;
			case PuppetType.TREBLE:
				UpdateTrebleUnits(team);
				break;
		}
	}

	void UpdateBassUnits(int team)
	{
		switch (team)
		{
			case 0:
				for (int i = 0; i < GameManager._LeftUnitBass.Count; i++)
				{
					if (GameManager._LeftUnitBass[i] != null)
					{
						GameManager._LeftUnitBass[i].UpdateActionPattern(actionPatternZeroBass[0]);
					}
					
				}
				break;
			case 1:
				for (int i = 0; i < GameManager._RightUnitBass.Count; i++)
				{
					if (GameManager._RightUnitBass[i] != null)
					{
						GameManager._RightUnitBass[i].UpdateActionPattern(actionPatternOneBass[0]);
					}
					
				}
				break;
		}
	}

	void UpdateTrebleUnits(int team)
	{
		switch (team)
		{
			case 0:
				for (int i = 0; i < GameManager._LeftUnitTreble.Count; i++)
				{
					UnitScript u = GameManager._LeftUnitTreble[i];
					u.UpdateActionPattern(actionPatternZeroTreble[0]);
				}
				break;
			case 1:
				for (int i = 0; i < GameManager._RightTreble.Count; i++)
				{
					if (GameManager._RightUnitTreble[i] != null)
					{
						GameManager._RightUnitTreble[i].UpdateActionPattern(actionPatternOneTreble[0]);
					}
				}
				break;
		}
	}
}
