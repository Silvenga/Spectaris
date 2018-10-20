using System;

namespace Spectaris.Metrics
{
    public interface IStorage
    {
        Guid StorageId { get; }
        MetricCounter TotalTimeMilliseconds { get; }
        MetricCounter HandlerTimeMilliseconds { get; }
        MetricCounter RequestSizeBytes { get; }
        void AddRequest(TimeSpan totalTimeMs, TimeSpan handlerTimeMs, long requestSizeBytes);
    }

    public class Storage : IStorage
    {
        public Guid StorageId { get; } = Guid.NewGuid();

        public MetricCounter TotalTimeMilliseconds { get; } = new MetricCounter();
        public MetricCounter HandlerTimeMilliseconds { get; } = new MetricCounter();
        public MetricCounter RequestSizeBytes { get; } = new MetricCounter();

        public void AddRequest(TimeSpan totalTimeMs, TimeSpan handlerTimeMs, long requestSizeBytes)
        {
            TotalTimeMilliseconds.AddMeasurement((long)totalTimeMs.TotalMilliseconds);
            HandlerTimeMilliseconds.AddMeasurement((long)handlerTimeMs.TotalMilliseconds);
            RequestSizeBytes.AddMeasurement(requestSizeBytes);
        }

        public override string ToString()
        {
            return
                $"{nameof(StorageId)}: {StorageId}\r\n {nameof(TotalTimeMilliseconds)}: {TotalTimeMilliseconds}\r\n {nameof(HandlerTimeMilliseconds)}: {HandlerTimeMilliseconds}\r\n {nameof(RequestSizeBytes)}: {RequestSizeBytes}";
        }
    }
}