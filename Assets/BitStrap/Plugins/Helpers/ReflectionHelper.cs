using System.Reflection;

namespace BitStrap
{
    /// <summary>
    /// Complementary methods to the System.Reflection classes.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns the value of all fields of type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static TField[] GetFieldValuesOfType<TField>( object instance )
        {
            return GetFieldValuesOfType<TField>( instance, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
        }

        /// <summary>
        /// Returns the value of all fields of type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="instance"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static TField[] GetFieldValuesOfType<TField>( object instance, BindingFlags bindingFlags )
        {
            FieldInfo[] fields = instance.GetType().GetFields( bindingFlags );
            int size = fields.Count( f => typeof( TField ).IsAssignableFrom( f.FieldType ) );
            TField[] values = new TField[size];

            int i = 0;
            foreach( var field in fields )
            {
                if( typeof( TField ).IsAssignableFrom( field.FieldType ) )
                {
                    values[i++] = ( TField ) field.GetValue( instance );
                }
            }

            return values;
        }
    }
}
