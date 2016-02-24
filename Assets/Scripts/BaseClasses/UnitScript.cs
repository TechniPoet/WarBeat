using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAudio;
using System;

public abstract class UnitScript : Mortal, IGATPulseClient
{
	public enum UnitType
	{
		BASS,
		TREBLE,
	}
	public UnitType currType;
    public enum Strategies
    {
        NEUTRAL,
        AGGRESSIVE,
        DEFENSIVE,
    }

	public enum Actions
	{
		ATTACK,
		REST,
		MOVE
	}
	public Actions currAction;
    public Strategies[] actionPattern;
    public Transform mainTarget;
    

    protected Vector3 spawnPos;
    protected List<GameObject> enemyList;

    protected Vector2 goalPos;
    protected Vector2 currTarget;

    // pulse variables
    protected PulseModule pulse;
    protected int curr_beat = 0;
    //Move Variables
    protected float moveCost;
    public float moveSpeed = .1f;
    // Attack variables
    protected float atkCost;
    public float attackSpeed = .2f;
    public float atkLife = 1.5f;
    // Time variables.. not sure if these will be needed
    protected float lastPulseTime = 0;
    protected float deltaPulseTime = 0;

    #region Pulse Methods
    void IGATPulseClient.OnPulse(IGATPulseInfo pulseInfo)
    {
        // Upkeep Tasks.
        PulseTimer(); // Manage pulse time.
        curr_beat = pulseInfo.StepIndex;

        // Action handle.
        switch (actionPattern[curr_beat])
        {
            case Strategies.AGGRESSIVE:
                AggressiveStrat();
                break;
            case Strategies.DEFENSIVE:
                DefensiveStrat();
                break;
            case Strategies.NEUTRAL:
                NeutralStrat();
                break;
        }
    }

    void IGATPulseClient.PulseStepsDidChange(bool[] newSteps)
    {
        //Nothing
    }

    #endregion

    #region House Keeping

    protected void PulseTimer()
    {
        deltaPulseTime = Time.time - lastPulseTime;
        lastPulseTime = Time.time;
    }

    protected void EnemyAdd(int eTeam, ref GameObject enemy)
    {
        if (eTeam != this.team)
        {
            enemyList.Add(enemy);
        }
    }

    protected void EnemyRemove(int eTeam, ref GameObject enemy)
    {
        if (eTeam != this.team)
        {
            enemyList.Remove(enemy);
        }
    }

    #endregion

    #region Abstract Methods

    protected abstract void Attack();

    protected abstract void NeutralStrat();

    protected abstract void AggressiveStrat();

    protected abstract void DefensiveStrat();

    #endregion

    public void UnitSetup(int newTeam, GameObject target, float newMaxE,
        float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
        float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
    {
        team = newTeam;
        mainTarget = target.transform;
        energy = newStartE;
        maxEnergy = newStartE;
        gainRate = newGainRate;
        lastPulseTime = Time.time;
        moveSpeed = newMoveSpeed;
        attackSpeed = newAtkSpeed;
        moveCost = newMoveCost;
        atkLife = newAtkLifeSpan;
        atkCost = newAtkCost;
        goalPos = new Vector2(mainTarget.position.x, transform.position.y);
        spawnPos = this.transform.position;

        enemyList = team == 0 ? GameManager._RightUnits : GameManager._LeftUnits;
        enemyList.Add(mainTarget.gameObject);

        //Subscriptions
        pulse = MusicManager._Pulse;
        pulse.SubscribeToPulse(this);
        MusicManager.units.Add(this);
        GameManager.AddUnit += EnemyAdd;
        GameManager.RemoveUnit += EnemyRemove;

        // Set color
        switch (team)
        {
            case 0:
				GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                break;
            case 1:
				GetComponentInChildren<SpriteRenderer>().color = Color.red;
				actionPattern = new Strategies[8] {Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.DEFENSIVE, Strategies.DEFENSIVE,
				Strategies.AGGRESSIVE, Strategies.NEUTRAL,Strategies.AGGRESSIVE, Strategies.DEFENSIVE};
                break;
        }
    }
	
	// Update is called once per frame
	protected void Update () {
        MortalUpdate();
        DeathMethod();
        enemyList.Sort((x, y) => CalcUtil.DistCompare(this.gameObject, x, y));
        currTarget = enemyList[0].transform.position;
    }

    protected void MoveTowards(Vector2 tar)
    {
        TakeDamage(moveCost);
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
        transform.position = new Vector2(transform.position.x, transform.position.y) + (moveDir * moveSpeed /* Time.deltaTime*/);
		currAction = Actions.MOVE;
    }

    protected void Rest()
    {
        energy += gainRate;
		currAction = Actions.REST;
    }

    protected void DeathMethod()
    {
        if (energy <= 0)
        {
            MusicManager.units.Remove((UnitScript)this);
            GameManager.RemoveDeadUnit(team, this.gameObject);
            pulse.UnsubscribeToPulse(this);
            Destroy(this.gameObject);
        }
    }

    
}
