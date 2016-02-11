﻿using UnityEngine;
using System.Collections;

public class Mortal : MonoBehaviour {
    public float energy;
    public float gainRate;
    public int team;

    public RectTransform energyBar;
    protected float minX;
    protected float maxX;
    protected float cachedY;
    protected float maxEnergy;

    // Use this for initialization
    void Awake () {
        cachedY = energyBar.anchoredPosition.y;
        maxX = energyBar.anchoredPosition.x;
        minX = energyBar.anchoredPosition.x - energyBar.rect.width;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    protected void MortalUpdate()
    {
        energy = Mathf.Clamp(energy, -10, maxEnergy);
        energyBar.anchoredPosition = new Vector3(minX + ((energy / maxEnergy) * energyBar.rect.width), cachedY);
    }

    public void TakeDamage(float amt)
    {
        energy -= amt;
    }
}
