﻿using System.IO;
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
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory);
            limitedContentRewritingFilter.RewriteAction += stream => { rewriteStream = new MemoryStream(stream.ToArray()); };

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
        public void When_content_is_flushed_and_there_are_no_subscribers_flush_cached_content()
        {
            var contentFake = Autofixture.Create<string>();

            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory);

            // Act
            using (var writer = new StreamWriter(limitedContentRewritingFilter))
            {
                writer.Write(contentFake);
            }

            // Assert
            memory.Position = 0;
            var result = new StreamReader(memory).ReadToEnd();
            result.Should().Be(contentFake);
        }

        [Fact]
        public void When_content_is_rewritten_use_modified_stream()
        {
            var contentFake = Autofixture.Create<string>();
            var modifiedFake = Autofixture.CreateMany<byte>().ToArray();

            void RewriteAction(MemoryStream stream)
            {
                stream.SetLength(0);
                stream.Write(modifiedFake, 0, modifiedFake.Length);
            }

            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory);
            limitedContentRewritingFilter.RewriteAction += RewriteAction;

            // Act
            using (var writer = new StreamWriter(limitedContentRewritingFilter))
            {
                writer.Write(contentFake);
            }

            // Assert
            memory.ToArray().Should().BeEquivalentTo(modifiedFake);
        }

        [Fact]
        public void When_content_is_larger_then_max_then_do_not_rewrite()
        {
            var contentFake1 = Autofixture.CreateMany<byte>().ToArray();

            var rewriteCalled = false;

            var memory = new MemoryStream();
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory, contentFake1.Length - 1);
            limitedContentRewritingFilter.RewriteAction += stream => rewriteCalled = true;

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
            var limitedContentRewritingFilter = new LimitedContentRewritingFilter(memory, contentFake1.Length + contentFake2.Length);
            limitedContentRewritingFilter.RewriteAction += stream => rewriteCalled = true;

            // Act
            limitedContentRewritingFilter.Write(contentFake1, 0, contentFake1.Length);
            limitedContentRewritingFilter.Write(contentFake2, 0, contentFake2.Length);

            // Assert
            rewriteCalled.Should().BeFalse();
        }
    }
}