using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( ParticleController ) )]
    public class ParticleControllerDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            SerializedProperty root = property.FindPropertyRelative( "rootParticleSystem" );

            EditorGUI.PropertyField( position, root, label );

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
