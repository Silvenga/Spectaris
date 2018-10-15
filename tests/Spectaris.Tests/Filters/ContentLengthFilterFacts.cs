using System.IO;

using AutoFixture;

using FluentAssertions;

using Spectaris.Filters;

using Xunit;

namespace Spectaris.Tests.Filters
{
    public class ContentLengthFilterFacts
    {
        private static readonly Fixture Autofixture = new Fixture();

        [Fact]
        public void When_content_is_written_then_observed_length_should_be_set()
        {
            var contentFake = Autofixture.Create<string>();

            var memory = new MemoryStream();
            var contentLengthFilter = new ContentLengthFilter(memory);

            // Act
            using (var writer = new StreamWriter(contentLengthFilter))
            {
                writer.Write(contentFake);
            }

            // Assert
            contentLengthFilter.ObservedContentLengthBytes.Should().Be(contentFake.Length);
        }
    }
}