using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

public class BeatSwitches : MonoBehaviour {

    public int team = 0;
    public int beat;
	[SerializeField]
	public UnitScript.UnitType type;
    public Image beatImage;
    public Switch neutralSwitch;
    public Switch defensiveSwitch;
    public Switch aggressiveSwitch;
    float waitTime = .6f;

    public delegate void switchedEvent(int team, int beat, UnitScript.Strategies action, UnitScript.UnitType type);
    public event switchedEvent SwitchChanged;

    [System.NonSerialized]
    public bool neutral;
    [System.NonSerialized]
    public bool defensive;
    [System.NonSerialized]
    public bool aggressive;

	// Use this for initialization
	void Awake ()
    {
        neutral = true;
        defensive = false;
        aggressive = false;
        neutralSwitch.OnValueChanged.AddListener(RestChange);
        defensiveSwitch.OnValueChanged.AddListener(AttackChange);
        aggressiveSwitch.OnValueChanged.AddListener(MoveChange);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSwitches();
    }

    void UpdateSwitches()
    {
        neutralSwitch.IsOn = neutral;
        defensiveSwitch.IsOn = defensive;
        aggressiveSwitch.IsOn = aggressive;
    }

    void RestChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (neutral && !defensive && !aggressive)
        {
            neutralSwitch.IsOn = true;
        }
        else
        {
            neutral = true;
            defensive = false;
            aggressive = false;
            SwitchChanged(team, beat, UnitScript.Strategies.NEUTRAL, type);
        }
        UpdateSwitches();
    }
    IEnumerator WaitRestChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (neutral && !defensive && !aggressive)
        {
            neutralSwitch.IsOn = true;
        }
        else
        {
            neutral = true;
            defensive = false;
            aggressive = false;
            SwitchChanged(team, beat, UnitScript.Strategies.NEUTRAL, type);
        }
        UpdateSwitches();
    }

    void AttackChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (!neutral && defensive && !aggressive)
        {
            defensiveSwitch.IsOn = true;
        }
        else
        {
            neutral = false;
            defensive = true;
            aggressive = false;
            SwitchChanged(team, beat, UnitScript.Strategies.DEFENSIVE, type);
        }
        UpdateSwitches();
    }

    IEnumerator WaitAttackChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (!neutral && defensive && !aggressive)
        {
            defensiveSwitch.IsOn = true;
        }
        else
        {
            neutral = false;
            defensive = true;
            aggressive = false;
            SwitchChanged(team, beat, UnitScript.Strategies.DEFENSIVE, type);
        }
        UpdateSwitches();
    }

    void MoveChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (!neutral && !defensive && aggressive)
        {
            aggressiveSwitch.IsOn = true;
        }
        else
        {
            neutral = false;
            defensive = false;
            aggressive = true;
            SwitchChanged(team, beat, UnitScript.Strategies.AGGRESSIVE, type);
        }
        UpdateSwitches();
    }

    IEnumerator WaitMoveChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (!neutral && !defensive && aggressive)
        {
            aggressiveSwitch.IsOn = true;
        }
        else
        {
            neutral = false;
            defensive = false;
            aggressive = true;
            SwitchChanged(team, beat, UnitScript.Strategies.AGGRESSIVE, type);
        }
        UpdateSwitches();
    }

    IEnumerator WaitAfterAnimation()
    {
        neutralSwitch.OnValueChanged.RemoveListener(RestChange);
        defensiveSwitch.OnValueChanged.RemoveListener(AttackChange);
        aggressiveSwitch.OnValueChanged.RemoveListener(MoveChange);
        yield return new WaitForSeconds(.3f);
        neutralSwitch.OnValueChanged.AddListener(RestChange);
        defensiveSwitch.OnValueChanged.AddListener(AttackChange);
        aggressiveSwitch.OnValueChanged.AddListener(MoveChange);
    }
}
