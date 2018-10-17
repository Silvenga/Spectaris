using System;
using System.IO;
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
        string RequestUrl { get; }
    }

    public interface IResponseContext : ICommonContent
    {
        long ResponseSizeBytes { get; }
        string ContentType { get; }
        bool IsClientConnected { get; }
        void AddRewrite(Action<MemoryStream> rewriteAction);
    }

    public interface IWorkerContext : IRequestContent, IResponseContext
    {
    }

    public class WorkerContext : IWorkerContext
    {
        private readonly HttpApplication _application;
        private readonly ContentLengthFilter _contentLengthFilter;
        private readonly LimitedContentRewritingFilter _limitedContentRewritingFilter;

        public string RequestUrl => _application.Request.RawUrl;
        public long ResponseSizeBytes => _contentLengthFilter.ObservedContentLengthBytes;
        public string ContentType => _application.Response.ContentType;
        public bool IsClientConnected => _application.Response.IsClientConnected;

        public WorkerContext(HttpApplication application)
        {
            _application = application;

            // TODO MINOR - move to factory.
            _application.Response.Filter = _contentLengthFilter = new ContentLengthFilter(_application.Response.Filter);
            _application.Response.Filter = _limitedContentRewritingFilter = new LimitedContentRewritingFilter(_application.Response.Filter);
        }

        public void AddRewrite(Action<MemoryStream> rewriteAction)
        {
            _limitedContentRewritingFilter.RewriteAction += rewriteAction;
        }

        public void AddHeader(string name, string value)
        {
            _application.Response.AddOnSendingHeaders(context => context.Response.Headers.Add(name, value));
        }
    }
}