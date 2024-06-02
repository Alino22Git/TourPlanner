using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;
using TourPlannerLogging;

namespace TourPlannerTest
{
    [TestFixture]
    public class RouteDataManagerTests
    {
        private Mock<GeocodeService> _mockGeocodeService;
        private Mock<DirectionService> _mockDirectionService;
        private Mock<IConfiguration> _mockConfiguration;
        private RouteDataManager _routeDataManager;

        [SetUp]
        public void SetUp()
        {
            _mockGeocodeService = new Mock<GeocodeService>();
            _mockDirectionService = new Mock<DirectionService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["ApiKeys:OpenRouteService"]).Returns("test-api-key");

            _routeDataManager = new RouteDataManager(_mockGeocodeService.Object, _mockDirectionService.Object, _mockConfiguration.Object);
        }

        [Test]
        public async Task GetTourDataAsync_ShouldReturnTourData_ForValidInputs()
        {
            // Arrange
            string from = "Vienna";
            string to = "Graz";
            string transportType = "Car";

            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(from, "test-api-key"))
                .ReturnsAsync((16.3738, 48.2082));
            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(to, "test-api-key"))
                .ReturnsAsync((15.4395, 47.0707));
            _mockDirectionService.Setup(service => service.GetDirectionsAsync(16.3738, 48.2082, 15.4395, 47.0707, "test-api-key", transportType))
                .ReturnsAsync("mock directions");

            // Act
            var result = await _routeDataManager.GetTourDataAsync(from, to, transportType);

            // Assert
            Assert.AreEqual(16.3738, result.startLongitude);
            Assert.AreEqual(48.2082, result.startLatitude);
            Assert.AreEqual(15.4395, result.endLongitude);
            Assert.AreEqual(47.0707, result.endLatitude);
            Assert.AreEqual("mock directions", result.directions);
        }

        [Test]
        public async Task GetTourDataAsync_ShouldHandleException()
        {
            // Arrange
            string from = "InvalidPlace";
            string to = "Graz";
            string transportType = "Car";

            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(from, "test-api-key"))
                .ThrowsAsync(new Exception("Geocode error"));

            // Act
            var result = await _routeDataManager.GetTourDataAsync(from, to, transportType);

            // Assert
            Assert.AreEqual(0, result.startLongitude);
            Assert.AreEqual(0, result.startLatitude);
            Assert.AreEqual(0, result.endLongitude);
            Assert.AreEqual(0, result.endLatitude);
            Assert.AreEqual("", result.directions);
        }

        [Test]
        public async Task SaveDirectionsToFileAsync_ShouldSaveToFile()
        {
            // Arrange
            string directions = "{ \"mock\": \"directions\" }";
            string filePath = Path.GetTempFileName();

            // Act
            await _routeDataManager.SaveDirectionsToFileAsync(directions, filePath);

            // Assert
            string savedContent = await File.ReadAllTextAsync(filePath);
            Assert.AreEqual($"const directions = {directions};", savedContent);

            // Clean up
            File.Delete(filePath);
        }

        [Test]
        public void GetProjectResourcePath_ShouldReturnValidPath()
        {
            // Arrange
            string relativePath = "testFile.txt";

            // Act
            string result = _routeDataManager.GetProjectResourcePath(relativePath);

            // Assert
            StringAssert.Contains("Resources", result);
            StringAssert.Contains(relativePath, result);
        }

        [Test]
        public async Task GetDistanceAndDurationAsync_ShouldReturnDistanceAndDuration()
        {
            // Arrange
            string from = "Vienna";
            string to = "Graz";
            string transportType = "Car";

            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(from, "test-api-key"))
                .ReturnsAsync((16.3738, 48.2082));
            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(to, "test-api-key"))
                .ReturnsAsync((15.4395, 47.0707));
            _mockDirectionService.Setup(service => service.GetRouteDistanceAndDurationAsync(16.3738, 48.2082, 15.4395, 47.0707, transportType, "test-api-key"))
                .ReturnsAsync(("200 km", "2 hours"));

            // Act
            var result = await _routeDataManager.GetDistanceAndDurationAsync(from, to, transportType);

            // Assert
            Assert.AreEqual("200 km", result.Distance);
            Assert.AreEqual("2 hours", result.Duration);
        }

        [Test]
        public async Task GetDistanceAndDurationAsync_ShouldHandleException()
        {
            // Arrange
            string from = "InvalidPlace";
            string to = "Graz";
            string transportType = "Car";

            _mockGeocodeService.Setup(service => service.GetCoordinatesAsync(from, "test-api-key"))
                .ThrowsAsync(new Exception("Geocode error"));

            // Act
            var result = await _routeDataManager.GetDistanceAndDurationAsync(from, to, transportType);

            // Assert
            Assert.AreEqual("", result.Distance);
            Assert.AreEqual("", result.Duration);
        }
    }
}
