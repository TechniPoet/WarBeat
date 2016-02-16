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
        leftBase.GetComponent<StatueScript>().energy -= GM._TUnit.spawnCost;

        Vector2 spawnPoint = GM._LTeam.baseScript.spawner.position;
        GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;

        TrebleUnit u = GM._TUnit;
        newUnit.GetComponent<TrebleUnitScript>().UnitSetup(0, rightBase, u.maxEnergy, u.startEnergy,
            u.gainEnergyRate,u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
            u.atkLifeSpan);
        newUnit.SetActive(true);

        GM.AddNewUnit(0, ref newUnit);
    }

    public void SpawnTrebleR()
    {
        rightBase.GetComponent<StatueScript>().energy -= GM._TUnit.spawnCost;

        Vector2 spawnPoint = GM._RTeam.baseScript.spawner.position;
        GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;

        TrebleUnit u = GM._TUnit;
        newUnit.GetComponent<TrebleUnitScript>().UnitSetup(1, leftBase, u.maxEnergy, u.startEnergy,
            u.gainEnergyRate, u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
            u.atkLifeSpan);
        newUnit.SetActive(true);

        GM.AddNewUnit(1, ref newUnit);
    }
}
