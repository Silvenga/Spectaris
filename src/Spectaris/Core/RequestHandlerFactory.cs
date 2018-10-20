using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class RequestHandlerFactory
    {
        private static readonly IStorage SharedStorage = new Storage(
            new MetricCounter(),
            new MetricCounter(),
            new MetricCounter()
        ); // Being lazy here...

        private readonly ITimeline _timeline = new Timeline(new Timepiece());

        public RequestHandler Create()
        {
            return new RequestHandler(_timeline, SharedStorage, new MetricsDisplayHtmlRewriter(SharedStorage));
        }
    }
}