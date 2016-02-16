using UnityEngine;
using System.Collections.Generic;
using GM = GameManager;

public class SpawnMaster : MonoBehaviour {
    bool init = false;

    public GameObject tUnit;

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
        leftBase = GM._LTeam.baseObject;
        rightBase = GM._RTeam.baseObject;
    }

    public void SpawnTrebleL()
    {
		SpawnTreble(leftBase, 0);
    }

    public void SpawnTrebleR()
    {
		SpawnTreble(rightBase, 1);
	}

	void SpawnTreble(GameObject hBase, int team)
	{
		Vector2 spawnPoint;
		GameObject attackBase;
		switch (team)
		{
			case 0:
				spawnPoint = GM._LTeam.baseScript.spawner.position;
				attackBase = rightBase;
				break;
			case 1:
				spawnPoint = GM._RTeam.baseScript.spawner.position;
				attackBase = leftBase;
				break;
			default:
				return;
		}

		hBase.GetComponent<StatueScript>().energy -= GM._TUnit.spawnCost;
		GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;

		TrebleUnit u = GM._TUnit;
		newUnit.GetComponent<TrebleUnitScript>().UnitSetup(team, attackBase, u.maxEnergy, u.startEnergy,
			u.gainEnergyRate, u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
			u.atkLifeSpan);
		newUnit.SetActive(true);

		if (team == 1)
		{
			newUnit.transform.Rotate(new Vector3(0,0, 180));
		}

		GM.AddNewUnit(team, ref newUnit);
	}
}
