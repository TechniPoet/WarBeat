using UnityEngine;
using System.Collections;

public class UnitScript : Mortal {
    
    public Transform mainTarget;
    public GameObject bullet;
    public DetectorScript vision;
    public float moveSpeed = .1f;
    public float attackDist = 2;
    public float attackSpeed = .2f;
    public float attackCooldown = 1;

    Vector2 goalPos;
    Vector2 currTarget;
    bool attack = false;
    bool attackCool = true;

    public void Setup(int newTeam, GameObject target)
    {
        team = newTeam;
        mainTarget = target.transform;

        switch (team)
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
        goalPos = new Vector2(mainTarget.position.x, transform.position.y);
        vision.team = team;
	}
	
	// Update is called once per frame
	void Update () {

        if(health <= 0)
        {
            this.gameObject.SetActive(false);
        }
        attack = vision.intruders.Count > 0;

        if (attack)
        {
            currTarget = vision.intruders[0].transform.position;
            if (Vector2.Distance(transform.position, currTarget) > attackDist)
            {
                MoveTowards(currTarget);
            }
            else
            {
                if (attackCool)
                {
                    Attack();
                } 
            }
        }
        else
        {
            goalPos = new Vector2(mainTarget.position.x, transform.position.y);
            MoveTowards(goalPos);
        }
	}

    void Attack()
    {
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team);
        StartCoroutine(bulletCooldown());
    }

    IEnumerator bulletCooldown()
    {
        attackCool = false;
        yield return new WaitForSeconds(attackCooldown);
        attackCool = true;
    }

    void MoveTowards(Vector2 tar)
    {
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
        transform.position = new Vector2(transform.position.x, transform.position.y) + (moveDir * moveSpeed * Time.deltaTime);
    }
}
