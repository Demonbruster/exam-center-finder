using System;
using System.Threading.Tasks;
using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Application.Services;
using ExamCenterFinder.Api.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExamCenterFinder.Test
{
    [TestFixture]
    public class DistanceCalculatorServiceTests
    {
        [Test]
        public async Task CalculateDistance_WithValidZipCode_ReturnsDistance()
        {
            // Arrange
            var zipCode = "12345";
            var distance = 10;
            var zipCodeCenterPointRepositoryMock = new Mock<IZipCodeCenterPointRepository>();
            var loggerMock = new Mock<ILogger<DistanceCalculatorService>>();

            var zipCodeData = new ZipCodeCenterPoint
            {
                Latitude = 40.123,
                Longitude = -75.456
            };

            zipCodeCenterPointRepositoryMock.Setup(repo =>
                    repo.GetZipCodeCenterPointsByZipCode(zipCode))
                .ReturnsAsync(zipCodeData);

            var distanceCalculatorService = new DistanceCalculatorService(
                zipCodeCenterPointRepositoryMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await distanceCalculatorService.CalculateDistance(zipCode, distance);

            // Assert
            Assert.That(result, Is.EqualTo(5535.9041318579557)); 
        }

        [Test]
        public void CalculateDistance_WithInvalidZipCode_ThrowsException()
        {
            // Arrange
            var zipCode = "InvalidZipCode";
            var distance = 10;
            var zipCodeCenterPointRepositoryMock = new Mock<IZipCodeCenterPointRepository>();
            var loggerMock = new Mock<ILogger<DistanceCalculatorService>>();

            zipCodeCenterPointRepositoryMock.Setup(repo =>
                    repo.GetZipCodeCenterPointsByZipCode(zipCode))
                .ReturnsAsync((ZipCodeCenterPoint)null); // Simulate null response for an invalid zip code

            var distanceCalculatorService = new DistanceCalculatorService(
                zipCodeCenterPointRepositoryMock.Object,
                loggerMock.Object
            );

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                distanceCalculatorService.CalculateDistance(zipCode, distance));
        }
    }
}
