using System.IO;
using System.Linq;

using AutoFixture;

using FluentAssertions;

using Spectaris.Filters;

using Xunit;

namespace Spectaris.Tests.Filters
{
    public class LimitedContentRewritingFilterFact
    {
        private static readonly Fixture Autofixture = new Fixture();

        [Fact]
        public void When_content_is_flushed_and_content_was_written_then_call_rewrite_action()
        {
            var contentFake = Autofixture.Create<string>();

            MemoryStream rewriteStream = null;
            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory, stream => { rewriteStream = new MemoryStream(stream.ToArray()); });

            // Act
            using (var writer = new StreamWriter(limitedContentRewritingFilter))
            {
                writer.Write(contentFake);
            }

            // Assert
            rewriteStream.Should().NotBeNull();
            rewriteStream.ToArray().Should().BeEquivalentTo(memory.ToArray());
        }

        [Fact]
        public void When_content_is_larger_then_max_then_do_not_rewrite()
        {
            var contentFake1 = Autofixture.CreateMany<byte>().ToArray();

            var rewriteCalled = false;

            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory, stream => rewriteCalled = true, contentFake1.Length - 1);

            // Act
            limitedContentRewritingFilter.Write(contentFake1, 0, contentFake1.Length);

            // Assert
            rewriteCalled.Should().BeFalse();
        }

        [Fact]
        public void When_content_is_larger_then_max_then_flush_cache()
        {
            // This one is a little hard to test, so this is just here for my own convenience...

            var contentFake1 = Autofixture.CreateMany<byte>().ToArray();
            var contentFake2 = Autofixture.CreateMany<byte>().ToArray();

            var rewriteCalled = false;

            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory, stream => rewriteCalled = true, contentFake1.Length + contentFake2.Length);

            // Act
            limitedContentRewritingFilter.Write(contentFake1, 0, contentFake1.Length);
            limitedContentRewritingFilter.Write(contentFake2, 0, contentFake2.Length);

            // Assert
            rewriteCalled.Should().BeFalse();
        }
    }
}