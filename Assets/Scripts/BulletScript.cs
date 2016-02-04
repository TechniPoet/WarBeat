using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    float speed;
    Vector2 dir;
    int teamNum;
    float damage = 34;
    float life;

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
        if (life <= 0)
        {
            this.Destroy();
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
                break;
            case GameManager.StatueTag:
                if (col.gameObject.GetComponent<Mortal>().team != teamNum)
                {
                    col.gameObject.GetComponent<Mortal>().TakeDamage(damage);
                    this.Destroy();
                }
                break;
            case "Bad":
                if (col.gameObject.GetComponent<BulletScript>().teamNum != teamNum)
                {
                    this.Destroy();
                }
                break;
            default:
                this.Destroy();
                break;
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
