using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PuppetType = ConstFile.PuppetType;

[System.Serializable]
public class TeamBase
{
    public GameObject baseObject;
    [System.NonSerialized]
    public StatueScript baseScript;
	public BaseScript triggerScript;
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

[System.Serializable]
public class BassUnit
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
	public Button bassButton;
	public Button trebleButton;
	
    public static TrebleUnit _TUnit;
	public static BassUnit _BUnit;
    public static TeamBase _LTeam;
    public static TeamBase _RTeam;

    public static Transform _Top;
    public static Transform _Bottom;
    public static Transform _Left;
    public static Transform _Right;
    public static int _ArenaDiv;

    public delegate void UnitDel(int team, GameObject unit, PuppetType type);
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
	public BassUnit bUnit;
    
    [Header("Teams")]
    public TeamBase leftTeam;
    public TeamBase rightTeam;

    public static List<GameObject> _LeftUnits;
	public static List<GameObject> _LeftBass;
	public static List<GameObject> _LeftTreble;


    public static List<GameObject> _RightUnits;
	public static List<GameObject> _RightBass;
	public static List<GameObject> _RightTreble;

	public static List<UnitScript> _LeftUnitBass;
	public static List<UnitScript> _LeftUnitTreble;
	public static List<UnitScript> _RightUnitBass;
	public static List<UnitScript> _RightUnitTreble;

	// Use this for initialization
	void Awake ()
    {
		ArenaGrid.Instance.GenerateGrid();
        _LeftUnits = new List<GameObject>();
		_LeftBass = new List<GameObject>();
		_LeftTreble = new List<GameObject>();
		_RightUnits = new List<GameObject>();
		_RightBass = new List<GameObject>();
		_RightTreble = new List<GameObject>();

		_RightUnitBass = new List<UnitScript>();
		_RightUnitTreble = new List<UnitScript>();
		_LeftUnitBass = new List<UnitScript>();
		_LeftUnitTreble = new List<UnitScript>();

		UpdateStatics();
        _Init = true;
        leftTeam.triggerScript.Setup(leftTeam.baseStartEnergy, leftTeam.baseMaxEnergy, leftTeam.energyGainPerSecond);
        rightTeam.triggerScript.Setup(rightTeam.baseStartEnergy, rightTeam.baseMaxEnergy, rightTeam.energyGainPerSecond);
    }

    void UpdateStatics()
    {
		_TUnit = tUnit;
		_BUnit = bUnit;
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
		
		if (leftTeam.triggerScript.energy <= 0 || rightTeam.triggerScript.energy <= 0)
        {
            SceneManager.LoadScene("AISetup");
        }
		bassButton.interactable = (leftTeam.triggerScript.energy - _BUnit.spawnCost) > 0;
		trebleButton.interactable = (leftTeam.triggerScript.energy - _TUnit.spawnCost) > 0;

		
		
	}

    

    // Adds new unit to proper list and sends event to all listeners.
    public static void AddNewUnit(int team, GameObject unit, PuppetType type)
    {
		switch (team)
        {
            case 0:
                _LeftUnits.Add(unit);
				switch (type)
				{
					case PuppetType.BASS:
						_LeftBass.Add(unit);
						_LeftUnitBass.Add(unit.GetComponent<UnitScript>());
						break;
					case PuppetType.TREBLE:
						_LeftTreble.Add(unit);
						_LeftUnitTreble.Add(unit.GetComponent<UnitScript>());
						break;
				}
                break;
            case 1:
                _RightUnits.Add(unit);
				switch (type)
				{
					case PuppetType.BASS:
						_RightBass.Add(unit);
						_RightUnitBass.Add(unit.GetComponent<UnitScript>());
						break;
					case PuppetType.TREBLE:
						_RightTreble.Add(unit);
						_RightUnitTreble.Add(unit.GetComponent<UnitScript>());
						break;
				}
				break;
        }
		
		if (AddUnit != null)
		{
			AddUnit(team, unit, type);
		}
		
    }

    public static void RemoveDeadUnit(int team, GameObject unit, PuppetType type)
    {
		int ind = -1;
		switch (team)
        {
            case 0:
                _LeftUnits.Remove(unit);
				
				switch (type)
				{
					case PuppetType.BASS:
						for (int i = 0; i < _LeftBass.Count; i++)
						{
							if (_LeftBass[i] == unit)
							{
								ind = i;
							}
						}
						_LeftBass.RemoveAt(ind);
						_LeftUnitBass.RemoveAt(ind);
						break;
					case PuppetType.TREBLE:
						for (int i = 0; i < _LeftTreble.Count; i++)
						{
							if (_LeftTreble[i] == unit)
							{
								ind = i;
							}
						}
						_LeftTreble.RemoveAt(ind);
						_LeftUnitTreble.RemoveAt(ind);
						break;
				}
				break;
            case 1:
                _RightUnits.Remove(unit);
				switch (type)
				{
					case PuppetType.BASS:
						for (int i = 0; i < _RightBass.Count; i++)
						{
							if (_RightBass[i] == unit)
							{
								ind = i;
							}
						}
						_RightBass.RemoveAt(ind);
						_RightUnitBass.RemoveAt(ind);
						break;
					case PuppetType.TREBLE:
						for (int i = 0; i < _RightTreble.Count; i++)
						{
							if (_RightTreble[i] == unit)
							{
								ind = i;
							}
						}
						_RightTreble.RemoveAt(ind);
						_RightUnitTreble.RemoveAt(ind);
						break;
				}
				break;
        }
		if (RemoveUnit != null)
		{
			RemoveUnit(team, unit, type);
		}
    }

	
}
