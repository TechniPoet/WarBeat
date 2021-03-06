﻿using UnityEngine;
using System.Collections.Generic;
using GM = GameManager;
using PuppetType = ConstFile.PuppetType;

public class SpawnMaster : MonoBehaviour {
    bool init = false;

    GameObject tUnit;
	GameObject bUnit;

    public GameObject leftBase;
    public GameObject rightBase;

    // Use this for initialization
    void Awake () {
	    if (GM._Init)
        {
            InitSetup();
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (!init && GM._Init)
        {
            InitSetup();
        }
	}

    void InitSetup()
    {
        init = true;
        tUnit = GM._TUnit.unitPrefab;
		bUnit = GM._BUnit.unitPrefab;
        leftBase = GM._LTeam.baseObject;
        rightBase = GM._RTeam.baseObject;
    }

    public void SpawnTrebleL()
    {
		SpawnTreble(0);
    }

    public void SpawnTrebleR()
    {
		SpawnTreble(1);
	}

	public void SpawnTreble(int team)
	{
		Vector2 spawnPoint;
		GameObject attackBase;
		switch (team)
		{
			case 0:
				spawnPoint = GM._LTeam.baseScript.spawner.position;
				attackBase = rightBase;
				GM._LTeam.triggerScript.TakeDamage(GM._TUnit.spawnCost);
				break;
			case 1:
				spawnPoint = GM._RTeam.baseScript.spawner.position;
				attackBase = leftBase;
				GM._RTeam.triggerScript.TakeDamage(GM._TUnit.spawnCost);
				break;
			default:
				return;
		}
		
		GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;

		TrebleUnit u = GM._TUnit;
		newUnit.GetComponent<TrebleUnitScript>().TrebleSetup(team, attackBase, u.maxEnergy, u.startEnergy,
			u.gainEnergyRate, u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
			u.atkLifeSpan);
		newUnit.SetActive(true);

		if (team == 1)
		{
			newUnit.transform.Rotate(new Vector3(0,0, 180));
		}

		GM.AddNewUnit(team, newUnit, PuppetType.TREBLE);
	}


	public void SpawnBass(int team)
	{
		Vector2 spawnPoint;
		GameObject attackBase;
		switch (team)
		{
			case 0:
				spawnPoint = GM._LTeam.baseScript.spawner.position;
				attackBase = rightBase;
				GM._LTeam.triggerScript.TakeDamage(GM._BUnit.spawnCost);
				break;
			case 1:
				spawnPoint = GM._RTeam.baseScript.spawner.position;
				attackBase = leftBase;
				GM._RTeam.triggerScript.TakeDamage(GM._BUnit.spawnCost);
				break;
			default:
				return;
		}
		
		GameObject newUnit = Instantiate(bUnit, spawnPoint, Quaternion.identity) as GameObject;

		BassUnit u = GM._BUnit;
		newUnit.GetComponent<BassUnitScript>().BassSetup(team, attackBase, u.maxEnergy, u.startEnergy,
			u.gainEnergyRate, u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
			u.atkLifeSpan);
		newUnit.SetActive(true);

		if (team == 1)
		{
			newUnit.transform.Rotate(new Vector3(0, 0, 180));
		}

		GM.AddNewUnit(team, newUnit, PuppetType.BASS);
	}
}
