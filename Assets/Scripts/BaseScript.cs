using UnityEngine;
using System.Collections;

public class BaseScript : Mortal
{
	public void Setup(float newStartE, float newMaxE, float newGainRate)
	{
		energy = newStartE;
		maxEnergy = newMaxE;
		gainRate = newGainRate;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<UnitScript>() != null)
		{
			UnitScript u = col.GetComponent<UnitScript>();
			if (u.team != team)
			{
				TakeDamage(col.GetComponent<UnitScript>().energy);
				col.GetComponent<UnitScript>().TakeDamage(1000000);
			}
		}
	}
}
