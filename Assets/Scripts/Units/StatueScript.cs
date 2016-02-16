using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;

public class StatueScript : Mortal, IGATPulseClient
{
    bool init = false;
    List<Vector3> movePoints;
    int moveInd = 0;
    bool up = true;
    public Transform spawner;

    #region Pulse Methods
    public void OnPulse(IGATPulseInfo pulseInfo)
    {
        if (up)
        {
            if (moveInd == movePoints.Count - 1)
            {
                up = false;
                moveInd --;
            }
            else
            {
                moveInd++;
            }
        }
        else
        {
            if (moveInd == 0)
            {
                up = true;
                moveInd ++;
            }
            else
            {
                moveInd--;
            }
        }
        Debug.Log(moveInd);
        transform.position = movePoints[moveInd];
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        //throw new NotImplementedException();
    }
    #endregion

    bool pInit = false;
    public void Setup(float newStartE, float newMaxE, float newGainRate)
    {
        
        energy = newStartE;
        maxEnergy = newMaxE;
        gainRate = newGainRate;
        init = true;

        movePoints = new List<Vector3>();

        int div = GameManager._BaseMoveOnMeasure ? 4 : GameManager._ArenaDiv;
        this.transform.localScale = new Vector3(this.transform.localScale.x, 7f/div, this.transform.localScale.z);
        float arenaHeight = GameManager._Top.transform.position.y - GameManager._Bottom.transform.position.y;
        float noteWindow = arenaHeight / div;
        float baseY = GameManager._Bottom.transform.position.y + (noteWindow/2);

        
        for (int i = 0; i < div; i++)
        {
            movePoints.Add(new Vector3(transform.position.x, baseY, transform.position.z));
            baseY += noteWindow;
        }
        if (MusicManager._PInit)
        {
            MusicManager._Pulse.SubscribeToPulse(this);
            pInit = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            energy += gainRate * Time.deltaTime;
            MortalUpdate();
        }
        if (!pInit && MusicManager._PInit)
        {
            MusicManager._Pulse.SubscribeToPulse(this);
            pInit = true;
        }
    }

}
