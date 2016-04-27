using BitStrap;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Open this window by navigating in Unity Editor to "Window/BitStrap/Extensions/EditorHelper".
/// </summary>
public class EditorHelperExample : EditorWindow
{
    private string searchText = "";

    [MenuItem( "Window/BitStrap/Helpers/EditorHelper" )]
    public static void CreateWindow()
    {
        GetWindow<EditorHelperExample>().Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField( "Selection Style", EditorHelper.Styles.Selection );
        EditorGUILayout.LabelField( "PreDrop Style", EditorHelper.Styles.PreDrop );

        EditorHelper.BeginChangeLabelWidth( 256.0f );
        EditorGUILayout.IntField( "This is a 256 width label", 0 );
        EditorHelper.EndChangeLabelWidth();

        EditorHelper.BeginBoxHeader();
        EditorGUILayout.LabelField( "Awesome Box" );
        EditorHelper.EndBoxHeaderBeginContent();

        EditorGUILayout.LabelField( "Box contents..." );

        Rect position = EditorHelper.Rect( 4.0f );
        EditorGUI.DrawRect( position, Color.gray );

        GUI.tooltip = "This is a tooltip";
        EditorGUILayout.LabelField( EditorHelper.Label( "This label has a tooltip" ) );

        EditorHelper.EndBox();

        searchText = EditorHelper.SearchField( searchText );
    }
}
