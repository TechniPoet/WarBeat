using UnityEngine;
using GAudio;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayHead : MonoBehaviour, IGATPulseClient
{
	//int index = 0;
	public ArenaGrid grid;
	public MusicManager mm;
	//GATActiveSampleBank sampleBank;
	public PlayBody body;
	public int team;
	public NoteManager eigthManager;
	List<GameObject> intervals;
	float trip = (ConstFile.NoteBPMCalcs[(int)ConstFile.Notes.HALF] * 2) / 120;

	public PulseModule pulse;
	List<int> unitCache = new List<int>();

	void OnEnable()
	{
		pulse.SubscribeToPulse(this);
	}

	void OnDisable()
	{
		pulse.UnsubscribeToPulse(this);
	}

	// Use this for initialization
	void Start ()
	{
		body.team = team;
		body.UnitHit += PlayUnit;
		//sampleBank = mm.sampleBank;
		switch (team)
		{
			case 0:
				intervals = grid.leftPlayHeads;
				break;
			case 1:
				intervals = grid.rightPlayHeads;
				trip = -trip;
				break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(ArenaGrid.arenaWidth / trip, 0);
	}

	void PlayUnit(UnitScript u)
	{
		if (unitCache.Contains(u.id))
		{
			return;
		}
		unitCache.Add(u.id);
		//float y = u.transform.position.y;
		//string[] noteArray;
		PlayInstructs instruct = u.CurrInstruction();

		switch (instruct.note)
		{
			case ConstFile.Notes.HALF:
				//Debug.Log("added half");
				mm.halfManager.AddInstruct(instruct);
				break;
			case ConstFile.Notes.QUARTER:
				//Debug.Log("added quarter");
				mm.quarterManager.AddInstruct(instruct);
				break;
			case ConstFile.Notes.EIGHTH:
				//Debug.Log("added eigth");
				mm.eigthManager.AddInstruct(instruct);
				break;
			case ConstFile.Notes.SIXTEENTH:
				mm.sixteenthManager.AddInstruct(instruct);
				break;
		}
	}

	public void OnPulse(IGATPulseInfo pulseInfo)
	{
		//index = pulseInfo.StepIndex;
		if (pulseInfo.StepIndex == 0)
		{
			unitCache.Clear();
			switch(team)
			{
				case 0:
					transform.position = new Vector3(ArenaGrid.lX, intervals[0].transform.position.y);
					break;
				case 1:
					transform.position = new Vector3(ArenaGrid.rX, intervals[0].transform.position.y);
					break;
			}
		}
	}

	public void PulseStepsDidChange(bool[] newSteps)
	{
		throw new NotImplementedException();
	}
}
