using UnityEngine;
using System.Collections;
using PuppetType = ConstFile.PuppetType;
using Actions = ConstFile.Actions;

public class PlayInstructs
{
	public Actions action;
	public ConstFile.Notes note;
	public float y;
	Puppet unit;
	public PlayInstructs(Actions newAct, ConstFile.Notes newNote, float yPos, Puppet u)
	{
		action = newAct;
		note = newNote;
		y = yPos;
		unit = u;
	}

	public PuppetType puppetType()
	{
		return unit.currType;
	}

	public bool Alive()
	{
		return unit != null;
	}
	public void MakeMove()
	{
		unit.MakeMove(this);
	}
}
