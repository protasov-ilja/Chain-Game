using System;
using UnityEngine;

namespace ProjectName.Core
{
    public class Timer : MonoBehaviour
    {
        private DateTime _startTime;
        private DateTime _pausedTime;
        private DateTime _endTime;

        private bool _timerStarted;
        private bool _timerPaused;

        private bool _isApplicationPaused = false;

        private event Action _onTimerRestart;
        private event Action _onTimerResume;
        private event Action<TimerVO> _onTimerUpdate;
        private event Action<TimerVO> _onTimerPause;
        private event Action _onTimerStop;

        private event Action _onTimerEnd;

        public Timer StartTimer(float seconds, Action onStartCallback = null)
        {
            _endTime = DateTime.Now.AddSeconds(seconds);
            _startTime = DateTime.Now;
            _timerPaused = false;
            _timerStarted = true;

            onStartCallback?.Invoke();

            return this;
        }

        public void RestartTimer()
        {
            _onTimerRestart?.Invoke();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _isApplicationPaused = pauseStatus;
                if (!_timerPaused && _timerStarted)
                {
                    _pausedTime = DateTime.Now;
                    _onTimerPause?.Invoke(new TimerVO(GetCurrentProgress(), GetSecondsPassed(), GetTimeLeft()));
                }
            }
            else if (_isApplicationPaused)
            {
                _isApplicationPaused = false;
                if (!_timerPaused && _timerStarted)
                {
                    ResumeTimer();
                }
            }
        }

        public Timer PauseTimer()
        {
            _pausedTime = DateTime.Now;

            _timerPaused = true;
            _onTimerPause?.Invoke(new TimerVO(GetCurrentProgress(), GetSecondsPassed(), GetTimeLeft()));

            return this;
        }

        public Timer ResumeTimer()
        {
            double passedPauseTime = (DateTime.Now - _pausedTime).TotalSeconds;

            _startTime = _startTime.AddSeconds(passedPauseTime);
            _endTime = _endTime.AddSeconds(passedPauseTime);

            _timerPaused = false;
            _onTimerResume?.Invoke();

            return this;
        }

        public Timer StopTimer()
        {
            _timerStarted = false;
            _onTimerStop?.Invoke();

            return this;
        }

        private void FixedUpdate()
        {
            if (_timerStarted && !_timerPaused)
            {
                _onTimerUpdate?.Invoke(new TimerVO(GetCurrentProgress(), GetSecondsPassed(), GetTimeLeft()));
                
                if (DateTime.Now >= _endTime)
                {
                    _timerStarted = false;

                    _onTimerEnd?.Invoke();
                }
            }
        }

        private float GetSecondsPassed()
        {
            return (float) (DateTime.Now - _startTime).TotalSeconds;
        }

        private float GetCurrentProgress()
        {
            return 1f - (float) (_endTime - DateTime.Now).TotalSeconds / (float) (_endTime - _startTime).TotalSeconds;
        }

        private TimeSpan GetTimeLeft()
        {
            TimeSpan timeDifference = _endTime - DateTime.Now;

            return timeDifference;
        }

        #region CallbackFunctions

        public Timer OnUpdate(Action<TimerVO> onUpdateCallback)
        {
            _onTimerUpdate += onUpdateCallback;

            return this;
        }

        public Timer OnPause(Action<TimerVO> onPauseCallback)
        {
            _onTimerPause += onPauseCallback;

            return this;
        }

        public Timer OnResume(Action onResumeCallback)
        {
            _onTimerResume += onResumeCallback;

            return this;
        }

        public Timer OnTimerStop(Action onStopCallback)
        {
            _onTimerStop += onStopCallback;

            return this;
        }

        public Timer OnRestart(Action onRestartCallback)
        {
            _onTimerRestart += onRestartCallback;
            
            return this;
        }

        public Timer OnTimerEnd(Action onStopCallback)
        {
            _onTimerEnd += onStopCallback;

            return this;
        }

        #endregion

    }
}