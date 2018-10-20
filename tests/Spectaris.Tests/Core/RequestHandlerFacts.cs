using System;
using System.IO;

using AutoFixture;

using FluentAssertions;

using NSubstitute;

using Spectaris.Core;
using Spectaris.Metrics;

using Xunit;

namespace Spectaris.Tests.Core
{
    public class RequestHandlerFacts
    {
        private static readonly Fixture Autofixture = new Fixture();

        private readonly ITimeline _timelineMock = Substitute.For<ITimeline>();
        private readonly IStorage _storageMock = Substitute.For<IStorage>();
        private readonly RequestHandler _handler;

        public RequestHandlerFacts()
        {
            _handler = new RequestHandler(_timelineMock, _storageMock, new MetricsDisplayHtmlRewriter(_storageMock));

            _storageMock.HandlerTimeMilliseconds.Returns(Substitute.For<IMetricCounter>());
            _storageMock.RequestSizeBytes.Returns(Substitute.For<IMetricCounter>());
            _storageMock.TotalTimeMilliseconds.Returns(Substitute.For<IMetricCounter>());
        }

        [Fact]
        public void When_request_is_processed_then_handle_lifecyle_events()
        {
            var requestContextMock = Substitute.For<IRequestContext>();
            var responseContextMock = Substitute.For<IResponseContext>();
            var workerId = Autofixture.Create<Guid>();

            // Act
            Action action = () =>
            {
                _handler.Start(workerId, requestContextMock);
                _handler.BeginHandler();
                _handler.EndHandler();
                _handler.Writing(responseContextMock);
                _handler.End(responseContextMock);
            };

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_request_is_processed_then_store_total_request_time()
        {
            var requestContextMock = Substitute.For<IRequestContext>();
            var responseContextMock = Substitute.For<IResponseContext>();
            var workerId = Autofixture.Create<Guid>();

            var startTickFake = Autofixture.Create<TimeSpan>();
            _timelineMock.GetCurrentTick().Returns(startTickFake);

            var timespanFake = Autofixture.Create<TimeSpan>();
            _timelineMock.GetTimeSinceTick(startTickFake).Returns(timespanFake);

            // Act
            _handler.Start(workerId, requestContextMock);
            _handler.End(responseContextMock);

            // Assert
            _storageMock.Received().AddRequest(timespanFake, Arg.Any<TimeSpan>(), Arg.Any<long>());
        }

        [Fact]
        public void When_a_request_is_processed_then_store_handler_request_time()
        {
            var responseContextMock = Substitute.For<IResponseContext>();

            var startTickFake = Autofixture.Create<TimeSpan>();
            _timelineMock.GetCurrentTick().Returns(startTickFake);

            var timespanFake = Autofixture.Create<TimeSpan>();
            _timelineMock.GetTimeSinceTick(startTickFake).Returns(timespanFake);

            // Act
            _handler.BeginHandler();
            _handler.EndHandler();
            _handler.End(responseContextMock);

            // Assert
            _storageMock.Received().AddRequest(Arg.Any<TimeSpan>(), timespanFake, Arg.Any<long>());
        }

        [Fact]
        public void When_a_request_is_processed_then_store_response_bytes()
        {
            var responseContextMock = Substitute.For<IResponseContext>();

            var responseSizeFake = Autofixture.Create<long>();
            responseContextMock.ResponseSizeBytes.Returns(responseSizeFake);

            // Act
            _handler.End(responseContextMock);

            // Assert
            _storageMock.Received().AddRequest(Arg.Any<TimeSpan>(), Arg.Any<TimeSpan>(), responseSizeFake);
        }

        [Fact]
        public void When_a_request_spooling_and_content_type_is_html_then_rewrite()
        {
            var responseContextMock = Substitute.For<IResponseContext>();
            responseContextMock.ContentType.Returns("text/html; utf-8");

            // Act
            _handler.Writing(responseContextMock);

            // Assert
            responseContextMock.Received().AddRewrite(Arg.Any<Action<MemoryStream>>());
        }

        [Fact]
        public void When_a_request_spooling_and_content_type_is_not_html_then_do_not_rewrite()
        {
            var responseContextMock = Substitute.For<IResponseContext>();
            responseContextMock.ContentType.Returns(Autofixture.Create<string>());

            // Act
            _handler.Writing(responseContextMock);

            // Assert
            responseContextMock.DidNotReceive().AddRewrite(Arg.Any<Action<MemoryStream>>());
        }
    }
}