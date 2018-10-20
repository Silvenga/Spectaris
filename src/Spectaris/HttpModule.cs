using System.Web;

using Spectaris.Core;

namespace Spectaris
{
    public class HttpModule : IHttpModule
    {
        private Worker _worker;

        public void Init(HttpApplication context)
        {
            var requestHandlerFactory = new RequestHandlerFactory();

            _worker = new Worker(requestHandlerFactory, () => new WorkerContext(context));

            context.BeginRequest += (sender, args) => _worker.BeginRequest();
            context.PreRequestHandlerExecute += (sender, args) => _worker.PreRequestHandlerExecute();
            context.PostRequestHandlerExecute += (sender, args) => _worker.PostRequestHandlerExecute();
            context.PostReleaseRequestState += (sender, args) => _worker.PostReleaseRequestState();
            context.EndRequest += (sender, args) => _worker.EndRequest();
        }

        public void Dispose()
        {
            _worker?.Dispose();
        }
    }
}