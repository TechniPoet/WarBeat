﻿using UnityEngine;
using System.Collections;

public class TrebleUnitScript : UnitScript
{
    public GameObject bullet;

    new void Update()
    {
        base.Update();
    }

    void TrebleSetup(int newTeam, GameObject target, float newMaxE,
        float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
        float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
    {
        UnitSetup(newTeam, target, newMaxE, newStartE, newGainRate, newMoveCost, newAtkCost,
        newMoveSpeed, newAtkSpeed, newAtkLifeSpan);
    }


    #region Strategies

    protected override void NeutralStrat()
    {
        Rest();
    }

    protected override void AggressiveStrat()
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

    protected override void DefensiveStrat()
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

    #endregion

    
    protected override void Attack()
    {
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        atkDir *= .5f;
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, atkLife);
        TakeDamage(atkCost);
    }
}
