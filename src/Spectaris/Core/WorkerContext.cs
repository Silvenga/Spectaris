using System.Web;

namespace Spectaris.Core
{
    public interface IWorkerContext
    {
    }

    public class WorkerContext : IWorkerContext
    {
        private readonly HttpApplication _application;

        public WorkerContext(HttpApplication application)
        {
            _application = application;
        }
    }
}