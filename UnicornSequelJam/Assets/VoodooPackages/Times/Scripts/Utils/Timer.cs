using UnityEngine;

namespace VoodooPackages.Tech.Times
{
    public class Timer : System.IDisposable
    {
        private float duration  = 0f;
        public  float tickRate  = 0.1f;
        private bool  loop      = false;
        private int   context   = 0;
                      
        private float startTime = 0f;
        private float past      = 0f;
        private int   tickCount = 0;
        private int   loopCount = 0;
        private bool  isRunning = false;
        
        public event TimeEvent Started;
        public event TimeEvent Ticked;
        public event TimeEvent Paused;
        public event TimeEvent Resumed;
        public event TimeEvent Looped;
        public event TimeEvent Stopped;


        public float StartTime      => startTime;

        public float Past           => past;
        public float PastNormalized => Mathf.Max(Mathf.Min(past / duration, 1.0f), 0.0f);

        public float Left           => duration - past;
        public float LeftNormalized => 1 - PastNormalized;
        
        public float Duration       => duration;

        public bool  IsOver         => past >= duration;

        public bool IsRunning
        {
            get => isRunning;

            set
            {
                if (value == IsRunning) { return; }
                
                isRunning = value;
                if (value == false) { Paused?.Invoke(this); } else { Resumed?.Invoke(this); }
            }
        }

        public Timer() : this(0f) {}

        public Timer(float _duration, int _context = 0, float _tickRate = 0f, bool _loop = false)
        {
            duration    = _duration;
            tickRate    = _tickRate;
            loop        = _loop;
            context     = _context;

            isRunning   = false;
        }

        /// <summary>
        /// Attach the timer to it's manager update callback, reset past time and set it to isRunning = true
        /// </summary>
        /// <param name="_duration">if value > 0 change timer duration, only needed in case you wish to chenge duration between to usage</param>
        public void Start(float _duration = 0f)
        {
            if (_duration > 0f)
            {
                duration = _duration;
            }

            startTime   = Time.realtimeSinceStartup;
            past        = 0f;
            isRunning   = true;

            TimeManager.Instance.AddTimer(this);

            Started?.Invoke(this);
        }

        /// <summary>
        /// If timer is running increase its past time of the given delta, then check if Ticked/Looped/Stopped are to be triggered.
        /// </summary>
        /// <param name="_delta">time past between two frame</param>
        public void Update(float _delta)
        {
            if (isRunning == false)
            {
                return;
            }

            past += _delta;

            if (tickRate == 0f || past % tickRate > tickCount) 
            { 
                tickCount++;
                Ticked?.Invoke(this);
            }

            if (IsOver == false)
            {
                return;
            }

            if (loop == false)
            {
                Stop();
            }
            else if ((int)(past / duration) > loopCount)
            {
                loopCount++;
                Looped?.Invoke(this);
            }
        }

        /// <summary>
        /// Reset all members value to default, dettach timer from its manager update, then trigger Stopped callback
        /// </summary>
        public void Stop()
        {
            Reset();
            if (TimeManager.Instance != null)
                TimeManager.Instance.RemoveTimer(this);
            
            Stopped?.Invoke(this);
        }

        /// <summary>
        /// Reset all members value ti default
        /// </summary>
        public void Reset()
        {
            startTime   = 0f;
            past        = 0f;
            tickCount   = 0;
            loopCount   = 0;
            isRunning   = false;
        }

        /// <summary>
        /// Nullify all events refs
        /// </summary>
        public void Unbind() 
        {
            Started = null;
            Ticked = null;
            Paused = null;
            Resumed = null;
            Looped = null;
            Stopped = null;
        }

        /// <summary>
        /// Free object
        /// </summary>
        public void Dispose()
        {
            Reset();
            Unbind();

            System.GC.SuppressFinalize(this);
        }
    }

    public delegate void TimeEvent(Timer _timer);
}