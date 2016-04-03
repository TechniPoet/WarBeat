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
	GATActiveSampleBank sample;
	public PlayBody body;
	public int team;
	List<GameObject> intervals;
	float timeTraveled = 0;
	float trip = (ConstFile.NoteBPMCalcs[(int)ConstFile.Notes.HALF] * 2) / 120;

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
		"paino_{0}_G_0",
	};

	// Use this for initialization
	void Start ()
	{
		body.UnitHit += PlayUnit;
		sample = mm.sampleBank;
		switch (team)
		{
			case 0:
				intervals = grid.leftPlayHeads;
				break;
			case 1:
				intervals = grid.rightPlayHeads;
				break;
		}
		MusicManager._Pulse.SubscribeToPulse(this);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log(GetComponent<Rigidbody2D>().velocity);
		GetComponent<Rigidbody2D>().velocity = new Vector2(grid.arenaWidth / trip, 0);
		/*
		timeTraveled += Time.deltaTime;
		float perc = timeTraveled / trip;
		//Debug.Log(perc);
		float x = Mathf.Lerp(grid.lX, intervals[3].transform.position.x, perc);
		transform.position = new Vector3(x, intervals[3].transform.position.y);

		bool increment = team == 0 ? x > intervals[index].transform.position.x : x < intervals[index].transform.position.x;
		*/

	}

	void PlayUnit(UnitScript u)
	{
		Debug.Log("hit unit");
		switch (index)
		{
			case 0:
				float y = u.transform.position.y;
				if (y < (grid.arenaHeight/3) + grid.bY)
				{
					GATData mySampleData = sample.GetAudioData(string.Format(CNotes[0], 3));
					GATManager.DefaultPlayer.PlayData(mySampleData, 1, 1);
				}
				else if (y < ((grid.arenaHeight / 3) * 2) + grid.bY)
				{
					GATData mySampleData = sample.GetAudioData(string.Format(CNotes[1], 3));
					GATManager.DefaultPlayer.PlayData(mySampleData, 1, 1);
				}
				else
				{
					GATData mySampleData = sample.GetAudioData(string.Format(CNotes[2], 3));
					GATManager.DefaultPlayer.PlayData(mySampleData, 1, 1);
				}
				break;

		}
		/*
		int register;

		switch (u.currType)
		{
			case UnitScript.UnitType.TREBLE:
				register = Random.Range(3, 4);
				break;
			case UnitScript.UnitType.BASS:
				register = Random.Range(1, 2);
				break;
			default:
				register = 3;
				break;
		}

		switch (u.currAction)
		{
			case ConstFile.Actions.MOVE_ENEMY:
				for (int i = 0; i < numNotes; i++)
				{
					if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
					{
						float arenaWidth = right.position.x - left.position.x;
						float percent = (u.gameObject.transform.position.x - left.position.x) / arenaWidth;
						int trackNumber = 0;
						string noteName;
						if (u.team == 0)
						{
							noteName = midzoneArr[(i + 2) % numNotes].name;
						}
						else
						{
							noteName = midzoneArr[i].name;
							percent = 1 - percent;
						}
						noteName = string.Format(noteName, register);
						if (!notes.Contains(noteName))
						{
							GATData mySampleData = sampleBank.GetAudioData(string.Format(noteName, register));
							GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
							notes.Add(noteName);
						}
					}
				}
				break;

			case ConstFile.Actions.MOVE_BACK:
				for (int i = 0; i < numNotes; i++)
				{
					if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
					{
						float arenaWidth = right.position.x - left.position.x;
						float percent = (u.gameObject.transform.position.x - left.position.x) / arenaWidth;
						int trackNumber = 1;
						string noteName;
						if (u.team == 0)
						{
							noteName = midzoneArr[(i + 2) % numNotes].name;
						}
						else
						{
							noteName = midzoneArr[i].name;
							percent = 1 - percent;
						}
						noteName = string.Format(noteName, register);
						if (!notes.Contains(noteName))
						{
							GATData mySampleData = sampleBank.GetAudioData(string.Format(noteName, register));
							GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
							notes.Add(noteName);
						}
					}
				}
				break;
			case ConstFile.Actions.MOVE_FORWARD:
				for (int i = 0; i < numNotes; i++)
				{
					if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
					{
						float arenaWidth = right.position.x - left.position.x;
						float percent = (u.gameObject.transform.position.x - left.position.x) / arenaWidth;
						int trackNumber = 2;
						string noteName;
						if (u.team == 0)
						{
							noteName = midzoneArr[(i + 2) % numNotes].name;
						}
						else
						{
							noteName = midzoneArr[i].name;
							percent = 1 - percent;
						}
						noteName = string.Format(noteName, register);
						if (!notes.Contains(noteName))
						{
							GATData mySampleData = sampleBank.GetAudioData(string.Format(noteName, register));
							GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
							notes.Add(noteName);
						}
					}
				}
				break;

			case ConstFile.Actions.ATTACK:
				for (int i = 0; i < numNotes; i++)
				{
					if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
					{
						float arenaWidth = right.position.x - left.position.x;
						float percent = (u.gameObject.transform.position.x - left.position.x) / arenaWidth;
						int trackNumber = 3;
						string noteName;
						if (u.team == 0)
						{
							noteName = midzoneArr[(i + 2) % numNotes].name;
						}
						else
						{
							noteName = midzoneArr[i].name;
							percent = 1 - percent;
						}

						GATData mySampleData = sampleBank.GetAudioData(string.Format(noteName, register + 1));
						GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
					}
				}
				break;
			case ConstFile.Actions.REST:
				// no sound
				break;
		}
		*/
	}

	public void OnPulse(IGATPulseInfo pulseInfo)
	{
		index = pulseInfo.StepIndex;
		if (pulseInfo.StepIndex == 0)
		{
			transform.position = new Vector3(grid.lX, intervals[0].transform.position.y);
		}
	}

	public void PulseStepsDidChange(bool[] newSteps)
	{
		throw new NotImplementedException();
	}
}
