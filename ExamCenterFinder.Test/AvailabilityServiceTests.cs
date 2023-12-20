using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Application.Services;
using ExamCenterFinder.Api.Domain;
using ExamCenterFinder.Api.Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExamCenterFinder.Test
{
    [TestFixture]
    public class AvailabilityServiceTests
    {
        [Test]
        public async Task GetAvailableExamCenters_WithValidInput_ReturnsAvaliablities()
        {
            // Arrange
            var examDuration = 2;
            var zipCode = "12345";
            var distance = 10;
            var distanceCalculatorServiceMock = new Mock<IDistanceCalculatorService>();
            var examSlotsRepositoryMock = new Mock<IExamSlotsRepository>();
            var zipCodeRepositoryMock = new Mock<IZipCodeCenterPointRepository>();
            var loggerMock = new Mock<ILogger<AvailabilityService>>();

            var zipCodeObj = new ZipCodeCenterPoint { Id = 1, ZipCode = "11111", Latitude = 45.22738570006638, Longitude = -93.9960240952021 };

            var expectedExamSlots = new List<ExamSlot>
            {
                new ExamSlot
                {
                    Id = 1,
                    StartTime = DateTime.Now,
                    Duration = examDuration,
                    TotalSeats = 50,
                    ReservedSeats = 20,
                    ExamCenter = new ExamCenter
                    {
                        Id = 1,
                        Name = "Test Center 1",
                        StreetAddress = "123 Main St",
                        ZipCodeCenterPoint = new ZipCodeCenterPoint
                        {
                            Latitude = 40.123,
                            Longitude = -75.456
                        }
                    }
                },
            };

            distanceCalculatorServiceMock.Setup(service =>
                    service.CalculateDistance(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(5.0);

            examSlotsRepositoryMock.Setup(repo =>
                    repo.GetSlotsByDurationAsync(examDuration))
                .ReturnsAsync(expectedExamSlots);

            zipCodeRepositoryMock.Setup(repo =>
            repo.GetZipCodeCenterPointsByZipCode(zipCode))
                .ReturnsAsync(zipCodeObj);

            var availabilityService = new AvailabilityService(
                zipCodeRepositoryMock.Object,
                distanceCalculatorServiceMock.Object,
                examSlotsRepositoryMock.Object,
                loggerMock.Object
            );


            // Act
            var result = await availabilityService.GetAvailalbleExamCenters(examDuration, zipCode, distance);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<ExamCenterDto>>());
            var examCenterDtos = (List<ExamCenterDto>)result;
            Assert.That(examCenterDtos, Is.Not.Empty);
            Assert.That(examCenterDtos.Count, Is.EqualTo(expectedExamSlots.Count));
        }

        [Test]
        public async Task GetZipcodeException_With_InvalidZipcode()
        {
            // Arrange
            var examDuration = 2;
            var zipCode = "12345";
            var distance = 10;
            var distanceCalculatorServiceMock = new Mock<IDistanceCalculatorService>();
            var examSlotsRepositoryMock = new Mock<IExamSlotsRepository>();
            var zipCodeRepositoryMock = new Mock<IZipCodeCenterPointRepository>();
            var loggerMock = new Mock<ILogger<AvailabilityService>>();

            zipCodeRepositoryMock.Setup(repo =>
                    repo.GetZipCodeCenterPointsByZipCode(zipCode))
                .ThrowsAsync(new InvalidOperationException("Test exception"));

            var availabilityService = new AvailabilityService(
                zipCodeRepositoryMock.Object,
                distanceCalculatorServiceMock.Object,
                examSlotsRepositoryMock.Object,
                loggerMock.Object
            );

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await availabilityService.GetAvailalbleExamCenters(examDuration, zipCode, distance));

            Assert.That(exception.Message, Is.EqualTo("Test exception"));
        }
    }
}
