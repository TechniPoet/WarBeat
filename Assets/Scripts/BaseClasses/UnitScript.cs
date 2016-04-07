using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAudio;
using System;
using Actions = ConstFile.Actions;
using CondOptions = ConstFile.ConditionOptions;

public class PlayInstructs
{
	public Actions action;
	public ConstFile.Notes note;
	public float y;
	UnitScript unit;
	public PlayInstructs(Actions newAct, ConstFile.Notes newNote, float yPos, UnitScript u)
	{
		action = newAct;
		note = newNote;
		y = yPos;
		unit = u;
	}

	public void MakeMove()
	{
		unit.MakeMove(this);
	}
}

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

	protected List<ConditionalItem> neutralAI;
	protected List<ConditionalItem> aggressiveAI;
	protected List<ConditionalItem> defensiveAI;
	Vector3 backMovementMod;
	public ConstFile.Actions currAction;
	public ConstFile.Notes currNote;
    public Strategies[] actionPattern;
    public Transform mainTarget;
    

    protected Vector3 spawnPos;
    protected List<GameObject> enemyList;

    protected Vector2 goalPos;
    protected Vector2 currTarget;

	protected int NoteMult
	{
		get
		{
			switch(currNote)
			{
				case ConstFile.Notes.SIXTEENTH:
					return 1;
				case ConstFile.Notes.EIGHTH:
					return 2;
				case ConstFile.Notes.QUARTER:
					return 4;
				case ConstFile.Notes.HALF:
					return 8;
				case ConstFile.Notes.WHOLE:
					return 16;
				
				default:
					return 1;
			}
		}
	}
    // pulse variables
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

	public static int idCounter;
	public int id;

	public Vector2 gridLocation;

    #region Pulse Methods
    void IGATPulseClient.OnPulse(IGATPulseInfo pulseInfo)
    {
        // Upkeep Tasks.
        PulseTimer(); // Manage pulse time.
        curr_beat = pulseInfo.StepIndex;

        // Action handle.
        switch (actionPattern[0])
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

    protected void EnemyAdd(int eTeam, ref GameObject enemy, UnitType type)
    {
        if (eTeam != this.team)
        {
            enemyList.Add(enemy);
        }
    }

    protected void EnemyRemove(int eTeam, ref GameObject enemy, UnitType type)
    {
        if (eTeam != this.team)
        {
            enemyList.Remove(enemy);
        }
    }

    #endregion

    #region Abstract Methods

    protected abstract void Attack();

	#endregion

	#region Strategies

	protected void NeutralStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < neutralAI.Count; i++)
		{
			ConditionalItem decision = neutralAI[i];
			actionMade = ActionDecision(decision);
			if (actionMade)
			{
				break;
			}
		}
		if (!actionMade)
		{
			Rest();
		}
	}

	bool ActionDecision(ConditionalItem decision)
	{
		bool actionMade = false;
		if (ValidAction(decision))
		{
			actionMade = true;
			currNote = decision.note;
			currAction = decision.action;
		}
		return actionMade;
	}


	public void MakeMove(PlayInstructs instruct)
	{
		currNote = instruct.note;
		switch (instruct.action)
		{
			case Actions.ATTACK:
				currTarget = enemyList[0].transform.position;
				Attack();
				break;
			case Actions.MOVE_BACK:
				Vector3 backTarget = this.transform.position + backMovementMod;
				currAction = Actions.MOVE_BACK;
				MoveTowards(backTarget);
				break;
			case Actions.MOVE_FORWARD:
				Vector3 forTarget = this.transform.position + -backMovementMod;
				currAction = Actions.MOVE_FORWARD;
				MoveTowards(forTarget);
				break;
			case Actions.MOVE_ENEMY:
				currAction = Actions.MOVE_ENEMY;
				MoveTowards(enemyList[0].transform.position);
				break;
			case Actions.REST:
				Rest();
				break;
		}
	}

	protected void AggressiveStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < aggressiveAI.Count; i++)
		{
			ConditionalItem decision = aggressiveAI[i];
			if (ValidAction(decision))
			{
				actionMade = ActionDecision(decision);
				if (actionMade)
				{
					break;
				}
			}
		}
		if (!actionMade)
		{
			Rest();
		}
	}

	protected void DefensiveStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < defensiveAI.Count; i++)
		{
			ConditionalItem decision = defensiveAI[i];
			actionMade = ActionDecision(decision);
			if (actionMade)
			{
				break;
			}
		}
		if (!actionMade)
		{
			Rest();
		}
	}

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

		id = idCounter++;
        enemyList = team == 0 ? GameManager._RightUnits : GameManager._LeftUnits;
        enemyList.Add(mainTarget.gameObject);
		
        MusicManager.units.Add(this);
        GameManager.AddUnit += EnemyAdd;
        GameManager.RemoveUnit += EnemyRemove;

		

		// Set color
		switch (team)
        {
            case 0:
				GetComponentInChildren<SpriteRenderer>().color = Color.blue;

				backMovementMod = new Vector3(-100, 0, 0);
				break;
            case 1:
				GetComponentInChildren<SpriteRenderer>().color = Color.red;
				actionPattern = new Strategies[] {Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.AGGRESSIVE, Strategies.AGGRESSIVE};

				backMovementMod = new Vector3(100, 0, 0);
				
				break;
        }

		gridLocation = ArenaGrid.ClosestGridPoint(transform.position);
		transform.position = ArenaGrid.GridToWorldPos(gridLocation);
		
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
		
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
		ConstFile.Direction dir = MathUtil.MoveDir(moveDir);
		Vector2 temp = (ConstFile.DirectionVectors[(int)dir] * moveSpeed * NoteMult) + gridLocation;
		if (!ArenaGrid.ValidGridPos(temp))
		{
			Rest();
		}
		gridLocation = temp;
		transform.position = ArenaGrid.GridToWorldPos(gridLocation);
		TakeDamage(moveCost * NoteMult);
        
	}

    protected void Rest()
    {
		currAction = Actions.REST;
		energy += gainRate;
    }

    protected void DeathMethod()
    {
        if (energy <= 0)
        {
            MusicManager.units.Remove((UnitScript)this);
            GameManager.RemoveDeadUnit(team, this.gameObject, currType);
            //pulse.UnsubscribeToPulse(this);
            Destroy(this.gameObject);
        }
    }

	protected bool ValidAction(ConditionalItem item)
	{
		float firstVal = 0, secondVal = 0;

		switch (item.cond1Ind)
		{
			case CondOptions.ENEMY_DISTANCE:
				if (enemyList.Count > 0)
				{
					firstVal = ArenaGrid.GridDistance(this.transform.position, enemyList[0].transform.position);
				}
				else
				{
					firstVal = float.MaxValue;
				}
				break;
			case CondOptions.ENERGY:
				firstVal = energy;
				break;
			case CondOptions.VALUE:
				firstVal = item.cond1Val;
				break;
		}

		switch (item.cond2Ind)
		{
			case CondOptions.ENEMY_DISTANCE:
				if (enemyList.Count > 0)
				{
					secondVal = ArenaGrid.GridDistance(this.transform.position, enemyList[0].transform.position);
				}
				else
				{
					secondVal = float.MaxValue;
				}
				break;
			case CondOptions.ENERGY:
				secondVal = energy;
				break;
			case CondOptions.VALUE:
				secondVal = item.cond2Val;
				break;
		}

		bool retVal = item.greater ? firstVal > secondVal : firstVal < secondVal;

		return retVal;
	}

	public void UpdateActionPattern(Strategies strat)
	{
		actionPattern[0] = strat;
	}
    
	public PlayInstructs CurrInstruction()
	{
		switch (actionPattern[0])
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
		return new PlayInstructs(currAction, currNote, transform.position.y, this);
	}
}
