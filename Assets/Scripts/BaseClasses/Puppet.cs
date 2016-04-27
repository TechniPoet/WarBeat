using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PuppetType = ConstFile.PuppetType;


[RequireComponent(typeof(Collider2D))]
public abstract class Puppet : Mortal
{
	public PuppetType currType;
	protected List<GameObject> enemyList = new List<GameObject>();
	public ConstFile.Actions currAction;
	public ConstFile.Notes currNote;
	public static int idCounter;
	public int id;
	public Vector2 gridLocation;

	public GameObject whole;
	public GameObject half;
	public GameObject quarter;
	public GameObject eigth;
	public GameObject sixteenth;

	public delegate void AddEnergy(float amt, int team);
	public static event AddEnergy UnitDied;

	protected virtual void Start()
	{
		GameManager.AddUnit += EnemyAdd;
		GameManager.RemoveUnit += EnemyRemove;
		DeathMethod();
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();
		DeathMethod();
	}

	#region House Keeping 
	/*
	Shouldn't need to touch these much, mostly automated by events.
	*/

	protected void EnemyAdd(int puppetTeam, GameObject enemy, PuppetType type)
	{
		if (puppetTeam != this.team)
		{
			enemyList.Add(enemy);
		}
	}

	protected void EnemyRemove(int eTeam, GameObject enemy, PuppetType type)
	{
		if (eTeam != this.team)
		{
			enemyList.Remove(enemy);
		}
	}

	#endregion

	#region Abstract Methods

	protected abstract void Attack();

	public virtual void MakeMove(PlayInstructs instrux)
	{
		ResetNotes();
		Color col = GetComponentInChildren<SpriteRenderer>().color;
		GetComponentInChildren<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 1);
	}

	public virtual PlayInstructs CurrInstruction()
	{
		Color col = GetComponentInChildren<SpriteRenderer>().color;
		GetComponentInChildren<SpriteRenderer>().color = new Color(col.r, col.g, col.b, .5f);
		return null;
	}

	protected abstract void Rest();

	#endregion

	#region Helper Functions

	protected virtual void SortEnemyList()
	{
		enemyList.Sort((x, y) => CalcUtil.DistCompare(this.gameObject, x, y));
	}

	#endregion

	public virtual void Setup(int newTeam)
	{
		team = newTeam;
		gridLocation = ArenaGrid.ClosestGridPoint(transform.position);
		transform.position = ArenaGrid.GridToWorldPos(gridLocation);
		id = idCounter++;
	}
	

	protected virtual void DeathMethod()
	{
		if (energy <= 0)
		{
			if (team == 0)
			{
				UnitDied(maxEnergy / 20, 0);
				UnitDied(maxEnergy / 17, 1);
			}
			else
			{
				UnitDied(maxEnergy / 17, 0);
				UnitDied(maxEnergy / 20, 1);
			}
			GameManager.AddUnit -= EnemyAdd;
			GameManager.RemoveUnit -= EnemyRemove;
			GameManager.RemoveDeadUnit(team, this.gameObject, currType);
			Destroy(this.gameObject);
		}
	}

	protected void SetNote()
	{
		switch(currNote)
		{
			case ConstFile.Notes.SIXTEENTH:
				sixteenth.SetActive(true);
				break;
			case ConstFile.Notes.EIGHTH:
				eigth.SetActive(true);
				break;
			case ConstFile.Notes.QUARTER:
				quarter.SetActive(true);
				break;
			case ConstFile.Notes.HALF:
				half.SetActive(true);
				break;
			case ConstFile.Notes.WHOLE:
				whole.SetActive(true);
				break;
		}
	}

	protected void ResetNotes()
	{
		sixteenth.SetActive(false);
		eigth.SetActive(false);
		quarter.SetActive(false);
		half.SetActive(false);
		whole.SetActive(false);
	}
}
