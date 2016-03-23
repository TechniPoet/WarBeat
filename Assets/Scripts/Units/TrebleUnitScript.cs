using UnityEngine;
using System.Collections;
using Actions = ConstFile.Actions;

public class TrebleUnitScript : UnitScript
{
    public GameObject bullet;

	float aggrAtkDist;
	float defAtkDist;

	new void Update()
    {
        base.Update();
		DebugExtension.DebugCircle(transform.position, Vector3.forward, GameManager._TUnit.aggrAtkDist);
	}

    public void TrebleSetup(int newTeam, GameObject target, float newMaxE,
        float newStartE, float newGainRate, float newMoveCost, float newAtkCost,
        float newMoveSpeed, float newAtkSpeed, float newAtkLifeSpan)
    {
        UnitSetup(newTeam, target, newMaxE, newStartE, newGainRate, newMoveCost, newAtkCost,
        newMoveSpeed, newAtkSpeed, newAtkLifeSpan);
		currType = UnitType.TREBLE;

		if (team == 0)
		{
			neutralAI = AIManager.TrebleUnit.NeutralAI;
			aggressiveAI = AIManager.TrebleUnit.AggressiveAI;
			defensiveAI = AIManager.TrebleUnit.DefensiveAI;
		}
		else
		{
			neutralAI = AIManager.EnemyTrebleUnit.NeutralAI;
			aggressiveAI = AIManager.EnemyTrebleUnit.AggressiveAI;
			defensiveAI = AIManager.EnemyTrebleUnit.DefensiveAI;
		}
	}

    
    protected override void Attack()
    {
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        atkDir *= .5f;
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, atkCost);
        TakeDamage(atkCost);
		currAction = Actions.ATTACK;

		
	}
}
