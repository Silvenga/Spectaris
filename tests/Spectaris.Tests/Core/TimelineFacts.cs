using System;

using AutoFixture;

using FluentAssertions;

using NSubstitute;

using Spectaris.Core;

using Xunit;

namespace Spectaris.Tests.Core
{
    public class TimelineFacts
    {
        private static readonly Fixture Autofixture = new Fixture();

        [Fact]
        public void When_timeline_is_constructed_then_start_timepiece()
        {
            var timepieceMock = Substitute.For<ITimepiece>();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            new Timeline(timepieceMock);

            // Assert
            timepieceMock.Received().Start();
        }

        [Fact]
        public void When_tick_is_requested_then_return_current_timer_time()
        {
            var timepieceMock = Substitute.For<ITimepiece>();
            var timeline = new Timeline(timepieceMock);

            var expected = Autofixture.Create<TimeSpan>();
            timepieceMock.Elapsed.Returns(expected);

            // Act
            var result = timeline.GetCurrentTick();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void When_time_since_tick_is_requested_then_return_span()
        {
            var timepieceMock = Substitute.For<ITimepiece>();
            var timeline = new Timeline(timepieceMock);

            var lastTick = Autofixture.Create<TimeSpan>();
            var expected = Autofixture.Create<TimeSpan>();
            var currentTick = lastTick + expected;
            timepieceMock.Elapsed.Returns(currentTick);

            // Act
            var result = timeline.GetTimeSinceTick(lastTick);

            // Assert
            result.Should().Be(expected);
        }
    }
}