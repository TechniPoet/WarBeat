using UnityEngine;
using System.Collections;

public class StatueScript : Mortal {

    public RectTransform healthBar;
    private float minX;
    private float maxX;
    private float cachedY;
    private float cachedHealth;

    // Use this for initialization
    void Awake()
    {
        health = 800;
        cachedHealth = health;
        cachedY = healthBar.anchoredPosition.y;
        maxX = healthBar.anchoredPosition.x;
        minX = healthBar.anchoredPosition.x - healthBar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.anchoredPosition = new Vector3(minX + ((health / cachedHealth) * healthBar.rect.width), cachedY);
    }

}
