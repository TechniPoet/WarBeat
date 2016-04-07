using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;
	

    public GATActiveSampleBank sampleBank;
    public GATSoundBank toLoad;

	public NoteManager wholeManager;
	public NoteManager halfManager;
	public NoteManager quarterManager;
	public NoteManager eigthManager;
	public NoteManager sixteenthManager;

	int BPM = 120;

	GATEnvelope wholeEnv;
	GATEnvelope halfEnv;
	GATEnvelope quarterEnv;
	public static GATEnvelope eigthEnv;
	GATEnvelope sixteenthEnv;

    public int arenaSplit = 4;
    int numNotes = 4;
    public static bool _PInit = false;
    public static List<UnitScript> units = new List<UnitScript>();


	int chordIndex = 0;
	List<string[]> chords;


	#region consts

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

	string[] MidNoteNames = new string[4]
	{ "paino_{0}_A_0",
		"paino_{0}_C_0",
		"paino_{0}_E_0",
		"paino_{0}_G_0",
	};

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


	#endregion



    int curr_beat = 0;
    bool init = false;
	bool pulseNow = false;

    void Awake()
    {
		Load();

		chords = new List<string[]>() { CNotes, AmNotes, FNotes, GNotes };

		
		wholeManager.PlayNotesEvent += Play;
		halfManager.PlayNotesEvent += Play;
		quarterManager.PlayNotesEvent += Play;
		eigthManager.PlayNotesEvent += Play;
		sixteenthManager.PlayNotesEvent += Play;
		
		if (GameManager._Init)
        {
			NoteSetup();
        }
    }


	void Play(List<PlayInstructs> instructs, ConstFile.Notes note)
	{
		while (GATManager.DefaultPlayer.NbOfTracks < 20)
		{
			GATManager.DefaultPlayer.AddTrack<GATTrack>();
		}
		
		if (note == ConstFile.Notes.WHOLE)
		{
			chordIndex++;
			chordIndex = chordIndex % chords.Count;
		}

		if (instructs.Count == 0)
		{
			return;
		}
		GATEnvelope env;
		switch (instructs[0].note)
		{
			case ConstFile.Notes.WHOLE:
				env = wholeEnv;
				break;
			case ConstFile.Notes.HALF:
				env = halfEnv;
				break;
			case ConstFile.Notes.QUARTER:
				env = quarterEnv;
				break;
			case ConstFile.Notes.EIGHTH:
				env = eigthEnv;
				break;
			case ConstFile.Notes.SIXTEENTH:
				env = sixteenthEnv;
				break;
			default:
				throw new System.Exception(string.Format("Invalid note: {0}", instructs[0].note));
		}
		IGATProcessedSample sample;

		string[] noteArr = chords[chordIndex];

		for (int i = 0; i < instructs.Count; i++)
		{
			if (instructs[i].action != ConstFile.Actions.REST)
			{
				int noteInd = ArenaGrid.FindSubsection(noteArr.Length, instructs[i].y);
				sample = sampleBank.GetProcessedSample(string.Format(noteArr[noteInd], 3), env);
				sample.Play(0);
				Debug.Log("Played " + instructs[i].note);
			}
			instructs[i].MakeMove();
		}
	}


	#region Pulse Methods


	public static GATEnvelope CreateEnvelope(ConstFile.Notes note, float BPM)
	{
		float sampleRate = 44100;

		int len = Mathf.FloorToInt( (ConstFile.NoteBPMCalcs[(int)note]/BPM) * sampleRate);
		int fadeIn = 0;
		int fadeOut = Mathf.FloorToInt( (sampleRate * len) / 4);
		int offset = 0;
		bool normalize = false;

		return new GATEnvelope(len, fadeIn, fadeOut, offset, normalize);
	}

	void StartPulse()
	{
		_PInit = true;
		sampleBank.LoadFinished -= StartPulse;
	}

	#endregion


	void Load()
	{
		sampleBank.LoadFinished += StartPulse;
		sampleBank.SoundBank = toLoad;
		sampleBank.LoadAll();
		wholeEnv = CreateEnvelope(ConstFile.Notes.WHOLE, BPM);
		halfEnv = CreateEnvelope(ConstFile.Notes.HALF, BPM);
		quarterEnv = CreateEnvelope(ConstFile.Notes.QUARTER, BPM);
		eigthEnv = CreateEnvelope(ConstFile.Notes.EIGHTH, BPM);
		sixteenthEnv = CreateEnvelope(ConstFile.Notes.SIXTEENTH, BPM);
	}


	#region Not Needed currently

	public void PulseStepsDidChange(bool[] newSteps)
	{
		//throw new NotImplementedException();
	}

	void UnitSounds()
    {
		while (GATManager.DefaultPlayer.NbOfTracks < 4)
		{
			GATManager.DefaultPlayer.AddTrack<GATTrack>();
		}


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
                case ConstFile.Actions.MOVE_ENEMY:
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
        }
    }

	void NoteSetup()
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

	#endregion
}
