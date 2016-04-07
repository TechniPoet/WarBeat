using UnityEngine;
using System.Collections.Generic;
using GAudio;
using System;

public class StatueScript : MonoBehaviour, IGATPulseClient
{
    bool init = false;
    List<Vector3> movePoints;
    int moveInd = 0;
    bool up = true;
    public Transform spawner;

	public PulseModule pulse;


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
        transform.position = movePoints[moveInd];
    }

    public void PulseStepsDidChange(bool[] newSteps)
    {
        //throw new NotImplementedException();
    }
    #endregion

    bool pInit = false;
    public void Start()
    {
		pulse.SubscribeToPulse(this);
		
        init = true;

        movePoints = new List<Vector3>();

        int div = GameManager._BaseMoveOnMeasure ? 4 : GameManager._ArenaDiv;
        this.transform.localScale = new Vector3(this.transform.localScale.x, 7f/div, this.transform.localScale.z);
       
        float noteWindow = ArenaGrid.arenaHeight / div;
        float baseY = ArenaGrid.bY;

        
        for (int i = 0; i < div; i++)
        {
            movePoints.Add(new Vector3(transform.position.x, baseY, transform.position.z));
            baseY += noteWindow;
        }
        
    }

	void OnDisable()
	{
		pulse.UnsubscribeToPulse(this);
	}

}
