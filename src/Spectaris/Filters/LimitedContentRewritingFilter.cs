using System;
using System.IO;

namespace Spectaris.Filters
{
    public class LimitedContentRewritingFilter : PassThroughFilter
    {
        public event Action<MemoryStream> RewriteAction;

        private readonly int _maxObservedBytes;
        private readonly MemoryStream _memory = new MemoryStream();
        private long _observedBytes;

        public LimitedContentRewritingFilter(Stream originalStream, int maxObservedBytes = 1 * 1024) : base(originalStream)
        {
            _maxObservedBytes = maxObservedBytes;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _observedBytes += count;

            var canCache = _observedBytes < _maxObservedBytes;
            if (canCache)
            {
                _memory.Write(buffer, offset, count);
            }
            else
            {
                if (_memory.Length > 0)
                {
                    // Bail cache if too big.
                    FlushCache();
                }

                base.Write(buffer, offset, count);
            }
        }

        private void RewriteCache()
        {
            if (_memory.Length > 0)
            {
                OnRewriteAction(_memory);
            }
        }

        private void FlushCache()
        {
            base.Write(_memory.ToArray(), 0, (int)_memory.Length);
            _memory.SetLength(0);
        }

        public override void Flush()
        {
            RewriteCache();
            FlushCache();
            base.Flush();
        }

        public override void Close()
        {
            _memory?.Close(); // Not strictly required on memory streams, /shrug.
            base.Close();
        }

        protected virtual void OnRewriteAction(MemoryStream memory)
        {
            RewriteAction?.Invoke(memory);
        }
    }
}