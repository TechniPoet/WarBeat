using UnityEngine;
using System.Collections.Generic;

public class SpawnMaster : MonoBehaviour {
    public GameObject unit;

    public List<GameObject> spawnPointsZero;
    public List<GameObject> spawnPointsOne;

    public GameObject zeroBase;
    public GameObject oneBase;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SpawnZero()
    {
        Vector2 spawnPoint = spawnPointsZero[Random.Range(0, spawnPointsZero.Count)].transform.position;
        GameObject newUnit = Instantiate(unit, spawnPoint, Quaternion.identity) as GameObject;
        newUnit.GetComponent<UnitScript>().Setup(0, oneBase);
        newUnit.SetActive(true);
    }

    public void SpawnOne()
    {
        Vector2 spawnPoint = spawnPointsOne[Random.Range(0, spawnPointsOne.Count - 1)].transform.position;
        GameObject newUnit = Instantiate(unit, spawnPoint, Quaternion.identity) as GameObject;
        newUnit.GetComponent<UnitScript>().Setup(1, zeroBase);
        newUnit.SetActive(true);
    }
}
