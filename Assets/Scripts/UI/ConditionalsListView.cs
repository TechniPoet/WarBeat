using UnityEngine;
using UIWidgets;
using System.Collections;

public class ConditionalsListView : ListViewCustom<ConditionalListViewComponent, ConditionalItem>
{
	public ConditionalEditBox editBox;
	bool updateVals = false;

	public override void Start()
	{
		editBox = transform.parent.GetComponentInChildren<ConditionalEditBox>();
		base.Start();
		OnSelect.AddListener(NewSelect);
	}

	void Update()
	{
		if (SelectedIndex != -1 && updateVals)
		{
			DataSource[SelectedIndex].action = editBox.tempItem.action;
			DataSource[SelectedIndex].cond1Ind = editBox.tempItem.cond1Ind;
			DataSource[SelectedIndex].cond1Val = editBox.tempItem.cond1Val;
			DataSource[SelectedIndex].cond2Ind = editBox.tempItem.cond2Ind;
			DataSource[SelectedIndex].cond2Val = editBox.tempItem.cond2Val;
			DataSource[SelectedIndex].greater = editBox.tempItem.greater;
			DataSource[SelectedIndex].note = editBox.tempItem.note;
		}
	}

	void NewSelect(int i, ListViewItem x)
	{
		updateVals = false;
		ConditionalListViewComponent sel = (ConditionalListViewComponent) x;
		editBox.SetVals(sel.item);
		updateVals = true;
	}

	public void Addnew()
	{
		ConditionalItem newItem = new ConditionalItem();
		Add(newItem);
	}

	protected override void SetData(ConditionalListViewComponent component, ConditionalItem item)
	{
		component.SetData(item);
	}

	public void RemoveSelected(ConditionalListViewComponent curr)
	{
		Remove(curr.item);
	}

	public void MoveUp(ConditionalListViewComponent curr)
	{
		int ind = DataSource.IndexOf(curr.item);
		if (ind > 0)
		{
			ConditionalItem temp = DataSource[ind - 1];
			DataSource[ind - 1] = curr.item;
			DataSource[ind] = temp;
		}
	}

	public void MoveDown(ConditionalListViewComponent curr)
	{
		int ind = DataSource.IndexOf(curr.item);
		if (ind < DataSource.Count - 1)
		{
			ConditionalItem temp = DataSource[ind + 1];
			DataSource[ind + 1] = curr.item;
			DataSource[ind] = temp;
		}
	}

	protected override void OnDestroy()
	{
		OnSelect.RemoveListener(NewSelect);
	}
}
