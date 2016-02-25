using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;

public class UnitCommander : MonoBehaviour, IGATPulseClient
{

    PulseModule pulse;
    public BeatSwitches[] switches;
    UnitScript.Strategies[] actionPatternZero = new UnitScript.Strategies[8];
    UnitScript.Strategies[] actionPatternOne = new UnitScript.Strategies[8];
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
            foreach (UnitScript u in MusicManager.units)
            {
                actionPatternZero = u.actionPattern;
                actionPatternOne = u.actionPattern;
            }
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
            foreach (UnitScript u in MusicManager.units)
            {
                actionPatternZero = u.actionPattern;
                actionPatternOne = u.actionPattern;
            }
            init = true;
        }

        foreach (UnitScript u in MusicManager.units)
        {
            switch (u.team)
            {
                case 0:
                    u.actionPattern = actionPatternZero;
                    
                    break;
                case 1:
                    break;
            }
        }
    }

    public void OnPulse(IGATPulseInfo pulseInfo)
    {
		/*
        foreach (BeatSwitches b in switches)
        {
            if (pulseInfo.StepIndex == b.beat)
            {
                b.beatImage.enabled = true;
            }
            else
            {
                b.beatImage.enabled = false;
            }
        }*/
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        throw new NotImplementedException();
    }

    void ActionChange(int team, int beatNum, UnitScript.Strategies newAction)
    {
        //Debug.Log(string.Format("Action Change\nTeam: {0}\nBeatNum: {1}\nAction: {2}",team,beatNum,newAction));
        switch (team)
        {
            case 0:
				for (int i = 0; i < 8; i++)
				{
					actionPatternZero[i] = newAction;
				}
                break;
            case 1:
                actionPatternOne[beatNum] = newAction;
                break;
        }
        
        foreach (UnitScript u in MusicManager.units)
        {
            switch (team)
            {
                case 0:
                    if (u.team == 0)
                    {
                        u.actionPattern = actionPatternZero;
                    }
                    break;
                case 1:
                    if (u.team == 1)
                    {
                        u.actionPattern = actionPatternOne;
                    }
                    break;
            }
        }
    }
}
