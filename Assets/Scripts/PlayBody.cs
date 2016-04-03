using UnityEngine;
using GAudio;
using System.Collections;

public class PlayBody : MonoBehaviour {

	public delegate void voidDel(UnitScript u);
	public event voidDel UnitHit;
	int team;
	

	void Start()
	{
		
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log("hit");
		if (col.GetComponent<UnitScript>() != null)
		{
			if (col.GetComponent<UnitScript>().team == team)
			{
				if (UnitHit != null)
				{
					UnitHit(col.GetComponent<UnitScript>());
				}
			}
		}
		
	}
}
