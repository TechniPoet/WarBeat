using UnityEngine;
using System.Collections;

public class BaseScript : Mortal
{
	

	public void Setup(float newStartE, float newMaxE, float newGainRate)
	{
		energy = newStartE;
		maxEnergy = newMaxE;
		gainRate = newGainRate;
		Puppet.UnitDied += AddDeathEnergy;
	}


	protected override void Update()
	{
		base.Update();
		energy += Time.deltaTime * gainRate;
		energy = Mathf.Clamp(energy, -1f, MaxEnergy);
	}

	void AddDeathEnergy(float amt, int addTeam)
	{
		if (team == addTeam)
		{
			energy += amt;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<UnitScript>() != null)
		{
			UnitScript u = col.GetComponent<UnitScript>();
			if (u.team != team)
			{
				Debug.Log("Damage to " + team);
				TakeDamage(col.GetComponent<UnitScript>().energy);
				col.GetComponent<UnitScript>().TakeDamage(1000000);
			}
		}
	}
}
