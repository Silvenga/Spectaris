using System;

namespace Spectaris.Core
{
    public class Worker : IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly RequestHandlerFactory _handlerFactory;
        private readonly Func<IWorkerContext> _workerContextFactory;

        private RequestHandler _currentRequestHandler;
        private IWorkerContext _currentWorkerContext;

        public Worker(RequestHandlerFactory handlerFactory, Func<IWorkerContext> workerContextFactory)
        {
            _handlerFactory = handlerFactory;
            _workerContextFactory = workerContextFactory;
        }

        public void BeginRequest()
        {
            _currentRequestHandler = _handlerFactory.Create();
            _currentWorkerContext = _workerContextFactory();

            _currentRequestHandler.Start(Id, _currentWorkerContext);
        }

        public void EndRequest()
        {
            _currentRequestHandler.End(_currentWorkerContext);
        }

        public void Dispose()
        {
        }
    }
}