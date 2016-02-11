using UnityEngine;
using System.Collections;
using GAudio;
using System;

public class UnitScript : Mortal, IGATPulseClient
{
    
    public Transform mainTarget;
    public GameObject bullet;
    public DetectorScript vision;

    public float moveSpeed = .1f;
    public float attackDist = 2;
    public float attackSpeed = .2f;
    public float attackCooldown = 1;
    public float bulletLife = 1.5f;
    public PlayNote currNote;

    public enum Actions
    {
        NONE,
        MOVE,
        ATTACK,
    }

    public Actions[] actionPattern;

    Vector2 goalPos;
    Vector2 currTarget;
    bool attack = false;
    bool attackCool = true;

    PulseModule pulse;
    int curr_beat = 0;

    int healCount = 0;
    float lastPulseTime = 0;
    float deltaPulseTime = 0;

    public void Setup(int newTeam, GameObject target, float newMaxE, float newStartE, float newGainRate)
    {
        team = newTeam;
        mainTarget = target.transform;
        energy = newStartE;
        maxEnergy = newStartE;
        gainRate = newGainRate;
        lastPulseTime = Time.time;
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
        pulse = MusicManager._Pulse;
        pulse.SubscribeToPulse(this);
        goalPos = new Vector2(mainTarget.position.x, transform.position.y);
        vision.team = team;
        MusicManager.units.Add(this);
    }
	
	// Update is called once per frame
	void Update () {
        if (energy <= 0)
        {
            MusicManager.units.Remove(this);
            pulse.UnsubscribeToPulse(this);
            Destroy(this.gameObject);
        }
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
        }
        else
        {
            currTarget = mainTarget.position;
        }
    }

    void IGATPulseClient.OnPulse(IGATPulseInfo pulseInfo)
    {
        // Upkeep Tasks.
        PulseTimer(); // Manage pulse time.
        curr_beat = pulseInfo.StepIndex;

        // Action handle.
        switch (actionPattern[curr_beat])
        {
            case Actions.MOVE:
                MoveTowards(currTarget);
                break;
            case Actions.ATTACK:
                Attack();
                break;
            case Actions.NONE:
                // Add energy
                break;
        }
    }

    void PulseTimer()
    {
        deltaPulseTime = Time.time - lastPulseTime;
        lastPulseTime = Time.time;
    }
    /*
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
    */
    void Attack()
    {
        //currNote.currNote = GameAmp.note.C;
        
        //GetComponent<AudioSource>().PlayOneShot(shootSound);
        Vector2 atkDir = currTarget - new Vector2(transform.position.x, transform.position.y);
        atkDir.Normalize();
        atkDir *= .5f;
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + atkDir;
        GameObject newBul = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        newBul.GetComponent<BulletScript>().Setup(attackSpeed, atkDir, team, bulletLife);
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
        //currNote.currNote = GameAmp.note.E;
        Vector2 moveDir = tar - new Vector2(transform.position.x, transform.position.y);
        moveDir.Normalize();
        
        transform.position = new Vector2(transform.position.x, transform.position.y) + (moveDir * moveSpeed /* Time.deltaTime*/);
    }

    

    void IGATPulseClient.PulseStepsDidChange(bool[] newSteps)
    {
        //Nothing
    }
}
