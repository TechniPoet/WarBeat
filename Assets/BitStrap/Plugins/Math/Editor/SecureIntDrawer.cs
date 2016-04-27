using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BitStrap
{
	[CustomPropertyDrawer(typeof ( SecureInt ))]
	public class SecureIntDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			SerializedProperty valueProp = prop.FindPropertyRelative("value");

			if ( EditorApplication.isPlaying )
			{
				SecureInt self = fieldInfo.GetValue(prop.serializedObject.targetObject) as SecureInt;
				EditorGUI.BeginChangeCheck();
				int value = EditorGUI.IntField(position, label, self.Value);
				if ( EditorGUI.EndChangeCheck() )
				{
					self.Value = value;
					valueProp.intValue = self.EncryptedValue;
				}
			}
			else
			{
				EditorGUI.PropertyField(position, valueProp, label);
			}
		}
	}
}