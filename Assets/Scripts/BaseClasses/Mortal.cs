using UnityEngine;
using System.Collections;

public class Mortal : MonoBehaviour {
    protected float health = 100;
    public int team;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(float amt)
    {
        health -= amt;
    }
}
