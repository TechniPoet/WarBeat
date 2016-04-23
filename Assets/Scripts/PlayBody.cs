using UnityEngine;
using GAudio;
using System.Collections;

public class PlayBody : MonoBehaviour {

	public delegate void voidDel(Puppet u);
	public event voidDel UnitHit;
	public int team;
	

	void Start()
	{
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<Puppet>() != null)
		{
			if (col.GetComponent<Puppet>().team == team)
			{
				DebugPanel.Log("found1", col.GetComponent<Puppet>().id.ToString(), col.GetComponent<Puppet>().id);
				if (UnitHit != null)
				{
					DebugPanel.Log("found", col.GetComponent<Puppet>().id.ToString(), col.GetComponent<Puppet>().id);
					UnitHit(col.GetComponent<Puppet>());
				}
			}
		}

	}
	
}

