using System;
using System.Threading;

namespace Spectaris.Metrics
{
    public interface IMetricCounter
    {
        long Total { get; }
        long Count { get; }
        long Minimum { get; }
        long Maximum { get; }
        decimal Average { get; }
        void AddMeasurement(long value);
    }

    public class MetricCounter : IMetricCounter
    {
        // ReSharper disable once ArgumentsStyleLiteral
        private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: true);

        public long Total { get; private set; }
        public long Count { get; private set; }
        public long Minimum { get; private set; } = long.MaxValue;
        public long Maximum { get; private set; } = long.MinValue;
        public decimal Average { get; private set; }

        public void AddMeasurement(long value)
        {
            var lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);

                Total += value;
                Count++;
                Minimum = value < Minimum ? value : Minimum;
                Maximum = value > Maximum ? value : Maximum;
                Average = Total / (decimal) Count;
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit(false);
                }
            }
        }

        public override string ToString()
        {
            return
                $"{nameof(Total)}: {Total}, {nameof(Count)}: {Count}, {nameof(Minimum)}: {Minimum}, {nameof(Maximum)}: {Maximum}, {nameof(Average)}: {Math.Round(Average, 2)}";
        }
    }
}