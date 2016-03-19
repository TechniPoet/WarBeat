using UnityEngine;
using System.Collections.Generic;
using Actions = ConstFile.Actions;

public class BassUnitScript : UnitScript
{

	public GameObject bullet;
	Vector3 backMovementMod;
	float aggrAtkDist;
	float defAtkDist;
	List<ConditionalItem> neutralAI;
	List<ConditionalItem> aggressiveAI;
	List<ConditionalItem> defensiveAI;

	public void BassSetup(int newTeam, GameObject target, float newMaxE,
		float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
		float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
	{
		UnitSetup(newTeam, target, newMaxE, newStartE, newGainRate, newMoveCost, newAtkCost,
		newMoveSpeed, newAtkSpeed, newAtkLifeSpan);
		currType = UnitType.BASS;
		if (team == 0)
		{
			backMovementMod = new Vector3(-100, 0, 0);
			neutralAI = AIManager.BassUnit.NeutralAI;
			aggressiveAI = AIManager.BassUnit.AggressiveAI;
			defensiveAI = AIManager.BassUnit.DefensiveAI;
		}
		else
		{
			backMovementMod = new Vector3(100, 0, 0);
			neutralAI = AIManager.EnemyBassUnit.NeutralAI;
			aggressiveAI = AIManager.EnemyBassUnit.AggressiveAI;
			defensiveAI = AIManager.EnemyBassUnit.DefensiveAI;
		}
	}


	#region Strategies

	protected override void NeutralStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < neutralAI.Count; i++)
		{
			ConditionalItem decision = neutralAI[i];
			if (ValidAction(decision))
			{
				actionMade = true;
				switch (decision.action)
				{
					case Actions.ATTACK:
						currTarget = enemyList[0].transform.position;
						Attack();
						break;
					case Actions.MOVE_BACK:
						Vector3 backTarget = this.transform.position + backMovementMod;
						MoveTowards(backTarget);
						break;
					case Actions.MOVE_FORWARD:
						Vector3 forTarget = this.transform.position + -backMovementMod;
						MoveTowards(forTarget);
						break;
					case Actions.MOVE_ENEMY:
						MoveTowards(enemyList[0].transform.position);
						break;
					case Actions.REST:
						Rest();
						break;
				}
			}
		}
		if (!actionMade)
		{
			Rest();
		}
	}

	protected override void AggressiveStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < aggressiveAI.Count; i++)
		{
			ConditionalItem decision = aggressiveAI[i];
			if (ValidAction(decision))
			{
				actionMade = true;
				switch (decision.action)
				{
					case Actions.ATTACK:
						currTarget = enemyList[0].transform.position;
						Attack();
						break;
					case Actions.MOVE_BACK:
						Vector3 backTarget = this.transform.position + backMovementMod;
						MoveTowards(backTarget);
						break;
					case Actions.MOVE_FORWARD:
						Vector3 forTarget = this.transform.position + -backMovementMod;
						MoveTowards(forTarget);
						break;
					case Actions.MOVE_ENEMY:
						MoveTowards(enemyList[0].transform.position);
						break;
					case Actions.REST:
						Rest();
						break;
				}
			}
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

	protected override void DefensiveStrat()
	{
		bool actionMade = false;
		for (int i = 0; i < defensiveAI.Count; i++)
		{
			ConditionalItem decision = defensiveAI[i];
			if (ValidAction(decision))
			{
				actionMade = true;
				switch (decision.action)
				{
					case Actions.ATTACK:
						currTarget = enemyList[0].transform.position;
						Attack();
						break;
					case Actions.MOVE_BACK:
						Vector3 backTarget = this.transform.position + backMovementMod;
						MoveTowards(backTarget);
						break;
					case Actions.MOVE_FORWARD:
						Vector3 forTarget = this.transform.position + -backMovementMod;
						MoveTowards(forTarget);
						break;
					case Actions.MOVE_ENEMY:
						MoveTowards(enemyList[0].transform.position);
						break;
					case Actions.REST:
						Rest();
						break;
				}
			}
		}
		if (!actionMade)
		{
			Rest();
		}
	}

	#endregion

	protected override void Attack()
	{
		Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
		atkDir.Normalize();
		Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
		GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
		newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, atkCost);
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
