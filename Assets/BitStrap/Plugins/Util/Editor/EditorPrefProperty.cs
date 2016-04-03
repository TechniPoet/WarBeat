using UnityEditor;

namespace BitStrap
{
    /// <summary>
    /// Makes it easy to work with EditorPrefs treating them as properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EditorPrefProperty<T>
    {
        protected string key;
        private T value;
        private bool initialized = false;

        /// <summary>
        /// Use this property to get/set this editor pref.
        /// </summary>
        public T Value
        {
            get { RetrieveValue(); return value; }
            set { SaveValue( value ); }
        }

        protected EditorPrefProperty( string prefKey )
        {
            key = prefKey;
            value = default( T );
            initialized = false;
        }

        protected void RetrieveValue()
        {
            if( !initialized )
            {
                value = OnRetrieveValue();
                initialized = true;
            }
        }

        protected void SaveValue( T newValue )
        {
            if( !value.Equals( newValue ) )
            {
                value = newValue;
                OnSaveValue( value );
            }
        }

        protected abstract T OnRetrieveValue();

        protected abstract void OnSaveValue( T value );
    }

    /// <summary>
    /// A specialization of EditorPrefProperty for strings.
    /// </summary>
    public class EditorPrefString : EditorPrefProperty<string>
    {
        public EditorPrefString( string key ) : base( key )
        {
        }

        protected override string OnRetrieveValue()
        {
            return EditorPrefs.GetString( key, "" );
        }

        protected override void OnSaveValue( string value )
        {
            EditorPrefs.SetString( key, value );
        }
    }

    /// <summary>
    /// A specialization of EditorPrefProperty class for ints.
    /// </summary>
    public class EditorPrefInt : EditorPrefProperty<int>
    {
        public EditorPrefInt( string key ) : base( key )
        {
        }

        protected override int OnRetrieveValue()
        {
            return EditorPrefs.GetInt( key, 0 );
        }

        protected override void OnSaveValue( int value )
        {
            EditorPrefs.SetInt( key, value );
        }
    }

    /// <summary>
    /// A specialization of EditorPrefProperty class for floats.
    /// </summary>
    public class EditorPrefFloat : EditorPrefProperty<float>
    {
        public EditorPrefFloat( string key ) : base( key )
        {
        }

        protected override float OnRetrieveValue()
        {
            return EditorPrefs.GetFloat( key, 0.0f );
        }

        protected override void OnSaveValue( float value )
        {
            EditorPrefs.SetFloat( key, value );
        }
    }

    /// <summary>
    /// A specialization of EditorPrefProperty class for bool.
    /// </summary>
    public class EditorPrefBool : EditorPrefProperty<bool>
    {
        public EditorPrefBool( string key ) : base( key )
        {
        }

        protected override bool OnRetrieveValue()
        {
            return EditorPrefs.GetBool( key, false );
        }

        protected override void OnSaveValue( bool value )
        {
            EditorPrefs.SetBool( key, value );
        }
    }
}
