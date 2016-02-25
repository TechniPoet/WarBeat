using UnityEngine;
using UIWidgets;
using System.Collections;

public class ConditionalsListView : ListViewCustom<ConditionalListViewComponent, ConditionalItem>
{
	public void Addnew()
	{
		ConditionalItem newItem = new ConditionalItem();
		Add(newItem);
	}
}
