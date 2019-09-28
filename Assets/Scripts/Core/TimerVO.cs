using System;

namespace ProjectName.Core
{
    public struct TimerVO
    {
        public float Progress { get; }
        public float SecondsPassed { get; }
        public TimeSpan Time { get; }

        public TimerVO(float progress, float secondsPassed, TimeSpan time)
        {
            Progress = progress;
            SecondsPassed = secondsPassed;
            Time = time;
        }
    }
}