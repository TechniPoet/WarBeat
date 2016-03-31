using UnityEngine;
using System.Collections;

public class MathUtil
{
	public static ConstFile.Direction MoveDir(Vector3 targetDir)
	{
		if (targetDir.x == 1)
		{
			return ConstFile.Direction.RIGHT;
		}
		else if (targetDir.x == -1)
		{
			return ConstFile.Direction.LEFT;
		}
		else if (targetDir.y == 1)
		{
			return ConstFile.Direction.UP;
		}
		else if (targetDir.y == -1)
		{
			return ConstFile.Direction.DOWN;
		}
		else
		{
			if (Mathf.Abs(targetDir.y) > Mathf.Abs(targetDir.x))
			{
				bool up = targetDir.y > 0;
				return up ? ConstFile.Direction.UP : ConstFile.Direction.DOWN;
			}
			else
			{
				bool right = targetDir.x > 0;
				return right ? ConstFile.Direction.RIGHT : ConstFile.Direction.LEFT;
			}
		}
	}
}
