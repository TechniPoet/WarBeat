using UnityEngine;
using System.Collections.Generic;
using Actions = ConstFile.Actions;
using PuppetType = ConstFile.PuppetType;

[SelectionBase]
public class BassUnitScript : UnitScript
{
	public GameObject bullet;
	
	float aggrAtkDist;
	float defAtkDist;
	

	public void BassSetup(int newTeam, GameObject target, float newMaxE,
		float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
		float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
	{
		Setup(newTeam, target, newMaxE, newStartE, newGainRate, newMoveCost, newAtkCost,
		newMoveSpeed, newAtkSpeed, newAtkLifeSpan);
		currType = PuppetType.BASS;
		AIManager.UnitAIs u;
		if (team == 0)
		{
			u = AIManager.BassUnit;
		}
		else
		{
			u = AIManager.EnemyBassUnit;
		}
		neutralAI = u.NeutralAI;
		aggressiveAI = u.AggressiveAI;
		defensiveAI = u.DefensiveAI;
	}




	protected override void Attack()
	{
		Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
		atkDir.Normalize();
		Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
		GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
		newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, atkCost * NoteMult);
		TakeDamage(atkCost * NoteMult);
		currAction = Actions.ATTACK;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
		DebugExtension.DebugCircle(transform.position, Vector3.forward, GameManager._BUnit.aggrAtkDist);
	}
}
