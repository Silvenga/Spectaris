using System;

using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class RequestHandler
    {
        private readonly ITimeline _timeline;
        private readonly IStorage _storage;
        private readonly MetricsDisplayHtmlRewriter _htmlRewriter;

        private TimeSpan _requestStartTick;
        private TimeSpan _handlerStartTick;
        private TimeSpan _handlerTime;

        public RequestHandler(ITimeline timeline, IStorage storage, MetricsDisplayHtmlRewriter htmlRewriter)
        {
            _timeline = timeline;
            _storage = storage;
            _htmlRewriter = htmlRewriter;
        }

        public void Start(Guid workerId, IRequestContext context)
        {
            _requestStartTick = _timeline.GetCurrentTick();

            context.AddHeader("X-Spectaris-WorkerId", workerId.ToString());
            context.AddHeader("X-Spectaris-StorageId", _storage.StorageId.ToString());
        }

        public void BeginHandler()
        {
            _handlerStartTick = _timeline.GetCurrentTick();
        }

        public void EndHandler()
        {
            _handlerTime = _timeline.GetTimeSinceTick(_handlerStartTick);
        }

        public void Writing(IResponseContext context)
        {
            if (context.ContentType.Contains("text/html"))
            {
                context.AddRewrite(stream => _htmlRewriter.Rewrite(stream));
            }
        }

        public void End(IResponseContext context)
        {
            // TODO MINOR - for some reason a single request invokes events on 3 workers when requesting /. This is causing some skewed metrics.
            // The first event appears to be correct - as completion actually sends headers to the client.
            // The other two requests are either for ~/default.aspx or ~/ - which makes me wonder if this is caused by the rewrite module.
            // In any case, these requests are canceled (requested) when the first request completes - ignoring for now.
            var requestTime = _timeline.GetTimeSinceTick(_requestStartTick);
            _storage.AddRequest(requestTime, _handlerTime, context.ResponseSizeBytes);

            context.AddHeader("X-Spectaris-Request-Count", _storage.TotalTimeMilliseconds.Count.ToString());
        }
    }
}