using System.IO;

using AutoFixture;

using FluentAssertions;

using NSubstitute;

using Spectaris.Core;
using Spectaris.Metrics;

using Xunit;

namespace Spectaris.Tests.Core
{
    public class MetricsDisplayHtmlRewriterFacts
    {
        private static readonly Fixture Autofixture = new Fixture();

        private readonly IStorage _storage = Substitute.For<IStorage>();

        [Fact]
        public void When_rewriting_and_body_exists_then_add_storage_content()
        {
            var displayMetricsFake = Autofixture.Create<string>();
            _storage.GetHumanReadable().Returns(displayMetricsFake);

            const string body = "<html><body></html>";
            var stream = TextToStream(body);

            var htmlRewriter = new MetricsDisplayHtmlRewriter(_storage);

            // Act
            htmlRewriter.Rewrite(stream);

            // Assert
            var resultingText = StreamToText(stream);
            var expected = $"<html><body><!--{displayMetricsFake}--></html>";
            resultingText.Should().Be(expected);
        }

        [Fact]
        public void When_rewriting_and_body_is_missing_then_do_not_rewrite()
        {
            var displayMetricsFake = Autofixture.Create<string>();
            _storage.GetHumanReadable().Returns(displayMetricsFake);

            var body = Autofixture.Create<string>();
            var stream = TextToStream(body);

            var htmlRewriter = new MetricsDisplayHtmlRewriter(_storage);

            // Act
            htmlRewriter.Rewrite(stream);

            // Assert
            var resultingText = StreamToText(stream);
            resultingText.Should().Be(body);
        }

        private static string StreamToText(Stream stream)
        {
            stream.Position = 0;
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static MemoryStream TextToStream(string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            return stream;
        }
    }
}