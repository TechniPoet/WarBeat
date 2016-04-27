using System.Linq;
using System.Reflection;
using UnityEditor;

namespace BitStrap
{
    /// <summary>
    /// Bunch of SerializedPropertyHelper helper methods to ease your Unity custom editor development.
    /// </summary>
    public static class SerializedPropertyHelper
    {
        /// <summary>
        /// Given and a SerializedProperty and its fieldInfo, returns its instance reference.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetValue( FieldInfo fieldInfo, SerializedProperty property )
        {
            object instance = property.serializedObject.targetObject;

            if( property.depth > 0 )
            {
                string[] elements = property.propertyPath.Split( '.' );
                foreach( string element in elements.Take( property.depth ) )
                {
                    instance = GetInstance( instance, element );
                }
            }

            return fieldInfo.GetValue( instance );
        }

        private static object GetInstance( object source, string fieldName )
        {
            if( source == null )
                return null;

            System.Type type = source.GetType();
            FieldInfo field = type.GetField( fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            if( field == null )
                return null;

            return field.GetValue( source );
        }
    }
}
