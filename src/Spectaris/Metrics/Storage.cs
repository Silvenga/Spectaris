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

        public IMetricCounter TotalTimeMilliseconds { get; }
        public IMetricCounter HandlerTimeMilliseconds { get; }
        public IMetricCounter RequestSizeBytes { get; }

        public Storage(IMetricCounter totalTimeMilliseconds, IMetricCounter handlerTimeMilliseconds, IMetricCounter requestSizeBytes)
        {
            TotalTimeMilliseconds = totalTimeMilliseconds;
            HandlerTimeMilliseconds = handlerTimeMilliseconds;
            RequestSizeBytes = requestSizeBytes;
        }

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