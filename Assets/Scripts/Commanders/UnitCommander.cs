using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;

public class UnitCommander : MonoBehaviour, IGATPulseClient
{

    PulseModule pulse;
    public BeatSwitches[] switches;
    UnitScript.Strategies[] actionPatternZeroBass = new UnitScript.Strategies[1];
	UnitScript.Strategies[] actionPatternZeroTreble = new UnitScript.Strategies[1];
	UnitScript.Strategies[] actionPatternOneBass = new UnitScript.Strategies[1];
	UnitScript.Strategies[] actionPatternOneTreble = new UnitScript.Strategies[1];
	bool init = false;

    // Use this for initialization
    void Awake()
    {
        if (MusicManager._Pulse != null)
        {
            pulse = MusicManager._Pulse;
            pulse.SubscribeToPulse(this);
            foreach (BeatSwitches b in switches)
            {
                b.SwitchChanged += ActionChange;
            }
			UpdateBassUnits(0);
			UpdateBassUnits(1);
			UpdateTrebleUnits(0);
			UpdateTrebleUnits(1);
			init = true;
        }
        else
        {
            init = false;
        }
    }

    void OnDisable()
    {
        pulse.UnsubscribeToPulse(this);
    }


    // Update is called once per frame
    void Update()
    {
        if (!init && MusicManager._Pulse != null)
        {
            pulse = MusicManager._Pulse;
            pulse.SubscribeToPulse(this);
            foreach (BeatSwitches b in switches)
            {
                b.SwitchChanged += ActionChange;
            }
			UpdateBassUnits(0);
			UpdateBassUnits(1);
			UpdateTrebleUnits(0);
			UpdateTrebleUnits(1);
			init = true;
        }

		UpdateBassUnits(0);
		//UpdateBassUnits(1);
		UpdateTrebleUnits(0);
		//UpdateTrebleUnits(1);
	}

    public void OnPulse(IGATPulseInfo pulseInfo)
    {
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        throw new NotImplementedException();
    }

	void ActionChange(int team, int beatNum, UnitScript.Strategies newAction, UnitScript.UnitType type)
	{
		//Debug.Log(string.Format("Action Change\nTeam: {0}\nBeatNum: {1}\nAction: {2}",team,beatNum,newAction));
		switch (team)
		{
			case 0:
				switch (type)
				{
					case UnitScript.UnitType.BASS:
						actionPatternZeroBass[0] = newAction;
						break;
					case UnitScript.UnitType.TREBLE:
						actionPatternZeroTreble[0] = newAction;
						break;
				}
				break;
			case 1:
				switch (type)
				{
					case UnitScript.UnitType.BASS:
						actionPatternOneBass[0] = newAction;
						break;
					case UnitScript.UnitType.TREBLE:
						actionPatternOneTreble[0] = newAction;
						break;
				}
				break;
		}
		Debug.Log(string.Format("Team: {0} Type {1} Action {2}", team,type,newAction));
		switch (type)
		{
			case UnitScript.UnitType.BASS:
				UpdateBassUnits(team);
				break;
			case UnitScript.UnitType.TREBLE:
				UpdateTrebleUnits(team);
				break;
		}
	}

	void UpdateBassUnits(int team)
	{
		switch (team)
		{
			case 0:
				for (int i = 0; i < GameManager._LeftBass.Count; i++)
				{
					GameManager._LeftBass[i].GetComponent<UnitScript>().UpdateActionPattern(actionPatternZeroBass[0]);
				}
				break;
			case 1:
				for (int i = 0; i < GameManager._RightBass.Count; i++)
				{
					GameManager._RightBass[i].GetComponent<UnitScript>().UpdateActionPattern(actionPatternOneBass[0]);
				}
				break;
		}
	}

	void UpdateTrebleUnits(int team)
	{
		switch (team)
		{
			case 0:
				for (int i = 0; i < GameManager._LeftTreble.Count; i++)
				{
					UnitScript u = GameManager._LeftTreble[i].GetComponent<UnitScript>();
					u.UpdateActionPattern(actionPatternZeroTreble[0]);
				}
				break;
			case 1:
				for (int i = 0; i < GameManager._RightTreble.Count; i++)
				{
					GameManager._RightTreble[i].GetComponent<UnitScript>().UpdateActionPattern(actionPatternOneTreble[0]);
				}
				break;
		}
	}
}
