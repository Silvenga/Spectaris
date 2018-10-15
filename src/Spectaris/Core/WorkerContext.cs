using System.Web;

using Spectaris.Filters;

namespace Spectaris.Core
{
    public interface ICommonContent
    {
        void AddHeader(string name, string value);
    }

    public interface IRequestContent : ICommonContent
    {
    }

    public interface IResponseContext : ICommonContent
    {
        long ResponseSizeBytes { get; }
    }

    public interface IWorkerContext : IRequestContent, IResponseContext
    {
    }

    public class WorkerContext : IWorkerContext
    {
        private readonly HttpApplication _application;
        private readonly ContentLengthFilter _contentLengthFilter;
        private readonly LimitedContentRewritingFilter _limitedContentRewritingFilter;

        public long ResponseSizeBytes => _contentLengthFilter.ObservedContentLengthBytes;

        public WorkerContext(HttpApplication application)
        {
            _application = application;

            // TODO MINOR - move to factory.
            _application.Response.Filter = _contentLengthFilter = new ContentLengthFilter(_application.Response.Filter);
            _application.Response.Filter = _limitedContentRewritingFilter = new LimitedContentRewritingFilter(_application.Response.Filter);
        }

        public void AddHeader(string name, string value)
        {
            _application.Response.AddOnSendingHeaders(context => context.Response.Headers.Add(name, value));
        }
    }
}