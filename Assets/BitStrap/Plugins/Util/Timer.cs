using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Timer utility class. Allows you to receive a callback after a certain
    /// amount of time has elapsed.
    /// </summary>
    [System.Serializable]
    public class Timer
    {
        [SerializeField]
        private float length = 1.0f;

        private float counter = -1.0f;

        /// <summary>
        /// The timer's length in seconds.
        /// </summary>
        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// Callback that gets called when "length" seconds has elapsed.
        /// </summary>
        public System.Action OnTimer { get; set; }

        /// <summary>
        /// The countdown time in seconds.
        /// </summary>
        public float RemainingTime
        {
            get { return Mathf.Clamp( Length - counter, 0, Length ); }
        }

        /// <summary>
        /// Is the timer countdown running?
        /// </summary>
        public bool IsRunning
        {
            get { return counter >= 0; }
        }

        public Timer( float length )
        {
            Length = length;
        }

        /// <summary>
        /// You need to manually call this at your script Update() method
        /// for the timer to work properly.
        /// </summary>
        public void OnUpdate()
        {
            if( counter < 0.0f )
            {
                // Already triggered callback.
            }
            else if( counter < Length )
            {
                counter += Time.deltaTime;
            }
            else
            {
                if( OnTimer != null )
                    OnTimer();

                counter = -1.0f;
            }
        }

        /// <summary>
        /// Stop the timer and its counter.
        /// </summary>
        public void Stop()
        {
            counter = -1;
        }

        /// <summary>
        /// Start the timer and play its counter.
        /// </summary>
        public void Start()
        {
            counter = 0.0f;
        }
    }
}
