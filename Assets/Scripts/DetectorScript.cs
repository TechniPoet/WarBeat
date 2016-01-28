using UnityEngine;
using System.Collections.Generic;

public class DetectorScript : MonoBehaviour {
    [System.NonSerialized]public List<GameObject> intruders;
    public int team;
	// Use this for initialization
	void Awake () {
        intruders = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (intruders.Count > 0)
        {
            for (int i=0; i < intruders.Count; i++)
            {
                if (intruders[i] == null || !intruders[i].activeSelf)
                {
                    intruders.RemoveAt(i);
                }
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case GameManager.UnitTag:
                if (col.gameObject.GetComponent<Mortal>().team != team)
                {
                    intruders.Sort((x, y) => DistCompare(x, y));
                    intruders.Add(col.gameObject);
                }
                break;
            case GameManager.StatueTag:
                if (col.gameObject.GetComponent<Mortal>().team != team)
                {
                    intruders.Sort((x, y) => DistCompare(x, y));
                    intruders.Add(col.gameObject);
                }
                break;
        }
        
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == GameManager.UnitTag)
        {
            intruders.Sort((x, y) => DistCompare(x, y));
            intruders.Remove(col.gameObject);
        }
    }

    int DistCompare(GameObject x, GameObject y)
    {
        float xDist = Vector2.Distance(transform.position, x.transform.position);
        float yDist = Vector2.Distance(transform.position, y.transform.position);
        return xDist.CompareTo(yDist);
    }
}
