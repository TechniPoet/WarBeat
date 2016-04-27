using UnityEngine;
using System.Collections.Generic;

public class DetectorScript : MonoBehaviour {
    [System.NonSerialized]public List<GameObject> enemies;
    public int team;
	// Use this for initialization
	void Awake () {
        enemies = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (enemies.Count > 0)
        {
            for (int i=0; i < enemies.Count; i++)
            {
                if (enemies[i] == null || !enemies[i].activeSelf)
                {
                    enemies.RemoveAt(i);
                }
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        /*
        switch (col.tag)
        {
            
            case GameManager.UnitTag:
                if (col.gameObject.GetComponent<Mortal>().team != team)
                {
                    enemies.Sort((x, y) => DistCompare(x, y));
                    enemies.Add(col.gameObject);
                }
                break;
            case GameManager.StatueTag:
                if (col.gameObject.GetComponent<Mortal>().team != team)
                {
                    enemies.Sort((x, y) => DistCompare(this.gameObject, x, y));
                    enemies.Add(col.gameObject);
                }
                break;
        }
        */
        
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == GameManager.UnitTag)
        {
            enemies.Sort((x, y) => DistCompare(x, y));
            enemies.Remove(col.gameObject);
        }
    }

    int DistCompare(GameObject x, GameObject y)
    {
        float xDist = Vector2.Distance(transform.position, x.transform.position);
        float yDist = Vector2.Distance(transform.position, y.transform.position);
        return xDist.CompareTo(yDist);
    }
	
}
