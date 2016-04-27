using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections;

public class ConditionalListViewComponent : ListViewItem
{
	[SerializeField]
	public Text condText;
	[SerializeField]
	public ConditionalItem item;

	protected override void Start()
	{
		//item = new ConditionalItem();
		base.Start();
	}

	void Update()
	{
		condText.text = item.GetSentence();
	}

	public void SetData(ConditionalItem newItem)
	{
		item = newItem;
	}
}
