using UnityEngine;
using System.Collections.Generic;

public class GameAmp : MonoBehaviour {
    public Transform top;
    public Transform bottom;
    public List<AudioSource> sources;
    public static List<UnitScript> units = new List<UnitScript>();
    
    List<PlayNote> notesToPlay = new List<PlayNote>();
    public enum note
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        REST,
    }

    Dictionary<note, AudioClip> noteClips;

    public AudioClip A;
    public AudioClip B;
    public AudioClip C;
    public AudioClip D;
    public AudioClip E;
    public AudioClip F;
    public AudioClip G;

    bool play = false;

    // Use this for initialization
    void Start () {
        noteClips = new Dictionary<note, AudioClip>();
        noteClips.Add(note.A,A);
        noteClips.Add(note.B, B);
        noteClips.Add(note.C, C);
        noteClips.Add(note.D, D);
        noteClips.Add(note.E, E);
        noteClips.Add(note.F, F);
        noteClips.Add(note.G, G);
        BeatMaster.sixteenthEvent += TickUpdate;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    if (play)
        {
            foreach(PlayNote n in notesToPlay)
            {
                sources[(int)n.currNote].PlayOneShot(noteClips[n.currNote]);
            }
            notesToPlay.Clear();
            play = false;
        }
	}

    void TickUpdate(int cnt)
    {
        if (cnt % (int)BeatMaster.Timing.Quarter == 0)
        {
            play = true;
            
            foreach (UnitScript u in units)
            {
                note posNote = u.currNote.currNote;
                bool found = false;
                foreach (PlayNote n in notesToPlay)
                {
                    if (n.currNote == posNote)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    notesToPlay.Add(u.currNote);
                }
            
            }

        }
        }
}
