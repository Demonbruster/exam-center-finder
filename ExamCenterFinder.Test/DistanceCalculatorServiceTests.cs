using System;
using System.Threading.Tasks;
using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Domain;
using ExamCenterFinder.Api.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExamCenterFinder.Test
{
    [TestFixture]
    public class DistanceCalculatorServiceTests
    {
        [Test]
        public async Task CalculateDistance_WithTwoPoints_ReturnsDistance()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DistanceCalculatorService>>();
            var fromLatitude = 10.00;
            var fromLongitude = 10.00;
            var toLatitude = 20.00;
            var toLongitude = 20.00;

            var distanceCalculatorService = new DistanceCalculatorService(
                loggerMock.Object
            );

            // Act
            var result = await distanceCalculatorService.CalculateDistanceAsync(fromLatitude, fromLongitude, toLatitude, toLongitude);

            // Assert
            Assert.That(result, Is.GreaterThan(0)); 
        }

        [Test]
        public async Task CalculateDistance_WithSamePoint_Returns0()
        {
            var loggerMock = new Mock<ILogger<DistanceCalculatorService>>();
            var latitude = 10.00;
            var longitude = 10.00;

            var distanceCalculatorService = new DistanceCalculatorService(
                loggerMock.Object
            );

            // Act
            var result = await distanceCalculatorService.CalculateDistanceAsync(latitude, longitude, latitude, longitude);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
