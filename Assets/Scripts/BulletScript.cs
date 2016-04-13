using UnityEngine;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour {

    float speed;
    Vector2 dir;
    int teamNum;
	//float energyPerUnit = 5;
    public float energy = 34;
	public bool dead = false;
	static int cnt = 0;
	public int id;
	public List<int> idSeen;

    public void Setup(float newSpeed, Vector2 newDir, int newTeam, float newEnergy)
    {
		cnt++;
		id = cnt;
		idSeen = new List<int>();

		energy = newEnergy * 1.2f;
        speed = newSpeed;
        dir = newDir;
        teamNum = newTeam;
        this.gameObject.SetActive(true);
		switch (teamNum)
		{
			case 0:
				GetComponent<SpriteRenderer>().color = Color.blue;
				break;
			case 1:
				GetComponent<SpriteRenderer>().color = Color.red;
				break;
		}
    }

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, transform.position.y) + (dir * speed * Time.deltaTime);
        //energy -= energyPerUnit * (Mathf.Abs((dir * speed * Time.deltaTime).magnitude) / ArenaGrid.GridUnitSize() );
        if (energy <= 0)
        {
			dead = true;
        }
		else
		{
			transform.localScale = new Vector3(energy/30, energy/30, 1);
		}
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.GetComponent<Mortal>()==null) {
			return;
		}

		switch (col.gameObject.tag)
        {
            case GameManager.UnitTag:
                if (col.gameObject.GetComponent<Mortal>().team != teamNum)
                {
                    col.gameObject.GetComponent<Mortal>().TakeDamage(energy);
                    this.Destroy();
                }
				else
				{
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
				}
                break;
            case GameManager.StatueTag:
                if (col.gameObject.GetComponent<Mortal>().team != teamNum)
                {
                    col.gameObject.GetComponent<Mortal>().TakeDamage(energy);
                    this.Destroy();
                }
                break;
            case "Bad":
				BulletScript bul = col.gameObject.GetComponent<BulletScript>();
				if (!idSeen.Contains(bul.id))
				{
					if (bul.teamNum != teamNum)
					{
						float oldDmg = bul.GetDamage();
						bul.TakeDamage(energy);
						this.TakeDamage(oldDmg);
					}
					bul.idSeen.Add(id);
				}
				
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
				break;
            default:
				this.Destroy();
                break;
        }
    }

	void LateUpdate()
	{
		if (dead)
		{
			this.Destroy();
		}
	}

    public float GetDamage()
    {
        return energy;
    }

	public void TakeDamage(float dmg)
	{
		energy -= Mathf.Abs(dmg);
	}

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
