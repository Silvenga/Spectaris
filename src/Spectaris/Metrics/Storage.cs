using System;

namespace Spectaris.Metrics
{
    public interface IStorage
    {
        Guid StorageId { get; }
        MetricCounter TotalTimeMilliseconds { get; }
        MetricCounter RequestTimeMilliseconds { get; }
        MetricCounter RequestSizeBytes { get; }
        void AddRequest(TimeSpan totalTimeMs, TimeSpan requestTimeMs, long requestSizeBytes);
    }

    public class Storage : IStorage
    {
        public Guid StorageId { get; } = Guid.NewGuid();

        public MetricCounter TotalTimeMilliseconds { get; } = new MetricCounter();
        public MetricCounter RequestTimeMilliseconds { get; } = new MetricCounter();
        public MetricCounter RequestSizeBytes { get; } = new MetricCounter();

        public void AddRequest(TimeSpan totalTimeMs, TimeSpan requestTimeMs, long requestSizeBytes)
        {
            TotalTimeMilliseconds.AddMeasurement((long) totalTimeMs.TotalMilliseconds);
            RequestTimeMilliseconds.AddMeasurement((long) requestTimeMs.TotalMilliseconds);
            RequestSizeBytes.AddMeasurement(requestSizeBytes);
        }

        public override string ToString()
        {
            return
                $"{nameof(StorageId)}: {StorageId}\r\n {nameof(TotalTimeMilliseconds)}: {TotalTimeMilliseconds}\r\n {nameof(RequestTimeMilliseconds)}: {RequestTimeMilliseconds}\r\n {nameof(RequestSizeBytes)}: {RequestSizeBytes}";
        }
    }
}