using UnityEngine;
using System.Collections;

public class BasicAIController : MonoBehaviour {

	public SpawnMaster sp;
	public StatueScript homeBase;
	[Range(0,1)]
	public float lowEnergy;
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
			if (homeBase.energy < (lowEnergy * homeBase.MaxEnergy))
			{
				yield return new WaitForSeconds(waitForRegenTime);
			}
			else
			{
				sp.SpawnTrebleR();
				Debug.Log("Enemy spawn");
				float waitTime = randIntervals ? Random.Range(minSpawnTime, spawnCoolTime) : spawnCoolTime;
				yield return new WaitForSeconds(waitTime);
			}
		}
	}
}
