using System;
using System.Diagnostics;

namespace Spectaris.Core
{
    public interface ITimepiece
    {
        TimeSpan Elapsed { get; }
        long ElapsedMilliseconds { get; }
        long ElapsedTicks { get; }
        bool IsRunning { get; }

        void Reset();
        void Restart();
        void Start();
        void Stop();
    }

    public class Timepiece : ITimepiece
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        public long ElapsedTicks => _stopwatch.ElapsedTicks;
        public TimeSpan Elapsed => _stopwatch.Elapsed;
        public bool IsRunning => _stopwatch.IsRunning;

        public void Reset() => _stopwatch.Reset();
        public void Restart() => _stopwatch.Restart();
        public void Start() => _stopwatch.Start();
        public void Stop() => _stopwatch.Stop();
    }
}