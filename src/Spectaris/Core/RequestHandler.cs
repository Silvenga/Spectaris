using System;

namespace Spectaris.Core
{
    public class RequestHandler
    {
        private readonly Guid _requestId = Guid.NewGuid();
        private readonly Guid _workerId;

        public RequestHandler(Guid workerId)
        {
            _workerId = workerId;
        }

        public void Start(IWorkerContext context)
        {
        }

        public void End(IWorkerContext context)
        {

        }
    }
}