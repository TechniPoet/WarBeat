using UnityEngine;
using System.Collections;

public class ArenaGrid : MonoBehaviour
{
	private LineRenderer line;

	public Material mat;

	public Transform leftPos;
	public Transform rightPos;
	public Transform topPos;
	public Transform bottomPos;
	public Transform lines;

	private int currLines = 0;
	float lX;
	float rX;
	float tY;
	float bY;

	// Use this for initialization
	void Start ()
	{
		lX = leftPos.position.x;
		rX = rightPos.position.x;
		tY = topPos.position.y;
		bY = bottomPos.position.y;

		int xLines = Mathf.FloorToInt(rX - lX);
		int yLines = Mathf.FloorToInt(tY - bY);
		
		for (int i = 0; i < xLines + 1; i++)
		{
			createLine();
			line.SetPosition(0, new Vector3(lX + i, tY, 0));
			line.SetPosition(1, new Vector3(lX + i, bY, 0));
			currLines++;
		}

		for (int i = 0; i < yLines; i++)
		{
			createLine();
			line.SetPosition(0, new Vector3(lX, tY - i - 1, 0));
			line.SetPosition(1, new Vector3(rX, tY - i - 1, 0));
			currLines++;
		}
	}

	private void createLine()
	{
		//create a new empty gameobject and line renderer component
		line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
		line.gameObject.transform.SetParent(lines);
		//assign the material to the line
		line.material = mat;
		//set the number of points to the line
		line.SetVertexCount(2);
		//set the width
		line.SetWidth(0.01f, 0.01f);
		//render line to the world origin and not to the object's position
		line.useWorldSpace = true;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
