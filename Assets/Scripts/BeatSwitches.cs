using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

public class BeatSwitches : MonoBehaviour {

    public int team = 0;
    public int beat;
    public Image beatImage;
    public Switch restSwitch;
    public Switch attackSwitch;
    public Switch moveSwitch;
    float waitTime = .6f;

    public delegate void switchedEvent(int team, int beat, UnitScript.Actions action);
    public event switchedEvent SwitchChanged;

    [System.NonSerialized]
    public bool rest;
    [System.NonSerialized]
    public bool attack;
    [System.NonSerialized]
    public bool move;

	// Use this for initialization
	void Awake ()
    {
        rest = true;
        attack = false;
        move = false;
        restSwitch.OnValueChanged.AddListener(RestChange);
        attackSwitch.OnValueChanged.AddListener(AttackChange);
        moveSwitch.OnValueChanged.AddListener(MoveChange);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSwitches();
    }

    void UpdateSwitches()
    {
        restSwitch.IsOn = rest;
        attackSwitch.IsOn = attack;
        moveSwitch.IsOn = move;
    }

    void RestChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (rest && !attack && !move)
        {
            restSwitch.IsOn = true;
        }
        else
        {
            rest = true;
            attack = false;
            move = false;
            SwitchChanged(team, beat, UnitScript.Actions.NONE);
        }
        UpdateSwitches();
    }
    IEnumerator WaitRestChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (rest && !attack && !move)
        {
            restSwitch.IsOn = true;
        }
        else
        {
            rest = true;
            attack = false;
            move = false;
            SwitchChanged(team, beat, UnitScript.Actions.NONE);
        }
        UpdateSwitches();
    }

    void AttackChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (!rest && attack && !move)
        {
            attackSwitch.IsOn = true;
        }
        else
        {
            rest = false;
            attack = true;
            move = false;
            SwitchChanged(team, beat, UnitScript.Actions.ATTACK);
        }
        UpdateSwitches();
    }

    IEnumerator WaitAttackChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (!rest && attack && !move)
        {
            attackSwitch.IsOn = true;
        }
        else
        {
            rest = false;
            attack = true;
            move = false;
            SwitchChanged(team, beat, UnitScript.Actions.ATTACK);
        }
        UpdateSwitches();
    }

    void MoveChange(bool newVal)
    {
        StartCoroutine(WaitAfterAnimation());
        if (!rest && !attack && move)
        {
            moveSwitch.IsOn = true;
        }
        else
        {
            rest = false;
            attack = false;
            move = true;
            SwitchChanged(team, beat, UnitScript.Actions.MOVE);
        }
        UpdateSwitches();
    }

    IEnumerator WaitMoveChange()
    {
        yield return new WaitForSeconds(waitTime);
        if (!rest && !attack && move)
        {
            moveSwitch.IsOn = true;
        }
        else
        {
            rest = false;
            attack = false;
            move = true;
            SwitchChanged(team, beat, UnitScript.Actions.MOVE);
        }
        UpdateSwitches();
    }

    IEnumerator WaitAfterAnimation()
    {
        restSwitch.OnValueChanged.RemoveListener(RestChange);
        attackSwitch.OnValueChanged.RemoveListener(AttackChange);
        moveSwitch.OnValueChanged.RemoveListener(MoveChange);
        yield return new WaitForSeconds(.3f);
        restSwitch.OnValueChanged.AddListener(RestChange);
        attackSwitch.OnValueChanged.AddListener(AttackChange);
        moveSwitch.OnValueChanged.AddListener(MoveChange);
    }
}
