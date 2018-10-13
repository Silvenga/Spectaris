using System;

namespace Spectaris.Core
{
    public interface ITimeline
    {
        TimeSpan GetCurrentTick();
        TimeSpan GetTimeSinceTick(TimeSpan tick);
    }

    public class Timeline
    {
        private readonly ITimepiece _timepiece;

        public Timeline(ITimepiece timepiece)
        {
            _timepiece = timepiece;
            _timepiece.Start();
        }

        public TimeSpan GetCurrentTick()
        {
            return _timepiece.Elapsed;
        }

        public TimeSpan GetTimeSinceTick(TimeSpan tick)
        {
            var currentTick = GetCurrentTick();
            return currentTick - tick;
        }
    }
}