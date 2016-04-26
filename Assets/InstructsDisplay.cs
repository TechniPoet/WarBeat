using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InstructsDisplay : MonoBehaviour {

	public Text treble;
	public Text bass;

	public UnitCommander commander;
	BeatSwitches[] switches;

	UnitScript.Strategies trebleStrat = UnitScript.Strategies.NEUTRAL;
	UnitScript.Strategies bassStrat = UnitScript.Strategies.NEUTRAL;

	void Start()
	{
		switches = commander.switches;
		foreach (BeatSwitches b in switches)
		{
			b.SwitchChanged += ChangeStrat;
		}
		UpdateTrebleInstructs(UnitScript.Strategies.NEUTRAL);
		UpdateBassInstructs(UnitScript.Strategies.NEUTRAL);
	}

	void ChangeStrat(int team, int beatNum, UnitScript.Strategies newAction, ConstFile.PuppetType type)
	{
		switch (type)
		{
			case ConstFile.PuppetType.TREBLE:
				UpdateTrebleInstructs(newAction);
				break;
			case ConstFile.PuppetType.BASS:
				UpdateBassInstructs(newAction);
				break;
		}
	}

	void UpdateTrebleInstructs(UnitScript.Strategies strat)
	{
		List<ConditionalItem> conditions = new List<ConditionalItem>();
		AIManager.UnitAIs u = AIManager.TrebleUnit;
		switch (strat)
		{
			case UnitScript.Strategies.AGGRESSIVE:
				conditions = u.AggressiveAI;
				break;
			case UnitScript.Strategies.DEFENSIVE:
				conditions = u.DefensiveAI;
				break;
			case UnitScript.Strategies.NEUTRAL:
				conditions = u.NeutralAI;
				break;
		}
		treble.text = "";
		foreach (ConditionalItem c in conditions)
		{
			treble.text += c.GetSentence() + "\n";
		}
		treble.text += "Else Rest for EIGHTH note";
	}
	void UpdateBassInstructs(UnitScript.Strategies strat)
	{
		List<ConditionalItem> conditions = new List<ConditionalItem>();
		AIManager.UnitAIs u = AIManager.BassUnit;
		switch (strat)
		{
			case UnitScript.Strategies.AGGRESSIVE:
				conditions = u.AggressiveAI;
				break;
			case UnitScript.Strategies.DEFENSIVE:
				conditions = u.DefensiveAI;
				break;
			case UnitScript.Strategies.NEUTRAL:
				conditions = u.NeutralAI;
				break;
		}
		bass.text = "";
		foreach (ConditionalItem c in conditions)
		{
			bass.text += c.GetSentence() + "\n";
		}
		bass.text += "Else Rest for EIGHTH note";
	}
}
