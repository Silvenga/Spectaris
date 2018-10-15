using System;

namespace Spectaris.Core
{
    public class Worker : IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly IWorkerContext _context;
        private readonly RequestHandlerFactory _handlerFactory;

        private RequestHandler _currentRequestHandler;

        public Worker(IWorkerContext context, RequestHandlerFactory handlerFactory)
        {
            _context = context;
            _handlerFactory = handlerFactory;
        }

        public void BeginRequest()
        {
            _currentRequestHandler = _handlerFactory.Create();
            _currentRequestHandler.Start(Id, _context);
        }

        public void EndRequest()
        {
            _currentRequestHandler.End(_context);
        }

        public void Dispose()
        {
        }
    }
}