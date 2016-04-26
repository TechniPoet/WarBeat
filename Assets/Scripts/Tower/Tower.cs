using UnityEngine;
using System.Collections;
using System;
using Actions = ConstFile.Actions;

public class Tower : Puppet
{
	public GameObject bullet;
	float atkCost = 5;
	float attackSpeed = 2;
	public SpriteRenderer sprite1;
	public SpriteRenderer sprite2;

	protected int NoteMult
	{
		get
		{
			switch (currNote)
			{
				case ConstFile.Notes.SIXTEENTH:
					return 1;
				case ConstFile.Notes.EIGHTH:
					return 2;
				case ConstFile.Notes.QUARTER:
					return 4;
				case ConstFile.Notes.HALF:
					return 8;
				case ConstFile.Notes.WHOLE:
					return 16;

				default:
					return 1;
			}
		}
	}

	Vector2 currTarget;

	float atkDist = 20;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		Setup(1);
		maxEnergy = 200;
		GameManager.AddNewUnit(team, gameObject, ConstFile.PuppetType.TOWER);

		switch (team)
		{
			case 0:
				sprite1.color = Color.blue;
				sprite2.color = Color.blue;
				break;
			case 1:
				sprite1.color = Color.red;
				sprite2.color = Color.red;
				break;
		}
	}

	public override void MakeMove(PlayInstructs instructs)
	{

		Color col = sprite1.color;
		sprite1.color = new Color(col.r, col.g, col.b, 1);
		col = sprite2.color;
		sprite2.color = new Color(col.r, col.g, col.b, 1);
		switch (instructs.action)
		{
			case Actions.REST:
				Rest();
				break;
			case Actions.ATTACK:
				Attack();
				break;
			default:
				Rest();
				break;
		}
		
	}


	protected override void Attack()
	{
		Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
		atkDir.Normalize();
		Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
		GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
		newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, atkCost* 2.5f * NoteMult);
		TakeDamage(atkCost * NoteMult);
		currAction = Actions.ATTACK;
	}
	protected override void Rest()
	{
		currAction = Actions.REST;
		energy += gainRate * NoteMult;
	}


	public override PlayInstructs CurrInstruction()
	{
		Color col = sprite1.color;
		sprite1.color = new Color(col.r, col.g, col.b, .5f);
		col = sprite2.color;
		sprite2.color = new Color(col.r, col.g, col.b, .5f);
		if (enemyList.Count > 0)
		{
			SortEnemyList();
			currTarget = enemyList[0].transform.position;
			if (ArenaGrid.GridDistance(currTarget, transform.position) <= atkDist && energy - (atkCost * NoteMult) > 0)
			{
				currAction = Actions.ATTACK;
			}
			else
			{
				currAction = Actions.REST;
			}
		}
		else
		{
			currAction = Actions.REST;
		}
		return new PlayInstructs(currAction, currNote, transform.position.y, this);
	}
}
