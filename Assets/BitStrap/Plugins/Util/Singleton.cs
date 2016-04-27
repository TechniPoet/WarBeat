using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Simple singleton class that implements the singleton code design pattern.
    /// Use it by inheriting from this class, using T as the class itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        /// <summary>
        /// The class's single instance.
        /// </summary>
        public static T Instance
        {
            get { return GetInstance( true ); }
        }

        /// <summary>
        /// Executes the callback passing the instance if there is one.
        /// </summary>
        /// <param name="callback"></param>
        public static void RequireInstance( System.Action<T> callback )
        {
            T inst = GetInstance( false );
            if( inst != null && callback != null )
            {
                callback( inst );
            }
        }

        /// <summary>
        /// Returns the class's single instance.
        /// </summary>
        /// <param name="warnIfNotFound"></param>
        /// <returns></returns>
        public static T GetInstance( bool warnIfNotFound )
        {
            if( instance == null )
            {
                instance = Object.FindObjectOfType<T>();

                if( instance == null && warnIfNotFound )
                {
                    OnInstanceNotFound();
                }
            }

            return instance;
        }

        protected void ForceSingletonInstance()
        {
            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }

        private static void OnInstanceNotFound()
        {
            Debug.LogWarningFormat( "Didn't find a object of type {0}!", typeof( T ).Name );
        }
    }
}
