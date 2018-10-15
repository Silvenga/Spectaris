using System.IO;
using System.Web;

namespace Spectaris.Core
{
    public interface ICommonContent
    {
        void AddHeader(string name, string value);
    }

    public interface IRequestContent : ICommonContent
    {
        void AttachFilters();
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

        public long ResponseSizeBytes => 0;

        public void AddHeader(string name, string value)
        {
            _application.Response.AddOnSendingHeaders(context => context.Response.Headers.Add(name, value));
        }

        public void AttachFilters()
        {
            // application.Response.Filter = Stream.Null;
        }

        public WorkerContext(HttpApplication application)
        {
            _application = application;
        }
    }
}