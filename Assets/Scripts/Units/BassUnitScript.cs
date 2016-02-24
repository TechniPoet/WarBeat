using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class BassUnitScript : UnitScript
{

	public GameObject bullet;

	float aggrAtkDist;
	float defAtkDist;


	public void BassSetup(int newTeam, GameObject target, float newMaxE,
		float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
		float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
	{
		UnitSetup(newTeam, target, newMaxE, newStartE, newGainRate, newMoveCost, newAtkCost,
		newMoveSpeed, newAtkSpeed, newAtkLifeSpan);
		currType = UnitType.BASS;
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
			if (Vector3.Distance(transform.position, enemyList[i].transform.position) < GameManager._BUnit.aggrAtkDist && ((enemyList[i].transform.position.x - transform.position.x) * front) > 0)
			{
				currTarget = enemyList[i].transform.position;
				Attack();
				return;
			}
		}
		if (energy < .5f * maxEnergy)
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
			if (Vector3.Distance(transform.position, enemyList[i].transform.position) < GameManager._BUnit.defAtkDist)
			{
				currTarget = enemyList[i].transform.position;
				Attack();
				return;
			}
		}
		Rest();
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
		currAction = Actions.ATTACK;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public new void Update () {
		base.Update();
		DebugExtension.DebugCircle(transform.position, Vector3.forward, GameManager._BUnit.aggrAtkDist);
	}
}
