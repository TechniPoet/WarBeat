using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    float speed;
    Vector2 dir;
    int teamNum;
	float energyPerUnit = 5;
    public float energy = 34;
	public bool dead = false;

    public void Setup(float newSpeed, Vector2 newDir, int newTeam, float newEnergy)
    {
        energy = newEnergy * 1.5f;
        speed = newSpeed;
        dir = newDir;
        teamNum = newTeam;
        this.gameObject.SetActive(true);
    }

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, transform.position.y) + (dir * speed * Time.deltaTime);
        energy -= energyPerUnit * Mathf.Abs((dir * speed * Time.deltaTime).magnitude);
        if (energy <= 0)
        {
			dead = true;
        }
		else
		{
			transform.localScale = new Vector3(energy/20, energy/20, 1);
		}
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
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

				if (bul.teamNum != teamNum)
                {
					float oldDmg = bul.GetDamage();
					bul.TakeDamage(energy);
					this.TakeDamage(oldDmg);
					
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
