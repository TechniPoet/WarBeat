using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public float maxEnergy;
    public float startEnergy;
    public float gainEnergyRate;
    [Header("Costs")]
    public float spawnCost;
    public float moveCost;
    public float eigthAtkCost;
    [Header("Speeds")]
    public float moveSpeed;
    public float atkSpeed;
    [Header("Timers")]
    public float atkLifeSpan;
    [Header("Limits")]
    public float aggrAtkDist;
    public float defAtkDist;
}

public class GameManager : MonoBehaviour
{
    public static TrebleUnit _TUnit;
    public static TeamBase _LTeam;
    public static TeamBase _RTeam;

    public static Transform _Top;
    public static Transform _Bottom;
    public static Transform _Left;
    public static Transform _Right;
    public static int _ArenaDiv;

    public delegate void UnitDel(int team, ref GameObject unit);
    public static event UnitDel AddUnit;
    public static event UnitDel RemoveUnit;

    public static bool _Init = false;
    public const string UnitTag = "Player";
    public const string StatueTag = "Finish";

    public Transform top;
    public Transform bottom;
    public Transform left;
    public Transform right;
    public int arenaDiv;
    public bool basesOnMeasures = true;
    public static bool _BaseMoveOnMeasure;

    [Header("Units")]
    public TrebleUnit tUnit;
    
    [Header("Teams")]
    public TeamBase leftTeam;
    public TeamBase rightTeam;

    public static List<GameObject> _LeftUnits;
    public static List<GameObject> _RightUnits;


	// Use this for initialization
	void Awake ()
    {
        _LeftUnits = new List<GameObject>();
        _RightUnits = new List<GameObject>();
        UpdateStatics();
        _Init = true;
        leftTeam.baseScript.Setup(leftTeam.baseStartEnergy, leftTeam.baseMaxEnergy, leftTeam.energyGainPerSecond);
        rightTeam.baseScript.Setup(rightTeam.baseStartEnergy, rightTeam.baseMaxEnergy, rightTeam.energyGainPerSecond);
    }

    void UpdateStatics()
    {
        _TUnit = tUnit;
        leftTeam.baseScript = leftTeam.baseObject.GetComponent<StatueScript>();
        rightTeam.baseScript = rightTeam.baseObject.GetComponent<StatueScript>();
        _LTeam = leftTeam;
        _RTeam = rightTeam;

        _Top = top;
        _Bottom = bottom;
        _Left = left;
        _Right = right;
        _ArenaDiv = arenaDiv;
        _BaseMoveOnMeasure = basesOnMeasures;
    }


    // Update is called once per frame
    void Update () {
        
        if (leftTeam.baseScript.energy <= 0 || rightTeam.baseScript.energy <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

    

    // Adds new unit to proper list and sends event to all listeners.
    public static void AddNewUnit(int team, ref GameObject unit)
    {
        switch(team)
        {
            case 0:
                _LeftUnits.Add(unit);
                break;
            case 1:
                _RightUnits.Add(unit);
                break;
        }
        AddUnit(team, ref unit);
    }

    public static void RemoveDeadUnit(int team, GameObject unit)
    {
        switch (team)
        {
            case 0:
                _LeftUnits.Remove(unit);
                break;
            case 1:
                _RightUnits.Remove(unit);
                break;
        }
        RemoveUnit(team, ref unit);
    }
}
