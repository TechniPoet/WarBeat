using UnityEngine;
using GAudio;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayHead : MonoBehaviour, IGATPulseClient
{
	int index = 0;
	public ArenaGrid grid;
	public MusicManager mm;
	GATActiveSampleBank sampleBank;
	public PlayBody body;
	public int team;
	List<GameObject> intervals;
	float timeTraveled = 0;
	float trip = (ConstFile.NoteBPMCalcs[(int)ConstFile.Notes.HALF] * 2) / 120;

	GATEnvelope halfEnv;
	GATEnvelope quarterEnv;
	GATEnvelope eigthEnv;

	string[] CNotes = new string[]
	{
		"paino_{0}_C_0",
		"paino_{0}_E_0",
		"paino_{0}_G_0",
	};

	string[] AmNotes = new string[]
	{
		"paino_{0}_A_0",
		"paino_{0}_C_0",
		"paino_{0}_E_0",
	};

	string[] FNotes = new string[]
	{
		"paino_{0}_F_0",
		"paino_{0}_A_0",
		"paino_{0}_C_0",
	};

	string[] GNotes = new string[]
	{
		"paino_{0}_G_0",
		"paino_{0}_B_0",
		"paino_{0}_D_0",
	};


	List<int> unitCache = new List<int>();

	// Use this for initialization
	void Start ()
	{
		body.team = team;
		body.UnitHit += PlayUnit;
		sampleBank = mm.sampleBank;
		halfEnv = MusicManager.CreateEnvelope(ConstFile.Notes.HALF, 120);
		quarterEnv = MusicManager.CreateEnvelope(ConstFile.Notes.QUARTER, 120);
		eigthEnv = MusicManager.CreateEnvelope(ConstFile.Notes.EIGHTH, 120);
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
		MusicManager._Pulse.SubscribeToPulse(this);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log(GetComponent<Rigidbody2D>().velocity);
		GetComponent<Rigidbody2D>().velocity = new Vector2(grid.arenaWidth / trip, 0);

	}

	void PlayUnit(UnitScript u)
	{
		if (unitCache.Contains(u.id))
		{
			return;
		}
		unitCache.Add(u.id);
		Debug.Log("hit unit");
		float y = u.transform.position.y;
		string[] noteArray;
		switch (index)
		{
			case 0:
				noteArray = CNotes;
				break;
			case 1:
				noteArray = AmNotes;
				break;
			case 2:
				noteArray = FNotes;
				break;
			case 3:
				noteArray = GNotes;
				break;
			default:
				throw new Exception("Play head index out of bounds");

		}
		PlayInstructs instruct = u.CurrInstruction();

		if (instruct.action != ConstFile.Actions.REST)
		{
			GATEnvelope env;
			switch (instruct.note)
			{
				case ConstFile.Notes.HALF:
					env = halfEnv;
					break;
				case ConstFile.Notes.QUARTER:
					env = quarterEnv;
					break;
				case ConstFile.Notes.EIGHTH:
					env = eigthEnv;
					break;
				default:
					Debug.LogError(string.Format("{0} note, not supported yet", instruct.note));
					env = quarterEnv;
					break;
			}
			IGATProcessedSample sample;
			int octive = UnityEngine.Random.Range(2,5);
			if (y < (grid.arenaHeight / 3) + grid.bY)
			{
				sample = sampleBank.GetProcessedSample(string.Format(noteArray[0], octive), env);
			}
			else if (y < ((grid.arenaHeight / 3) * 2) + grid.bY)
			{
				sample = sampleBank.GetProcessedSample(string.Format(noteArray[1], octive), env);
			}
			else
			{
				sample = sampleBank.GetProcessedSample(string.Format(noteArray[2], octive), env);
			}
			sample.Play(0);
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
					transform.position = new Vector3(grid.lX, intervals[0].transform.position.y);
					break;
				case 1:
					transform.position = new Vector3(grid.rX, intervals[0].transform.position.y);
					break;
			}
			index++;
			index = index % 4;
		}

		if (pulseInfo.StepIndex == 0 || pulseInfo.StepIndex == 2)
		{
			string[] noteArray;
			switch (index)
			{
				case 0:
					noteArray = CNotes;
					break;
				case 1:
					noteArray = AmNotes;
					break;
				case 2:
					noteArray = FNotes;
					break;
				case 3:
					noteArray = GNotes;
					break;
				default:
					throw new System.Exception();
			}

			IGATProcessedSample sample1 = sampleBank.GetProcessedSample(string.Format(noteArray[0], 3), halfEnv);
			IGATProcessedSample sample2 = sampleBank.GetProcessedSample(string.Format(noteArray[1], 3), halfEnv);
			IGATProcessedSample sample3 = sampleBank.GetProcessedSample(string.Format(noteArray[2], 3), halfEnv);
			sample1.Play(0);
			sample2.Play(0);
			sample3.Play(0);
		}
		
		
	}

	public void PulseStepsDidChange(bool[] newSteps)
	{
		throw new NotImplementedException();
	}
}
