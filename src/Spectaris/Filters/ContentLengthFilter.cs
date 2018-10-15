using System.IO;

namespace Spectaris.Filters
{
    public class ContentLengthFilter : PassThroughFilter
    {
        public long ObservedContentLengthBytes { get; private set; }

        public ContentLengthFilter(Stream originalStream) : base(originalStream)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Position could be changed
            // iirc, the http stream does not support random access, so not a problem /shrug?
            ObservedContentLengthBytes += count;
            base.Write(buffer, offset, count);
        }
    }
}