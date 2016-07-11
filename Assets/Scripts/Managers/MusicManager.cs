using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System.Collections;
using PuppetType = ConstFile.PuppetType;

public class MusicManager : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;

	public delegate void voidDel(int ind);
	public static voidDel Beat;

    public GATActiveSampleBank sampleBank;
    public GATSoundBank toLoad;

	public NoteManager wholeManager;
	public NoteManager halfManager;
	public NoteManager quarterManager;
	public NoteManager eigthManager;
	public NoteManager sixteenthManager;

	GATEnvelope wholeEnv;
	GATEnvelope halfEnv;
	GATEnvelope quarterEnv;
	public static GATEnvelope eigthEnv;
	GATEnvelope sixteenthEnv;

    public int arenaSplit = 4;
    int numNotes = 4;
    public static bool _PInit = false;
    public static List<UnitScript> units = new List<UnitScript>();
	int sixteenthCnt = 0;
	int eigthCnt = 0;
	int quarterCnt = 0;
	int halfCnt = 0;
	int wholeCnt = 0;

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



    //int curr_beat = 0;
    //bool init = false;
	//bool pulseNow = false;

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
		DebugPanel.Log(note.ToString(), "W/E", instructs.Count);
		IGATProcessedSample sample;
		string[] noteArr;
		while (GATManager.DefaultPlayer.NbOfTracks < 20)
		{
			
			GATManager.DefaultPlayer.AddTrack<GATTrack>();
			/*
			noteArr = chords[chordIndex];

			sample = sampleBank.GetProcessedSample(string.Format(noteArr[0], 3), wholeEnv);
			sample.Play(0);
			sample = sampleBank.GetProcessedSample(string.Format(noteArr[1], 3), wholeEnv);
			sample.Play(0);
			sample = sampleBank.GetProcessedSample(string.Format(noteArr[2], 3), wholeEnv);
			sample.Play(0);
			*/
		}
		noteArr = chords[chordIndex];
		GATData mySampleData;
		switch (note)
		{
			case ConstFile.Notes.WHOLE:
				chordIndex++;
				chordIndex = chordIndex % chords.Count;
				wholeCnt++;
				halfCnt = -1;
				quarterCnt = -1;
				eigthCnt = -1;
				sixteenthCnt = -1;
				sampleBank.FlushCacheForEnvelope(wholeEnv);
				sampleBank.FlushCacheForEnvelope(halfEnv);
				sampleBank.FlushCacheForEnvelope(quarterEnv);
				sampleBank.FlushCacheForEnvelope(eigthEnv);
				sampleBank.FlushCacheForEnvelope(sixteenthEnv);
				break;
			case ConstFile.Notes.HALF:
				halfCnt++;
				break;
			case ConstFile.Notes.QUARTER:
				quarterCnt++;
				if (quarterCnt % 4 == 1 || quarterCnt % 4 == 3)
				{
					mySampleData = sampleBank.GetAudioData("clap-808");
					GATManager.DefaultPlayer.PlayData(mySampleData, 1, 1);
				}
				break;
			case ConstFile.Notes.EIGHTH:
				eigthCnt++;
				if (eigthCnt % 8 == 0 || eigthCnt % 8 == 1 || eigthCnt % 8 == 5)
				{
					mySampleData = sampleBank.GetAudioData("kick-gritty");
					GATManager.DefaultPlayer.PlayData(mySampleData, 1, 1);
				}
				mySampleData = sampleBank.GetAudioData(string.Format(noteArr[0], 2));
				GATManager.DefaultPlayer.PlayData(mySampleData, 3, .2f);
				break;
			case ConstFile.Notes.SIXTEENTH:
				sixteenthCnt++;
				if (Beat != null)
				{
					Beat(sixteenthCnt);
				}
				
				if ((sixteenthCnt % 16 == 14 || sixteenthCnt % 16 == 15) && wholeCnt % 2 == 0)
				{
					mySampleData = sampleBank.GetAudioData("hihat-808");
					GATManager.DefaultPlayer.PlayData(mySampleData, 2, 1);
				}
				break;
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
		

		for (int i = 0; i < instructs.Count; i++)
		{
			if (instructs[i].Alive())
			{
				instructs[i].MakeMove();
				if (instructs[i].action != ConstFile.Actions.REST || instructs[i].puppetType() == ConstFile.PuppetType.TOWER)
				{
					// Attempt to double size of note area for more variance.
					int noteInd = ArenaGrid.FindSubsection(noteArr.Length * 2, instructs[i].y);
					int octave = instructs[i].puppetType() == PuppetType.BASS ? 2 : 4;
					if (noteInd > noteArr.Length)
					{
						octave++;
					}
					noteInd = noteInd % noteArr.Length;
					sample = sampleBank.GetProcessedSample(string.Format(noteArr[noteInd], octave), env);
					sample.Play(0);
					//Debug.Log("Played " + instructs[i].note);
				}
			}
		}
		
	}


	#region Pulse Methods


	public static GATEnvelope CreateEnvelope(ConstFile.Notes note, float BPM)
	{
		float sampleRate = 44100;

		int len = Mathf.FloorToInt( (ConstFile.NoteBPMCalcs[(int)note]/ConstFile.BPM) * sampleRate);
		int fadeIn = Mathf.FloorToInt((sampleRate * len) / 135000);
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
		wholeEnv = CreateEnvelope(ConstFile.Notes.WHOLE, ConstFile.BPM);
		halfEnv = CreateEnvelope(ConstFile.Notes.HALF, ConstFile.BPM);
		quarterEnv = CreateEnvelope(ConstFile.Notes.QUARTER, ConstFile.BPM);
		eigthEnv = CreateEnvelope(ConstFile.Notes.EIGHTH, ConstFile.BPM);
		sixteenthEnv = CreateEnvelope(ConstFile.Notes.SIXTEENTH, ConstFile.BPM);
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
				case PuppetType.TREBLE:
					register = Random.Range(3, 4);
					break;
				case PuppetType.BASS:
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
