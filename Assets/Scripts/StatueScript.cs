using UnityEngine;
using System.Collections;
using GAudio;
using System;

public class StatueScript : Mortal, IGATPulseClient
{
    bool init = false;

    #region Pulse Methods
    public void OnPulse(IGATPulseInfo pulseInfo)
    {
        //throw new NotImplementedException();
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        throw new NotImplementedException();
    }
    #endregion

    public void Setup(float newStartE, float newMaxE, float newGainRate)
    {
        energy = newStartE;
        maxEnergy = newMaxE;
        gainRate = newGainRate;
        init = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            energy += gainRate * Time.deltaTime;
            MortalUpdate();
        }
    }

}
