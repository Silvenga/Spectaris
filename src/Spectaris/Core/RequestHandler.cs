using System;
using System.IO;
using System.Text.RegularExpressions;

using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class RequestHandler
    {
        private const string BodyTag = "<body>";
        private static readonly Regex BodySearch = new Regex(BodyTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ITimeline _timeline;
        private readonly IStorage _storage;
        private TimeSpan _requestStartTick;
        private TimeSpan _handlerStartTick;
        private TimeSpan _handlerTime;

        public RequestHandler(ITimeline timeline, IStorage storage)
        {
            _timeline = timeline;
            _storage = storage;
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
                context.AddRewrite(stream =>
                {
                    // Let the read start from the start of the memory stream.
                    // This is also required if another subscriber got here first. 
                    stream.Position = 0;
                    TextReader reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();

                    var output = $"<!--\r\nHistorical Metrics:\r\n{_storage}\r\n-->";
                    content = BodySearch.Replace(content, BodyTag + output);

                    // Zero the buffer so that the writer can start at the beginning.
                    // Also prevents corrupting the output if < original stream.
                    stream.SetLength(0);
                    TextWriter writer = new StreamWriter(stream);
                    writer.Write(content);
                    writer.Flush();
                });
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