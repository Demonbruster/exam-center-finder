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
        public async Task GetAvailableExamCenters_ReturnsListOfExamCenterDto()
        {
            // Arrange
            var examDuration = 2;
            var zipCode = "12345";
            var distance = 10;
            var distanceCalculatorServiceMock = new Mock<IDistanceCalculatorService>();
            var examSlotsRepositoryMock = new Mock<IExamSlotsRepository>();
            var loggerMock = new Mock<ILogger<AvailabilityService>>();

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
                    service.CalculateDistance(zipCode, distance))
                .ReturnsAsync(5.0);

            examSlotsRepositoryMock.Setup(repo =>
                    repo.GetSlotsByDurationAsync(examDuration))
                .ReturnsAsync(expectedExamSlots);

            var availabilityService = new AvailabilityService(
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
    }
}
