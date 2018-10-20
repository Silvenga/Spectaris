using AutoFixture;

using FluentAssertions;

using Spectaris.Metrics;

using Xunit;

namespace Spectaris.Tests.Metrics
{
    public class MetricCounterFacts
    {
        private static readonly Fixture Autofixture = new Fixture();
        private readonly IMetricCounter _metricCounter = new MetricCounter();

        [Fact]
        public void When_a_metric_is_added_then_count_should_increment()
        {
            // Act
            _metricCounter.AddMeasurement(Autofixture.Create<long>());

            // Assert
            _metricCounter.Count.Should().Be(1);
        }

        [Fact]
        public void When_a_metric_is_added_then_total_should_increment()
        {
            var measurementFake = Autofixture.Create<long>();

            // Act
            _metricCounter.AddMeasurement(measurementFake);

            // Assert
            _metricCounter.Total.Should().Be(measurementFake);
        }

        [Fact]
        public void When_multiple_metrics_are_added_then_average_should_be_an_average_of_all_measurements()
        {
            var measurementFake1 = Autofixture.Create<long>();
            var measurementFake2 = Autofixture.Create<long>();

            // Act
            _metricCounter.AddMeasurement(measurementFake1);
            _metricCounter.AddMeasurement(measurementFake2);

            // Assert
            var expected = (measurementFake1 + measurementFake2) / 2m;
            _metricCounter.Average.Should().Be(expected);
        }

        [Fact]
        public void When_a_metric_is_added_and_the_metric_is_smaller_then_current_min_then_min_should_be_the_measurement()
        {
            const int measurementFake1 = 10;
            const int measurementFake2 = 5;

            _metricCounter.AddMeasurement(measurementFake1);

            // Act
            _metricCounter.AddMeasurement(measurementFake2);

            // Assert
            _metricCounter.Minimum.Should().Be(measurementFake2);
        }

        [Fact]
        public void When_a_metric_is_added_and_the_metric_is_larger_then_current_max_then_max_should_be_the_measurement()
        {
            const int measurementFake1 = 5;
            const int measurementFake2 = 10;

            _metricCounter.AddMeasurement(measurementFake1);

            // Act
            _metricCounter.AddMeasurement(measurementFake2);

            // Assert
            _metricCounter.Maximum.Should().Be(measurementFake2);
        }
    }
}