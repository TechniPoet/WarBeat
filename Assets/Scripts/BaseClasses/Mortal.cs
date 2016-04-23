using UnityEngine;
using System.Collections;
using PuppetType = ConstFile.PuppetType;

public class Mortal : MonoBehaviour {
    public float energy;
    public float gainRate;
    public int team;

    public RectTransform energyBar;
    protected float minX;
    protected float maxX;
    protected float cachedY;
    public float maxEnergy;
	protected bool dead;

	public float MaxEnergy
	{
		get { return maxEnergy; }
	}

    // Use this for initialization
    void Awake () {
		dead = false;
		cachedY = energyBar.anchoredPosition.y;
        maxX = energyBar.anchoredPosition.x;
        minX = energyBar.anchoredPosition.x - energyBar.rect.width;
    }
	

    protected virtual void Update()
    {
        energy = Mathf.Clamp(energy, -10, maxEnergy);
        energyBar.anchoredPosition = new Vector3(minX + ((energy / maxEnergy) * energyBar.rect.width), cachedY);
		dead = energy <= 0;
    }

    public void TakeDamage(float amt)
    {
        energy -= amt;
    }
}
