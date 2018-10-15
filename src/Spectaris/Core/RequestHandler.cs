using System;
using System.Globalization;

using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class RequestHandler
    {
        private readonly ITimeline _timeline;
        private readonly IStorage _storage;
        private TimeSpan _startTick;

        public RequestHandler(ITimeline timeline, IStorage storage)
        {
            _timeline = timeline;
            _storage = storage;
        }

        public void Start(Guid workerId, IRequestContent context)
        {
            context.AddHeader("X-Spectaris-WorkerId", workerId.ToString());

            _startTick = _timeline.GetCurrentTick();
        }

        public void End(IResponseContext context)
        {
            var requestTime = _timeline.GetTimeSinceTick(_startTick);
            _storage.AddRequest(requestTime, requestTime, context.ResponseSizeBytes);

            context.AddHeader("X-Spectaris-Count", _storage.TotalTimeMilliseconds.Count.ToString());
            context.AddHeader("X-Spectaris-TotalMs", _storage.TotalTimeMilliseconds.Total.ToString());
            context.AddHeader("X-Spectaris-AverageMs", _storage.TotalTimeMilliseconds.Average.ToString(CultureInfo.InvariantCulture));
            context.AddHeader("X-Spectaris-TotalBytes", _storage.RequestSizeBytes.Total.ToString());
            context.AddHeader("X-Spectaris-AverageBytes", _storage.RequestSizeBytes.Average.ToString(CultureInfo.InvariantCulture));
        }
    }
}