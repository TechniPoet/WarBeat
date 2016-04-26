using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaGrid : UnitySingleton<ArenaGrid>
{
	public GameObject linePrefab;
	public GameObject playHead;
	public Transform leftPos;
	public Transform rightPos;
	public Transform topPos;
	public Transform bottomPos;
	public Transform lines;
	public Transform playLeftPos;
	public Transform playRightPos;


	public static Vector2[,] grid;
	public static float topY, leftX;
	public float lineDist;
	
	public List<GameObject> leftPlayHeads = new List<GameObject>();
	public List<GameObject> rightPlayHeads = new List<GameObject>();
	public List<GameObject> visibleLines = new List<GameObject>();
	public static float lX;
	public static float rX;
	public static float tY;
	public static float bY;
	public static float midX, midY;
	public static float arenaWidth;
	public static float arenaHeight;
	public static float playLX;
	public static float playRX;
	public static float playArenaWidth;

	//private int currLines = 0;
	

	/// <summary>
	/// Returns the subsection the given position is in.
	/// </summary>
	public static int FindSubsection(int sections, float y)
	{
		for (int i = 0; i < sections; i++)
		{
			if (y < ((arenaHeight / sections) * (i + 1)) + bY)
			{
				return i;
			}
		}
		throw new System.Exception(string.Format("Unable to find subsection for given position\n"
			+ "Arena lowY: {0}\nArena highY: {1}\nSearch Y: {2}", bY, tY, y));
	}


	public static Vector2 ClosestGridPoint(Vector2 target)
	{
		Vector2 closestPoint = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 pointLocation = new Vector2(float.MaxValue, float.MaxValue);
		for (int y = 0; y < grid.GetLength(0); y++)
		{
			for (int x = 0; x < grid.GetLength(1); x++)
			{
				if (Vector2.Distance(target, grid[y,x]) < Vector2.Distance(target, closestPoint))
				{
					closestPoint = grid[y, x];
					pointLocation = new Vector2(x,y);
				}
			}
		}
		return pointLocation;
	}

	public static bool ValidGridPos(Vector2 gridPos, int id)
	{
		bool retVal = gridPos.x < 0 || gridPos.x >= grid.GetLength(1) || gridPos.y < 0 || gridPos.y >= grid.GetLength(0);
		if (!retVal)
		{
			Vector2 worldGridPos = grid[(int)gridPos.y, (int)gridPos.x];
			Vector3 origin = new Vector3(worldGridPos.x, worldGridPos.y, 15);
			Ray ray = new Ray(origin, new Vector3(0, 0, -1));
			RaycastHit hit;
			return !Physics.Raycast(ray, out hit, -17f);
			
		}
		return !retVal;
	}


	public static Vector2 GridToWorldPos(Vector2 gridPos)
	{
		return grid[(int)gridPos.y, (int)gridPos.x];
	}

	public static float GridDistance(Vector2 s, Vector2 t)
	{
		float dist = Vector2.Distance(s,t);
		return dist / GridUnitSize();
	}
	
	public static float GridUnitSize()
	{
		return grid[0, 1].x - grid[0, 0].x;
	}

	public void GenerateGrid()
	{
		for (int i = 0; i < visibleLines.Count; i++)
		{
			DestroyImmediate(visibleLines[i]);
		}
		for (int i = 0; i < leftPlayHeads.Count; i++)
		{
			DestroyImmediate(leftPlayHeads[i]);
		}
		for (int i = 0; i < rightPlayHeads.Count; i++)
		{
			DestroyImmediate(rightPlayHeads[i]);
		}
		leftPlayHeads.Clear();
		rightPlayHeads.Clear();
		visibleLines.Clear();
		UpdateValues();

		midY = (arenaHeight / 2) + bY;
		midX = (arenaWidth / 2) + lX;
		int vertNum = Mathf.FloorToInt(arenaHeight / lineDist);
		topY = midY + (((float)vertNum / 2) * lineDist);
		if (vertNum % 2 == 0)
		{
			topY -= (lineDist / 2);
		}
		else
		{
			vertNum++;
		}

		int horizNum = Mathf.FloorToInt(arenaWidth / lineDist);

		leftX = midX - ((float)horizNum / 2) * lineDist;
		if (horizNum % 2 == 0)
		{
			leftX += (lineDist / 2);
		}
		else
		{
			horizNum++;
		}

		grid = new Vector2[vertNum, horizNum];

		for (int i = 0; i < vertNum; i++)
		{
			GameObject g = Instantiate(linePrefab, new Vector3(midX, topY - (lineDist * i), 0), Quaternion.identity) as GameObject;
			g.transform.localScale = new Vector3(arenaWidth * 2, g.transform.localScale.y, g.transform.localScale.z);
			g.transform.SetParent(lines);
			visibleLines.Add(g);
		}

		for (int i = 0; i < horizNum; i++)
		{
			GameObject g = Instantiate(linePrefab, new Vector3(leftX + (lineDist * i), midY, 0), Quaternion.identity) as GameObject;
			g.transform.localScale = new Vector3(g.transform.localScale.x, arenaHeight * 2, g.transform.localScale.z);
			g.transform.SetParent(lines);
			visibleLines.Add(g);
		}


		for (int i = 1; i < 17; i++)
		{
			GameObject g = Instantiate(playHead, new Vector3( playLX +((playArenaWidth/16) * i), tY), Quaternion.identity) as GameObject;
			if (i % 16 == 0)
			{
				g.transform.localScale = new Vector3(1.5f, 1.5f);
				g.GetComponent<NoteHead>().whole.SetActive(true);
			}
			if (i % 8 == 0)
			{
				g.transform.localScale = new Vector3(1.3f, 1.3f);
				g.GetComponent<NoteHead>().half.SetActive(true);
			}
			else if (i % 4 == 0)
			{
				g.transform.localScale = new Vector3(1.1f, 1.1f);
				g.GetComponent<NoteHead>().quarter.SetActive(true);
			}
			else if (i % 2 == 0)
			{
				g.transform.localScale = new Vector3(.9f, .9f);
				g.GetComponent<NoteHead>().eigth.SetActive(true);
			}
			else
			{
				g.transform.localScale = new Vector3(.7f, .7f);
				g.GetComponent<NoteHead>().sixteenth.SetActive(true);
			}
			g.transform.SetParent(lines);
			leftPlayHeads.Add(g);
		}

		for (int i = 1; i < 5; i++)
		{
			GameObject g = Instantiate(playHead, new Vector3(playRX - ((playArenaWidth / 4) * i), bY), Quaternion.identity) as GameObject;
			g.GetComponent<SpriteRenderer>().flipY = false;
			g.transform.SetParent(lines);
			rightPlayHeads.Add(g);
		}

		for (int y = 0; y < vertNum; y++)
		{
			for (int x = 0; x < horizNum; x++)
			{
				ArenaGrid.grid[y, x] = new Vector2(leftX + (lineDist * x), topY - (lineDist * y));
			}
		}
	}

	void UpdateValues()
	{
		lX = leftPos.position.x;
		rX = rightPos.position.x;
		tY = topPos.position.y;
		bY = bottomPos.position.y;
		arenaWidth = rX - lX;
		arenaHeight = tY - bY;
		playLX = playLeftPos.position.x;
		playRX = playRightPos.position.x;
		playArenaWidth = playRX - playLX;
	}
}
