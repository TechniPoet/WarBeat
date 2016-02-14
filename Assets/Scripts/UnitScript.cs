using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GAudio;
using System;

public class UnitScript : Mortal, IGATPulseClient
{
    public Transform mainTarget;
    public GameObject bullet;
    public DetectorScript vision;
    Vector3 spawnPos;
    
    public PlayNote currNote;

    public enum Actions
    {
        NEUTRAL,
        AGGRESSIVE,
        DEFENSIVE,
    }

    public Actions[] actionPattern;


    List<GameObject> enemyList;

    Vector2 goalPos;
    Vector2 currTarget;
    bool attack = false;
    bool attackCool = true;

    PulseModule pulse;
    int curr_beat = 0;

    public float moveSpeed = .1f;
    public float attackDist = 2;
    public float attackSpeed = .2f;
    public float attackCooldown = 1;
    public float bulletLife = 1.5f;
    float moveCost;
    float atkCost;
    int healCount = 0;
    float lastPulseTime = 0;
    float deltaPulseTime = 0;

    public void Setup(int newTeam, GameObject target, float newMaxE,
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
        bulletLife = newAtkLifeSpan;
        atkCost = newAtkCost;
        pulse = MusicManager._Pulse;
        pulse.SubscribeToPulse(this);
        goalPos = new Vector2(mainTarget.position.x, transform.position.y);
        vision.team = team;
        MusicManager.units.Add(this);
        spawnPos = this.transform.position;
        switch (team)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
        enemyList = team == 0 ? GameManager._RightUnits : GameManager._LeftUnits;
        enemyList.Add(mainTarget.gameObject);
        GameManager.AddUnit += EnemyAdd;
        GameManager.RemoveUnit += EnemyRemove;
    }

    // Use this for initialization
    void Awake () {
        
    }
	
	// Update is called once per frame
	void Update () {
        MortalUpdate();
        DeathMethod();
        enemyList.Sort((x, y) => CalcUtil.DistCompare(this.gameObject, x, y));
        attack = vision.enemies.Count > 0;
        currTarget = enemyList[0].transform.position;
    }

    void IGATPulseClient.OnPulse(IGATPulseInfo pulseInfo)
    {
        // Upkeep Tasks.
        PulseTimer(); // Manage pulse time.
        curr_beat = pulseInfo.StepIndex;

        // Action handle.
        switch (actionPattern[curr_beat])
        {
            case Actions.AGGRESSIVE:
                AggressiveStrat();
                break;
            case Actions.DEFENSIVE:
                DefensiveStrat();
                break;
            case Actions.NEUTRAL:
                NeutralStrat();
                break;
        }
    }

    void PulseTimer()
    {
        deltaPulseTime = Time.time - lastPulseTime;
        lastPulseTime = Time.time;
    }

    void Attack()
    {
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        atkDir *= .5f;
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, bulletLife);
        TakeDamage(atkCost);
    }

    void MoveTowards(Vector2 tar)
    {
        TakeDamage(moveCost);
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
        transform.position = new Vector2(transform.position.x, transform.position.y) + (moveDir * moveSpeed /* Time.deltaTime*/);
    }

    void Rest()
    {
        energy += gainRate;
    }

    void IGATPulseClient.PulseStepsDidChange(bool[] newSteps)
    {
        //Nothing
    }

    void EnemyAdd(int eTeam, ref GameObject enemy)
    {
        if (eTeam != this.team)
        {
            enemyList.Add(enemy);
        }
    }

    void EnemyRemove(int eTeam, ref GameObject enemy)
    {
        if (eTeam != this.team)
        {
            enemyList.Remove(enemy);
        }
    }

    void DeathMethod()
    {
        if (energy <= 0)
        {
            MusicManager.units.Remove(this);
            GameManager.RemoveDeadUnit(team, this.gameObject);
            pulse.UnsubscribeToPulse(this);
            Destroy(this.gameObject);
        }
    }

    void NeutralStrat()
    {
        Rest();
    }

    void AggressiveStrat()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            int front = team == 0 ? 1 : -1;
            if (Vector3.Distance(transform.position, enemyList[i].transform.position) < GameManager._TUnit.aggrAtkDist && ((enemyList[i].transform.position.x - transform.position.x) * front) > 0)
            {
                currTarget = enemyList[i].transform.position;
                Attack();
                return;
            }
        }
        if (energy < .4f * maxEnergy)
        {
            Rest();
            return;
        }
        MoveTowards(mainTarget.position);
    }

    void DefensiveStrat()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            int front = team == 0 ? 1 : -1;
            if (Vector3.Distance(transform.position, enemyList[i].transform.position) < GameManager._TUnit.defAtkDist)
            {
                currTarget = enemyList[i].transform.position;
                Attack();
                return;
            }
        }
        if (energy < .8f * maxEnergy)
        {
            Rest();
            return;
        }
        //MoveTowards(spawnPos);
    }
}
