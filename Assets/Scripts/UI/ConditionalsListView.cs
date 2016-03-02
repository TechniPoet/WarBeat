using UnityEngine;
using UIWidgets;
using System.Collections;

public class ConditionalsListView : ListViewCustom<ConditionalListViewComponent, ConditionalItem>
{
	public ConditionalEditBox editBox;
	int cnt = 0;

	public void Addnew()
	{
		ConditionalItem newItem = new ConditionalItem(cnt++);
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
		//int ind = DataSource.IndexOf();
	}
}
