using System.IO;
using System.Text.RegularExpressions;

using Spectaris.Metrics;

namespace Spectaris.Core
{
    public class MetricsDisplayHtmlRewriter
    {
        private const string BodyTag = "<body>";
        private static readonly Regex BodySearch = new Regex(BodyTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IStorage _storage;

        public MetricsDisplayHtmlRewriter(IStorage storage)
        {
            _storage = storage;
        }

        public void Rewrite(MemoryStream stream)
        {
            // Let the read start from the start of the memory stream.
            // This is also required if another subscriber got here first. 
            stream.Position = 0;
            TextReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            var humanReadable = _storage.GetHumanReadable();
            var output = $"<!--{humanReadable}-->";
            content = BodySearch.Replace(content, BodyTag + output);

            // Zero the buffer so that the writer can start at the beginning.
            // Also prevents corrupting the output if < original stream.
            stream.SetLength(0);
            TextWriter writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
        }
    }
}