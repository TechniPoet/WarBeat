using UnityEngine;
using System.Collections;

public class Mortal : MonoBehaviour {
    public float health = 100;
    public int team;

    public RectTransform healthBar;
    protected float minX;
    protected float maxX;
    protected float cachedY;
    protected float cachedHealth;

    // Use this for initialization
    void Awake () {
        cachedY = healthBar.anchoredPosition.y;
        maxX = healthBar.anchoredPosition.x;
        minX = healthBar.anchoredPosition.x - healthBar.rect.width;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void MortalUpdate()
    {
        healthBar.anchoredPosition = new Vector3(minX + ((health / cachedHealth) * healthBar.rect.width), cachedY);
    }

    public void TakeDamage(float amt)
    {
        health -= amt;
    }
}
