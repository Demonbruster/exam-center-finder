using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Controllers;
using ExamCenterFinder.Api.Domain;
using ExamCenterFinder.Api.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace ExamCenterFinder.Test
{
    [TestFixture]
    public class AvailabilityControllerTests
    {
        [Test]
        public async Task GetAvailability_Returns_InternalServerError_OnException()
        {
            // Arrange
            var mockAvailabilityService = new Mock<IAvailabilityService>();
            var mockZipCodeRepo = new Mock<IZipCodeCenterPointRepository>();
            var logger = new Mock<ILogger<AvailabilityController>>();
            mockAvailabilityService.Setup(service =>
                    service.GetAvailalbleExamCentersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            var controller = new AvailabilityController(logger.Object, mockAvailabilityService.Object);

            // Act
            var result = await controller.GetAvailability(2, "12345", 10);

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = (ObjectResult)result;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task GetAvailability_WithInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AvailabilityController>>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();

            var controller = new AvailabilityController(loggerMock.Object, availabilityServiceMock.Object);

            // Act
            var result = await controller.GetAvailability(0, null, -5);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetAvailability_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AvailabilityController>>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var zipCodeCenterPointRepositoryMock = new Mock<IZipCodeCenterPointRepository>();

            var controller = new AvailabilityController(loggerMock.Object, availabilityServiceMock.Object);


            // Mock data
            var examDuration = 2;
            var zipCode = "12345";
            var distance = 10;

            var zipCodeDetails = new ZipCodeCenterPoint
            {
                Id = 1,
                ZipCode = zipCode,
                Latitude = 40.123,
                Longitude = -75.456
            };

            zipCodeCenterPointRepositoryMock.Setup(repo =>
                    repo.GetZipCodeCenterPointsByZipCodeAsync(zipCode))
                .ReturnsAsync(zipCodeDetails);

            var availabilityData = new List<ExamCenterDto>
            {
                new ExamCenterDto
                {
                    AvailabilityId = 1,
                    Name = "Test Center 1",
                    Address = "123 Main St",
                    StartTime = DateTime.Now,
                    Seats = 50,
                    Latitude = 40.123,
                    Longitude = -75.456,
                    DistanceMiles = 5.0
                },
                // Add more test data if needed
            };

            availabilityServiceMock.Setup(service =>
                    service.GetAvailalbleExamCentersAsync(examDuration, zipCode, distance))
                .ReturnsAsync(availabilityData);

            // Act
            var result = await controller.GetAvailability(examDuration, zipCode, distance);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult)result;
            Assert.That(okObjectResult.Value, Is.TypeOf<AvailablitiesDto>());

            var model = (AvailablitiesDto)okObjectResult.Value;
            Assert.That(model.Availability, Is.EqualTo(availabilityData));
        }

        [Test]
        public async Task GetAvailability_Returns_NotFound_WhenNoCentersAvailable()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AvailabilityController>>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();

            // Assume the service returns an empty list when no centers are available
            availabilityServiceMock.Setup(service =>
                service.GetAvailalbleExamCentersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ExamCenterDto>());

            var controller = new AvailabilityController(loggerMock.Object, availabilityServiceMock.Object);

            // Act
            var result = await controller.GetAvailability(2, "12345", 10);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.EqualTo("There no available centers matching the request"));
        }

    }
}
