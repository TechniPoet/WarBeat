using BitStrap;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Open this window by navigating in Unity Editor to "Window/BitStrap/Extensions/ProjectBrowserHelper".
/// </summary>
public class ProjectBrowserHelperExample : EditorWindow
{
    private string searchString = "Search";

    [MenuItem( "Window/BitStrap/Helpers/ProjectBrowserHelper" )]
    public static void CreateWindow()
    {
        GetWindow<ProjectBrowserHelperExample>().Show();
    }

    private void OnGUI()
    {
        searchString = EditorGUILayout.TextField( "Search String", searchString );

        if( GUILayout.Button( "Set Search in Project Browser Window" ) )
        {
            ProjectBrowserHelper.SetSearch( searchString );
        }
    }
}
