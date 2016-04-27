using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ArenaGrid))]
public class ArenaGridEditor : Editor
{
	private ArenaGrid targ;
	
	

	public override void OnInspectorGUI()
	{
		targ = (ArenaGrid)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate Grid"))
		{
			targ.GenerateGrid();
		}
	}



	
}
