using System;

namespace Spectaris.Core
{
    public class Worker : IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly IWorkerContext _context;

        private RequestHandler _currentRequestHandler;

        public Worker(IWorkerContext context)
        {
            _context = context;
        }

        public void BeginRequest()
        {
            _currentRequestHandler = new RequestHandler(Id);
            _currentRequestHandler.Start(_context);
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