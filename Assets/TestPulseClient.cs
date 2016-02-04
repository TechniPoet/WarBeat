using UnityEngine;
using System.Collections;
using GAudio;
using System;

public class TestPulseClient : MonoBehaviour, IGATPulseClient
{

    public PulseModule pulse;
    public GATSampleBank bank;
    public GATSoundBank toLoad;

    void OnEnable()
    {
        
        bank.SoundBank = toLoad;
        bank.LoadAll();
        pulse.SubscribeToPulse( this );
    }

    void OnDisable()
    {
        pulse.UnsubscribeToPulse( this );
    }

    public void OnPulse(IGATPulseInfo pulseInfo)
    {
        GATData mySampleData = bank.GetAudioData("paino_5_G_0");
        int trackNumber = 0;
        GATManager.DefaultPlayer.PlayData(mySampleData, trackNumber);

        mySampleData = bank.GetAudioData("paino_5_A_0");
        GATManager.DefaultPlayer.PlayData(mySampleData, 0);
        Debug.Log("pulse");
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        //throw new NotImplementedException();
    }
}
