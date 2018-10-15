using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class RequestHandlerFactory
    {
        private static readonly IStorage SharedStorage = new Storage(); // Being lazy here - by avoiding laziness...

        private readonly ITimeline _timeline = new Timeline(new Timepiece());

        public RequestHandler Create()
        {
            return new RequestHandler(_timeline, SharedStorage);
        }
    }
}