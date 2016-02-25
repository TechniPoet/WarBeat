﻿using UnityEngine;
using System.Collections.Generic;
using GAudio;

public class MusicManager : MonoBehaviour, IGATPulseClient
{
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;

    public PulseModule pulse;
    public static PulseModule _Pulse;

    public GATSampleBank sampleBank;
    public GATSoundBank toLoad;
    public int arenaSplit = 4;
    int numNotes = 4;
    public static bool _PInit = false;
    public static List<UnitScript> units = new List<UnitScript>();
    
    enum Midnotes
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
    }

	/*
    string[] MidNoteNames = new string[7] 
    { "paino_{0}_A_0", "paino_{0}_B_0",
		"paino_{0}_C_0", "paino_{0}_D_0",
		"paino_{0}_E_0", "paino_{0}_F_0",
		"paino_{0}_G_0",
	};
	*/
	string[] MidNoteNames = new string[4]
	{ "paino_{0}_A_0",
		"paino_{0}_C_0",
		"paino_{0}_E_0",
		"paino_{0}_G_0",
	};

	struct MidnoteZone
    {
        public Midnotes note;
        public string name;
        public float low;
        public float high;

        public bool InZone(float yPos)
        {
            return yPos >= low && yPos <= high;
        }
    }

    MidnoteZone[] midzoneArr = new MidnoteZone[4];

    int curr_beat = 0;
    bool init = false;
	bool pulseNow = false;

    void Awake()
    {
        if (pulse != null)
        {
            _Pulse = pulse;
            //sampleBank.SoundBank = toLoad;
            //sampleBank.LoadAll();
            _Pulse.SubscribeToPulse(this);
            _PInit = true;
        }
        if (GameManager._Init)
        {
			NoteSetup();
        }
        
        

    }


	void NoteSetup ()
	{
		float arenaHeight = top.transform.position.y - bottom.transform.position.y;
		float noteWindow = arenaHeight / arenaSplit;
		Debug.Log(string.Format("Arena Height: {0}, split: {1}, Window: {2}", arenaHeight, arenaSplit, noteWindow));
		for (int i = 0; i < numNotes; i++)
		{
			midzoneArr[i] = new MidnoteZone();
			midzoneArr[i].note = (Midnotes)i;
			midzoneArr[i].name = MidNoteNames[i];
			midzoneArr[i].low = bottom.transform.position.y + (noteWindow * i);
			midzoneArr[i].high = midzoneArr[i].low + noteWindow;
			//Debug.Log(string.Format("line at {0}", midzoneArr[i].low));
			Debug.DrawLine(new Vector3(-1000, midzoneArr[i].low, 0),
				new Vector3(1000, midzoneArr[i].low, 0), Color.black, 9999, false);
		}
		init = true;
	}
    void Update()
    {
        if (!_PInit && pulse != null)
        {
            _Pulse = pulse;
            //sampleBank.SoundBank = toLoad;
            //sampleBank.LoadAll();
            _Pulse.SubscribeToPulse(this);
            _PInit = true;
        }

        if (!init)
        {
			NoteSetup();
        }
    }


	void LateUpdate()
	{
		if (pulseNow)
		{
			UnitSounds();
			pulseNow = false;
		}
	}


    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        _Pulse.UnsubscribeToPulse(this);
    }

    public void OnPulse(IGATPulseInfo pulseInfo)
    {
        curr_beat = pulseInfo.StepIndex;
		pulseNow = true;
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        //throw new NotImplementedException();
    }

    void UnitSounds()
    {
		List<string> notes = new List<string>();
        foreach(UnitScript u in units)
        {
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
			if (u == null || u.gameObject == null)
			{
				continue;
			}

			switch (u.currAction)
            {
                case UnitScript.Actions.MOVE:
                    for (int i = 0; i < numNotes; i++)
                    {
						if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
                        {
                            float arenaWidth = right.position.x - left.position.x;
                            float percent = (u.gameObject.transform.position.x - left.position.x)/arenaWidth;
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
                case UnitScript.Actions.ATTACK:
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

							GATData mySampleData = sampleBank.GetAudioData(string.Format(noteName, register + 1));
							GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
						}
					}
					break;
                case UnitScript.Actions.REST:
                    // no sound
                    break;
            }
        }
    }
}