using UnityEngine;
using System.Collections;

public static class CalcUtil{

	public static int DistCompare(GameObject me, GameObject x, GameObject y)
    {
        float xDist = Vector2.Distance(me.transform.position, x.transform.position);
        float yDist = Vector2.Distance(me.transform.position, y.transform.position);
        return xDist.CompareTo(yDist);
    }
}
