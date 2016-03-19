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
	public List<ConditionalItem> AggressiveList
	{
		get { return aggressiveList; }
		set
		{
			if (value == null)
			{
				aggressiveList = new List<ConditionalItem>();
			}
			else
			{
				aggressiveList = value;
				if (currMode == ConstFile.AIModes.AGGRESSIVE)
				{
					UpdateVisibleList(aggressiveList);
				}
			}
		}
	}
	List<ConditionalItem> defensiveList;
	public List<ConditionalItem> DefensiveList
	{
		get { return defensiveList; }
		set
		{
			if (value == null)
			{
				defensiveList = new List<ConditionalItem>();
			}
			else
			{
				defensiveList = value;
				if (currMode == ConstFile.AIModes.DEFENSIVE)
				{
					UpdateVisibleList(defensiveList);
				}
			}
		}
	}
	List<ConditionalItem> neutralList;
	public List<ConditionalItem> NeutralList
	{
		get { return neutralList;  }
		set
		{
			if (value == null)
			{
				neutralList = new List<ConditionalItem>();
			}
			else
			{
				neutralList = value;
				if (currMode == ConstFile.AIModes.NEUTRAL)
				{
					UpdateVisibleList(neutralList);
				}
			}
		}
	}

	// Use this for initialization
	void Start()
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

	public void ActivateAggressive()
	{
		StoreCurrentList();
		currMode = ConstFile.AIModes.AGGRESSIVE;
		aggressiveButton.interactable = false;
		defensiveButton.interactable = true;
		neutralButton.interactable = true;
		for (int i = 0; i < aggressiveList.Count; i++)
		{
			List.Add(aggressiveList[i]);
		}
	}

	public void ActivateDefensive()
	{
		StoreCurrentList();
		currMode = ConstFile.AIModes.DEFENSIVE;
		aggressiveButton.interactable = true;
		defensiveButton.interactable = false;
		neutralButton.interactable = true;
		for (int i = 0; i < defensiveList.Count; i++)
		{
			List.Add(defensiveList[i]);
		}
	}

	public void ActivateNeutral()
	{
		StoreCurrentList();
		currMode = ConstFile.AIModes.NEUTRAL;
		aggressiveButton.interactable = true;
		defensiveButton.interactable = true;
		neutralButton.interactable = false;
		for (int i = 0; i < neutralList.Count; i++)
		{
			List.Add(neutralList[i]);
		}
	}

	public void StoreCurrentList()
	{
		if (!defensiveButton.interactable)
		{
			defensiveList = new List<ConditionalItem>();
			for (int i = 0; i < List.DataSource.Count; i++)
			{
				defensiveList.Add(List.DataSource[i]);
			}
		}
		else if (!aggressiveButton.interactable)
		{
			aggressiveList = new List<ConditionalItem>();
			for (int i = 0; i < List.DataSource.Count; i++)
			{
				aggressiveList.Add(List.DataSource[i]);
			}
		}
		else
		{
			neutralList = new List<ConditionalItem>();
			for (int i = 0; i < List.DataSource.Count; i++)
			{
				neutralList.Add(List.DataSource[i]);
			}
		}
		List.DataSource.Clear();
	}

	void UpdateVisibleList(List<ConditionalItem> newList)
	{
		for (int i = 0; i < newList.Count; i++)
		{
			List.Add(newList[i]);
		}
	}
}
