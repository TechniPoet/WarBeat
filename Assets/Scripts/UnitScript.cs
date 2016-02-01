using UnityEngine;
using System.Collections;

public class UnitScript : Mortal
{
    
    public Transform mainTarget;
    public GameObject bullet;
    public DetectorScript vision;
    public float moveSpeed = .1f;
    public float attackDist = 2;
    public float attackSpeed = .2f;
    public float attackCooldown = 1;

    public enum unitState
    {
        nothing,
        move,
        shoot,
    }

    public PlayNote currNote;
    public AudioClip moveSound;
    public AudioClip shootSound;

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
        BeatMaster.sixteenthEvent += TickUpdate;
        GameAmp.units.Add(this);
        currNote = new PlayNote();
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            GameAmp.units.Remove(this);
            BeatMaster.sixteenthEvent -= TickUpdate;
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }

    }

    void TickUpdate(int cnt)
    {
        Debug.Log("tick");
        if (cnt % (int)BeatMaster.Timing.Quarter == 0)
        {
            attack = vision.intruders.Count > 0;

            if (attack)
            {
                for (int i = 0; i < vision.intruders.Count; i++)
                {
                    if (vision.intruders[i] != null)
                    {
                        currTarget = vision.intruders[i].transform.position;
                        break;
                    }
                        
                }
                
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
        

    }

    void Attack()
    {
        currNote.currNote = GameAmp.note.C;
        
        //GetComponent<AudioSource>().PlayOneShot(shootSound);
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        atkDir *= .5f;
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team);
        //StartCoroutine(bulletCooldown());
    }

    IEnumerator bulletCooldown()
    {
        attackCool = false;
        yield return new WaitForSeconds(attackCooldown);
        attackCool = true;
    }

    void MoveTowards(Vector2 tar)
    {
        //GetComponent<AudioSource>().PlayOneShot(moveSound);
        currNote.currNote = GameAmp.note.E;
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
        
        transform.position = new Vector2(transform.position.x, transform.position.y) + (moveDir * moveSpeed /* Time.deltaTime*/);
    }
}
