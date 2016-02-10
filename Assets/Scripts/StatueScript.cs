using UnityEngine;
using System.Collections;

public class StatueScript : Mortal {

    
    public float healthPerSec;
    // Use this for initialization
    void Awake()
    {
        health = 400;
        cachedHealth = 800;
        
    }

    // Update is called once per frame
    void Update()
    {
        health += healthPerSec * Time.deltaTime;
        MortalUpdate();
    }

}
