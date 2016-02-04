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
    public int arenaSplit = 7;
    int numNotes = 7;
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

    string[] MidNoteNames = new string[7] 
    { "paino_3_A_0", "paino_3_B_0",
        "paino_3_C_0", "paino_3_D_0",
        "paino_3_E_0", "paino_3_F_0",
        "paino_3_G_0",};

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

    MidnoteZone[] midzoneArr = new MidnoteZone[7];

    int curr_beat = 0;

    void Awake()
    {
        float arenaHeight = top.transform.position.y - bottom.transform.position.y;
        float noteWindow = arenaHeight / arenaSplit;
        for (int i = 0; i < numNotes; i++)
        {
            midzoneArr[i] = new MidnoteZone();
            midzoneArr[i].note = (Midnotes)i;
            midzoneArr[i].name = MidNoteNames[i];
            midzoneArr[i].low = bottom.transform.position.y + (noteWindow* i);
            midzoneArr[i].high = midzoneArr[i].low + noteWindow;
            Debug.DrawLine(new Vector3(-1000, midzoneArr[i].low, 0),
                new Vector3(1000, midzoneArr[i].low, 0), Color.black, 9999, false);
        }
        _Pulse = pulse;
    }

    void OnEnable()
    {
        sampleBank.SoundBank = toLoad;
        sampleBank.LoadAll();
        pulse.SubscribeToPulse(this);
    }

    void OnDisable()
    {
        pulse.UnsubscribeToPulse(this);
    }

    public void OnPulse(IGATPulseInfo pulseInfo)
    {
        curr_beat = pulseInfo.StepIndex;
        UnitSounds();
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        //throw new NotImplementedException();
    }

    void UnitSounds()
    {
        foreach(UnitScript u in units)
        {
            switch(u.actionPattern[curr_beat])
            {
                case UnitScript.Actions.MOVE:
                    for (int i = 0; i < arenaSplit; i++)
                    {
                        if (midzoneArr[i].InZone(u.gameObject.transform.position.y))
                        {
                            float arenaWidth = right.position.x - left.position.x;
                            float percent = (u.gameObject.transform.position.x - left.position.x)/arenaWidth;
                            if (u.team == 0)
                            {
                                GATData mySampleData = sampleBank.GetAudioData(midzoneArr[(i+2)%numNotes].name);
                                int trackNumber = 0;
                                GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, percent);
                            }
                            else
                            {
                                GATData mySampleData = sampleBank.GetAudioData(midzoneArr[i].name);
                                int trackNumber = 0;
                                GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber, 1-percent);
                            }
                            
                        }
                    }
                    break;
                case UnitScript.Actions.ATTACK:
                    break;
                case UnitScript.Actions.NONE:
                    // Add energy
                    break;
            }
        }
    }
}
