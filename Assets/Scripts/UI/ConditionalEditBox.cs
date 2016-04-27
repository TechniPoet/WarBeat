using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections;

public class ConditionalEditBox : MonoBehaviour {

	public ValDropdown cond1;
	public Dropdown condBool;
	public ValDropdown cond2;
	public Dropdown actionDropdown;
	public Dropdown noteDropdown;
	public ConditionalItem tempItem;

	// Use this for initialization
	void Start () {
		actionDropdown.ClearOptions();
		for (int i = 0; i < ConstFile.ActionsText.Length; i++)
		{
			Dropdown.OptionData opt = new Dropdown.OptionData(ConstFile.ActionsText[i]);
			actionDropdown.options.Add(opt);
		}
		noteDropdown.ClearOptions();
		for (int i = 0; i < 5; i++)
		{
			Dropdown.OptionData opt = new Dropdown.OptionData(((ConstFile.Notes)i).ToString());
			noteDropdown.options.Add(opt);
		}
	}

	void Update()
	{
		tempItem = new ConditionalItem();
		tempItem.action = (ConstFile.Actions)actionDropdown.value;
		tempItem.cond1Ind = (ConstFile.ConditionOptions)cond1.varCheck.value;
		tempItem.cond2Ind = (ConstFile.ConditionOptions)cond2.varCheck.value;
		tempItem.cond1Val = cond1.currVal;
		tempItem.cond2Val = cond2.currVal;
		tempItem.greater = condBool.value == 0 ? true : false;
		tempItem.note = (ConstFile.Notes) noteDropdown.value;
	}

	public void SetVals(ConditionalItem newItem)
	{
		cond1.varCheck.value = (int)newItem.cond1Ind;
		cond2.varCheck.value = (int)newItem.cond2Ind;
		cond1.spin.Value = newItem.cond1Val;
		cond2.spin.Value = newItem.cond2Val;
		condBool.value = newItem.greater ? 0 : 1;
		actionDropdown.value = (int)newItem.action;
		noteDropdown.value = (int)newItem.note;
	}
}
