using System.Web;

using Spectaris.Core;

namespace Spectaris
{
    public class HttpModule : IHttpModule
    {
        private Worker _worker;

        public void Init(HttpApplication context)
        {
            var workerContext = new WorkerContext(context);
            _worker = new Worker(workerContext);

            context.BeginRequest += (sender, args) => _worker.BeginRequest();
            context.EndRequest += (sender, args) => _worker.EndRequest();
        }

        public void Dispose()
        {
            _worker?.Dispose();
        }
    }
}