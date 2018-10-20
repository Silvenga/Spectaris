using System;

using AutoFixture;

using NSubstitute;

using Spectaris.Metrics;

using Xunit;

namespace Spectaris.Tests.Metrics
{
    public class StorageFacts
    {
        private static readonly Fixture Autofixture = new Fixture();

        private readonly IStorage _storage;
        private readonly IMetricCounter _totalTimeMilliseconds = Substitute.For<IMetricCounter>();
        private readonly IMetricCounter _handlerTimeMilliseconds = Substitute.For<IMetricCounter>();
        private readonly IMetricCounter _requestSizeBytes = Substitute.For<IMetricCounter>();

        public StorageFacts()
        {
            _storage = new Storage(_totalTimeMilliseconds, _handlerTimeMilliseconds, _requestSizeBytes);
        }

        [Fact]
        public void When_recording_request_then_store_total_time()
        {
            var totalRequestTimeFake = Autofixture.Create<TimeSpan>();
            var handlerTimeFake = Autofixture.Create<TimeSpan>();
            var responseSizeFake = Autofixture.Create<long>();

            // Act
            _storage.AddRequest(totalRequestTimeFake, handlerTimeFake, responseSizeFake);

            // Assert
            _totalTimeMilliseconds.Received().AddMeasurement((long)totalRequestTimeFake.TotalMilliseconds);
        }

        [Fact]
        public void When_recording_request_then_store_handler_time()
        {
            var totalRequestTimeFake = Autofixture.Create<TimeSpan>();
            var handlerTimeFake = Autofixture.Create<TimeSpan>();
            var responseSizeFake = Autofixture.Create<long>();

            // Act
            _storage.AddRequest(totalRequestTimeFake, handlerTimeFake, responseSizeFake);

            // Assert
            _handlerTimeMilliseconds.Received().AddMeasurement((long)handlerTimeFake.TotalMilliseconds);
        }

        [Fact]
        public void When_recording_request_then_store_response_size()
        {
            var totalRequestTimeFake = Autofixture.Create<TimeSpan>();
            var handlerTimeFake = Autofixture.Create<TimeSpan>();
            var responseSizeFake = Autofixture.Create<long>();

            // Act
            _storage.AddRequest(totalRequestTimeFake, handlerTimeFake, responseSizeFake);

            // Assert
            _requestSizeBytes.Received().AddMeasurement(responseSizeFake);
        }
    }
}