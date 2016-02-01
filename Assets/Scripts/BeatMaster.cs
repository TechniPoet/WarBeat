using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatMaster : MonoBehaviour {
    
    public enum Timing { Measure = 16, Half = 8, Quarter = 4, Eighth = 2, Sixteenth = 1};
    public double bpm = 120.0;
    int measureNum = 0;

    public delegate void voidDel(int cnt);
    public static event voidDel newMeasureEvent;
    public static event voidDel sixteenthEvent;

    private int tickCounter;
    private double nextTick = 0.0;

    // Use this for initialization
    void Start () {
        tickCounter = 16;
        double time = AudioSettings.dspTime;
        nextTick = time + (60.0f / bpm) * 4.0f; // pre-roll 4 beats
    }
	
	// Update is called once per frame
	void Update () {
        tickManage();
    }

    void tickManage()
    {
        double time = AudioSettings.dspTime;
        if (time > nextTick)
        {
            nextTick = nextTick + ((60.0 / bpm) / 4); // sixteenths
            if (tickCounter > 16)
            {
                tickCounter = 1;
            }

            if (tickCounter % (int)Timing.Measure == 0)
            {
                //p[4].reveal();
                measureNum++;
                if (newMeasureEvent != null)
                {
                    newMeasureEvent(tickCounter);
                }
                if (sixteenthEvent != null)
                {
                    sixteenthEvent(tickCounter);
                }

            }
            else
            {
                if (sixteenthEvent != null)
                {
                    sixteenthEvent(tickCounter);
                }
            }

            /*
            if (tickCounter % (int)Timing.Sixteenth == 0)
            {
                //p[0].reveal();
            }
            if (tickCounter % (int)Timing.Eighth == 0)
            {
                p[1].reveal();
            }
            if (tickCounter % (int)Timing.Quarter == 0)
            {
                p[2].reveal();
            }
            if (tickCounter % (int)Timing.Half == 0)
            {
                p[3].reveal();
            }
            if (tickCounter % (int)Timing.Measure == 0)
            {
                p[4].reveal();
                measureNum++;
            }*/
            tickCounter++;
        }
    }
}
