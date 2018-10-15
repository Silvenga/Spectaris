using System.IO;

namespace Spectaris.Filters
{
    public class PassThroughFilter : Stream
    {
        private readonly Stream _originalStream;

        public override bool CanRead => _originalStream.CanRead;
        public override bool CanSeek => _originalStream.CanSeek;
        public override bool CanWrite => _originalStream.CanWrite;
        public override long Length => _originalStream.Length;

        public override long Position
        {
            get => _originalStream.Position;
            set => _originalStream.Position = value;
        }

        protected PassThroughFilter(Stream originalStream)
        {
            _originalStream = originalStream;
        }

        public override void Flush() => _originalStream.Flush();

        public override long Seek(long offset, SeekOrigin origin) => _originalStream.Seek(offset, origin);

        public override void SetLength(long value) => _originalStream.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count) => _originalStream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count) => _originalStream.Write(buffer, offset, count);
    }
}