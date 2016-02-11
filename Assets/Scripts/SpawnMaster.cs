using UnityEngine;
using System.Collections.Generic;
using GM = GameManager;

public class SpawnMaster : MonoBehaviour {
    bool init = false;

    public GameObject tUnit;
    public List<GameObject> spawnPointsZero;
    public List<GameObject> spawnPointsOne;

    public GameObject zeroBase;
    public GameObject oneBase;

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
        zeroBase = GM._LTeam.baseObject;
        oneBase = GM._RTeam.baseObject;
    }

    public void SpawnZero()
    {
        zeroBase.GetComponent<StatueScript>().energy -= GM._TUnit.spawnCost;
        Vector2 spawnPoint = spawnPointsZero[Random.Range(0, spawnPointsZero.Count)].transform.position;
        GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;
        TrebleUnit u = GM._TUnit;
        newUnit.GetComponent<UnitScript>().Setup(0, oneBase, u.maxEnergy, u.startEnergy,
            u.gainEnergyRate,u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
            u.atkLifeSpan);
        newUnit.SetActive(true);
        GM.AddNewUnit(0, ref newUnit);
    }

    public void SpawnOne()
    {
        oneBase.GetComponent<StatueScript>().energy -= GM._TUnit.spawnCost;
        Vector2 spawnPoint = spawnPointsOne[Random.Range(0, spawnPointsOne.Count - 1)].transform.position;
        GameObject newUnit = Instantiate(tUnit, spawnPoint, Quaternion.identity) as GameObject;
        TrebleUnit u = GM._TUnit;
        newUnit.GetComponent<UnitScript>().Setup(1, zeroBase, u.maxEnergy, u.startEnergy,
            u.gainEnergyRate, u.moveCost, u.eigthAtkCost, u.moveSpeed, u.atkSpeed,
            u.atkLifeSpan);
        newUnit.SetActive(true);
        GM.AddNewUnit(1, ref newUnit);
    }
}
