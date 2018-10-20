using System;

namespace Spectaris.Metrics
{
    public interface IStorage
    {
        Guid StorageId { get; }
        IMetricCounter TotalTimeMilliseconds { get; }
        IMetricCounter HandlerTimeMilliseconds { get; }
        IMetricCounter RequestSizeBytes { get; }
        void AddRequest(TimeSpan totalTimeMs, TimeSpan handlerTimeMs, long requestSizeBytes);
    }

    public class Storage : IStorage
    {
        public Guid StorageId { get; } = Guid.NewGuid();

        public IMetricCounter TotalTimeMilliseconds { get; } = new MetricCounter();
        public IMetricCounter HandlerTimeMilliseconds { get; } = new MetricCounter();
        public IMetricCounter RequestSizeBytes { get; } = new MetricCounter();

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