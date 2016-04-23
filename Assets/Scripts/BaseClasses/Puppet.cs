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

	public abstract void MakeMove(PlayInstructs instrux);

	public abstract PlayInstructs CurrInstruction();

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
			GameManager.AddUnit -= EnemyAdd;
			GameManager.RemoveUnit -= EnemyRemove;
			GameManager.RemoveDeadUnit(team, this.gameObject, currType);
			Destroy(this.gameObject);
		}
	}

	
}
