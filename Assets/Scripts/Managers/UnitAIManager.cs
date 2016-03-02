using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitAIManager : MonoBehaviour
{
	public ConditionalsListView List;
	public Button aggressiveButton;
	public Button defensiveButton;
	public Button neutralButton;
	ConstFile.AIModes currMode = ConstFile.AIModes.AGGRESSIVE;

	List<ConditionalItem> aggressiveList;
	List<ConditionalItem> defensiveList;

	// Use this for initialization
	void Start ()
	{
		aggressiveButton.interactable = false;
		defensiveButton.interactable = true;
		neutralButton.interactable = true;

		aggressiveButton.onClick.AddListener(ActivateAggressive);
		defensiveButton.onClick.AddListener(ActivateDefensive);
		neutralButton.onClick.AddListener(ActivateNeutral);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ActivateAggressive()
	{
		currMode = ConstFile.AIModes.AGGRESSIVE;
		aggressiveButton.interactable = false;
		defensiveButton.interactable = true;
		neutralButton.interactable = true;
	}

	void ActivateDefensive()
	{
		currMode = ConstFile.AIModes.DEFENSIVE;
		aggressiveButton.interactable = true;
		defensiveButton.interactable = false;
		neutralButton.interactable = true;
	}

	void ActivateNeutral()
	{
		currMode = ConstFile.AIModes.NEUTRAL;
		aggressiveButton.interactable = true;
		defensiveButton.interactable = true;
		neutralButton.interactable = false;
	}
}
