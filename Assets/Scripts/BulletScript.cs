using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    float speed;
    Vector2 dir;
    int teamNum;
    public float damage = 34;
    float life;
	public bool dead = false;

    public void Setup(float newSpeed, Vector2 newDir, int newTeam, float newLife)
    {
        life = newLife;
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
        life -= Time.deltaTime;
        if (life <= 0 || damage <= 0)
        {
			dead = true;
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case GameManager.UnitTag:
                if (col.gameObject.GetComponent<Mortal>().team != teamNum)
                {
                    col.gameObject.GetComponent<Mortal>().TakeDamage(damage);
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
                    col.gameObject.GetComponent<Mortal>().TakeDamage(damage);
                    this.Destroy();
                }
                break;
            case "Bad":
				BulletScript bul = col.gameObject.GetComponent<BulletScript>();

				if (bul.teamNum != teamNum)
                {
					float oldDmg = bul.GetDamage();
					bul.TakeDamage(damage);
					TakeDamage(oldDmg);
                }
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
        return damage;
    }

	public void TakeDamage(float dmg)
	{
		damage -= dmg;
	}

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
