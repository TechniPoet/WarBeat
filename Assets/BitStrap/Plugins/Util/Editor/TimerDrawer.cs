using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( Timer ) )]
    public class TimerDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.PropertyField( position, property.FindPropertyRelative( "length" ), label );
        }
    }
}
