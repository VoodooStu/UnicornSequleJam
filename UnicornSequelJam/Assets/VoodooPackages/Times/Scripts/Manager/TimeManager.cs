using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech.Times
{
    public class TimeManager : SingletonMB<TimeManager>
    {
        private const string LastDayKey             = "TimeManager_LastDay";
        private const string DayCountKey            = "TimeManager_DayCount";
        private const string SessionCountKey        = "TimeManager_SessionCount";
        private const string LastLoginTimeString= "TimeManager_LastLoginTime";
        private const string DailySessionCountKey   = "TimeManager_DailySessionCount";

        public string   suffixDays                  = "days";
        public string   suffixHours                 = "h";
        public string   suffixMinutes               = "m";
        public string   suffixSeconds               = "s";

        private List<int>   pausedContext           = new List<int>();
        private List<Timer> timers                  = new List<Timer>();

        private List<Timer> timersToAdd             = new List<Timer>();
        private List<Timer> timersToRemove          = new List<Timer>();

        public int DayCount                         => BinaryPrefs.GetInt(DayCountKey, 1);
        public int DailySessionCount                => BinaryPrefs.GetInt(DailySessionCountKey);
        public int SessionCount                     => BinaryPrefs.GetInt(SessionCountKey);

        public DateTime LastLoginTime
        {
            set
            {
                BinaryPrefs.SetDouble(LastLoginTimeString,value.ToOADate());
            }
            get
            {
                Double test = BinaryPrefs.GetDouble(LastLoginTimeString,-1);
                if (test < 0)
                {
                    return DateTime.Now;
                }
                    return DateTime.FromOADate(test);
               
            }
        }

        private void Awake()
        {
            double lastOADay = BinaryPrefs.GetDouble(LastDayKey);
            DateTime lastDay = lastOADay == 0.0 ? DateTime.MinValue : DateTime.FromOADate(lastOADay);
            bool isNextDay = DateTime.Now.Year != lastDay.Year ||
                             DateTime.Now.Month != lastDay.Month ||
                             DateTime.Now.Day != lastDay.Day;
       
            if (lastDay == DateTime.MinValue || isNextDay)
            {
                IncreaseDayCount();
            }
            else
            {
                BinaryPrefs.SetInt(DailySessionCountKey, BinaryPrefs.GetInt(DailySessionCountKey) + 1);
                BinaryPrefs.SetInt(SessionCountKey, BinaryPrefs.GetInt(SessionCountKey) + 1);
            }
        }

        /// <summary>
        /// Reset prefs to initial values
        /// </summary>
        public static void ResetValues()
        {
            BinaryPrefs.SetDouble(LastDayKey, DateTime.Now.ToOADate());
            BinaryPrefs.SetInt(DayCountKey, 1);
            BinaryPrefs.SetInt(DailySessionCountKey, 1);
            BinaryPrefs.SetInt(SessionCountKey, 1);
        }

        /// <summary>
        /// Force Daycount increase in prefs
        /// </summary>
        public static void IncreaseDayCount()
        {  
            BinaryPrefs.SetDouble(LastDayKey, DateTime.Now.ToOADate());
            BinaryPrefs.SetInt(DayCountKey, BinaryPrefs.GetInt(DayCountKey) + 1);
            BinaryPrefs.SetInt(DailySessionCountKey, 1);
            BinaryPrefs.SetInt(SessionCountKey, BinaryPrefs.GetInt(SessionCountKey) + 1);
        }

        /// <summary>
        /// Add a timer to the updated collection of timer, should never be called outside of the timer class
        /// </summary>
        /// <param name="_timer">timer instance to add</param>
        public void AddTimer(Timer _timer)
        {
            if (timers.IndexOf(_timer) >= 0 && timersToAdd.IndexOf(_timer) >= 0) 
            {
                return;
            }

            timersToAdd.Add(_timer);
        }

        /// <summary>
        /// Remove a timer from the updated collection of timer, should never be called outside of the timer class
        /// </summary>
        /// <param name="_timer">timer instance to remove</param>
        public void RemoveTimer(Timer _timer)
        {
            if (timers.IndexOf(_timer) < 0 && timersToRemove.IndexOf(_timer) < 0)
            {
                return;
            }

            timersToRemove.Add(_timer);
        }
                
        void FixedUpdate()
        {
            for (int i = 0; i < timersToAdd.Count; i++)
            {
                timers.Add(timersToAdd[i]);
            }
            timersToAdd.Clear();

            for (int i = 0; i < timersToRemove.Count; i++)
            {
                timers.Remove(timersToRemove[i]);
            }
            timersToRemove.Clear();

            if (timers.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Update(UnityEngine.Time.fixedDeltaTime);
            }
        }
        
        /// <summary>
        /// Return a TimeSpan of the interval between startDate and endDate.
        /// If negative, it will return a TimeSpan.Zero.
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public TimeSpan GetInterval(DateTime _startDate, DateTime _endDate)
        {
            TimeSpan _timeSpan = _endDate.Subtract(_startDate);

            if (_timeSpan.TotalSeconds < 0)
            {
                _timeSpan = TimeSpan.Zero;
            }

            return _timeSpan;
        }
        
        /// <summary>
        /// Return the interval between startDate and endDate in seconds
        /// If negative, it will return a 0
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public double GetIntervalSeconds(DateTime _startDate, DateTime _endDate)
        {
            return GetInterval(_startDate, _endDate).TotalSeconds;
        }

        /// <summary>
        /// Return true if the startDate is superior or equal to the endDate
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_endTime"></param>
        /// <returns></returns>
        public bool IsDelayExpired(DateTime _startDate, DateTime _endDate)
        {
            int value = DateTime.Compare(_startDate, _endDate); 
  
            // checking 
            if (value >= 0)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Return the interval between startDate and endTime in a string format 
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_endTime"></param>
        /// <returns></returns>
        public string GetCountdownDisplayValue(DateTime _startDate, DateTime _endTime)
        {
            TimeSpan _timeSpan = _endTime.Subtract(_startDate);
            
            if (_timeSpan.TotalDays > 1.0)
            {
                if (_timeSpan.Hours > 0 || _timeSpan.Minutes > 0 || _timeSpan.Seconds>0)
                {
                    return (int)(_timeSpan.TotalDays+1) + suffixDays;
                }

                return (int)_timeSpan.TotalDays + suffixDays;
            }
            if (_timeSpan.TotalHours > 1.0)
            {
                if (_timeSpan.Minutes > 0 || _timeSpan.Seconds>0)
                {
                    return (int)(_timeSpan.TotalHours+1) + suffixHours;
                }

                return (int)_timeSpan.TotalHours + suffixHours;
            }
            if (_timeSpan.TotalMinutes > 1.0)
            {
                if (_timeSpan.Seconds > 0)
                {
                    return (int)(_timeSpan.TotalMinutes+1) + suffixMinutes;
                }

                return (int)_timeSpan.TotalMinutes + suffixMinutes;

            }
            if (_timeSpan.TotalSeconds > 1.0)
            {
                return (int)_timeSpan.TotalSeconds + suffixSeconds;
            }

            return String.Empty;

        }
        private void OnDestroy()
        {
            LastLoginTime = DateTime.Now;
            BinaryPrefs.ForceSave();
        }
    }
}
