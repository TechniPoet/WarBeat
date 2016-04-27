using BitStrap;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Open this window by navigating in Unity Editor to "Window/BitStrap/Extensions/RectExtensions".
/// </summary>
public class RectExtensionsExample : EditorWindow
{
    private float width = 0.4f;
    private float height = 120.0f;

    [MenuItem( "Window/BitStrap/Extensions/RectExtensions" )]
    public static void CreateWindow()
    {
        GetWindow<RectExtensionsExample>().Show();
    }

    private void OnGUI()
    {
        width = EditorGUILayout.Slider( "Width %", width, -1.0f, 1.0f );

        Rect rect = EditorGUILayout.GetControlRect( true, height );

        GUI.backgroundColor = Color.gray;
        GUI.Box( rect, GUIContent.none );

        Rect lineRect = rect.Row( 1 );

        Rect leftRect = lineRect.Left( lineRect.width * width );
        Rect floatedLeftRect = lineRect.Left( lineRect.width * ( -width ) );

        GUI.backgroundColor = Color.green;
        GUI.Box( leftRect, GUIContent.none );
        EditorGUI.LabelField( leftRect, "Left Rect" );
        EditorGUI.LabelField( floatedLeftRect, "Floated Left Rect" );

        lineRect = rect.Row( 2 );

        Rect rightRect = lineRect.Right( lineRect.width * width );
        Rect floatedRightRect = lineRect.Right( lineRect.width * ( -width ) );

        GUI.backgroundColor = Color.red;
        GUI.Box( rightRect, GUIContent.none );
        EditorGUI.LabelField( rightRect, "Right Rect" );
        EditorGUI.LabelField( floatedRightRect, "Floated Right Rect" );
    }
}
