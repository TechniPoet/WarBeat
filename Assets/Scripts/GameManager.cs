using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class TeamBase
{
    public GameObject baseObject;
    [System.NonSerialized]
    public StatueScript baseScript;
    public float baseMaxEnergy;
    public float baseStartEnergy;
    public float energyGainPerSecond;
}

[System.Serializable]
public class TrebleUnit
{
    public GameObject unitPrefab;
    public float spawnCost;
    public float maxEnergy;
    public float startEnergy;
    public float gainEnergyRate;
}

public class GameManager : MonoBehaviour
{
    public static TrebleUnit _TUnit;
    public static TeamBase _LTeam;
    public static TeamBase _RTeam;


    public static bool _Init = false;
    public const string UnitTag = "Player";
    public const string StatueTag = "Finish";

    [Header("Units")]
    public TrebleUnit tUnit;
    
    [Header("Teams")]
    public TeamBase leftTeam;
    public TeamBase rightTeam;


	// Use this for initialization
	void Awake ()
    {
        
        UpdateStatics();
        _Init = true;
        leftTeam.baseScript.Setup(leftTeam.baseStartEnergy, leftTeam.baseMaxEnergy, leftTeam.energyGainPerSecond);
        rightTeam.baseScript.Setup(rightTeam.baseStartEnergy, rightTeam.baseMaxEnergy, rightTeam.energyGainPerSecond);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (leftTeam.baseScript.energy <= 0 || rightTeam.baseScript.energy <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

    void UpdateStatics()
    {
        _TUnit = tUnit;
        leftTeam.baseScript = leftTeam.baseObject.GetComponent<StatueScript>();
        rightTeam.baseScript = rightTeam.baseObject.GetComponent<StatueScript>();
        _LTeam = leftTeam;
        _RTeam = rightTeam;
    }
}
