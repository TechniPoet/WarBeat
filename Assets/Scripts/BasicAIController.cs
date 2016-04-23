using UnityEngine;
using System.Collections;

public class BasicAIController : MonoBehaviour {

	public SpawnMaster sp;
	public StatueScript homeBase;
	public BaseScript bs;
	[Range(0,1)]
	public float lowEnergy;
	[Range(0, 1)]
	public float highEnergy;
	public bool randEnergyThresh;

	public float waitForRegenTime;
	public float minSpawnTime;
	public float spawnCoolTime;
	public bool randIntervals;

	// Use this for initialization
	void Awake () {
		StartCoroutine(AIStuff());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator AIStuff()
	{
		yield return new WaitForSeconds(1);
		while (true)
		{
			float energyPercent;
			
			energyPercent = randEnergyThresh ? Random.Range(lowEnergy, highEnergy) : lowEnergy;
			
			
			if (bs.energy < (energyPercent * bs.MaxEnergy))
			{
				yield return new WaitForSeconds(waitForRegenTime);
			}
			else
			{
				if (Random.Range(0f,1f) < .7f)
				{
					sp.SpawnTreble(1);
				}
				else
				{
					sp.SpawnBass(1);
				}
				float waitTime = randIntervals ? Random.Range(minSpawnTime, spawnCoolTime) : spawnCoolTime;
				yield return new WaitForSeconds(waitTime);
			}
			
		}
	}
}
