using NUnit.Framework;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TourPlannerBusinessLayer.Service;
using TourPlannerBusinessLayer.Exceptions;

namespace TourPlannerTest
{
    [TestFixture]
    public class DirectionServiceTests
    {
        private Mock<MockableHttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private DirectionService _directionService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<MockableHttpMessageHandler> { CallBase = true };
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _directionService = new DirectionService(_httpClient);
        }

        [Test]
        public async Task GetDirectionsAsync_ShouldReturnDirections_ForValidRequest()
        {
            // Arrange
            var responseContent = "{\"routes\":[{\"segments\":[{\"distance\":10000,\"duration\":3600}]}]}";
            _mockHttpMessageHandler.Setup(m => m.SendAsyncPublic(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _directionService.GetDirectionsAsync(16.3738, 48.2082, 16.3738, 48.2082, "test-api-key", "Car");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("routes"));
        }

        [Test]
        public async Task GetDirectionsAsync_ShouldReturnNull_ForInvalidTransportType()
        {
            // Act
            var result = await _directionService.GetDirectionsAsync(16.3738, 48.2082, 16.3738, 48.2082, "test-api-key", "InvalidTransportType");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetRouteDistanceAndDurationAsync_ShouldReturnDistanceAndDuration_ForValidRequest()
        {
            // Arrange
            var responseContent = "{\"features\":[{\"properties\":{\"segments\":[{\"distance\":10000,\"duration\":3600}]}}]}";
            _mockHttpMessageHandler.Setup(m => m.SendAsyncPublic(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var (distance, duration) = await _directionService.GetRouteDistanceAndDurationAsync(16.3738, 48.2082, 16.3738, 48.2082, "Car", "test-api-key");

            // Assert
            Assert.AreEqual("10,00 km", distance);
            Assert.AreEqual("1,00 h", duration);
        }

        [Test]
        public async Task GetRouteDistanceAndDurationAsync_ShouldReturnNull_ForInvalidTransportType()
        {
            // Act
            var (distance, duration) = await _directionService.GetRouteDistanceAndDurationAsync(16.3738, 48.2082, 16.3738, 48.2082, "InvalidTransportType", "test-api-key");

            // Assert
            Assert.IsNull(distance);
            Assert.IsNull(duration);
        }

        [Test]
        public void GetDirectionsAsync_ShouldThrowDirectionsServiceException_ForUnsuccessfulResponse()
        {
            // Arrange
            _mockHttpMessageHandler.Setup(m => m.SendAsyncPublic(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Bad Request")
                });

            // Act & Assert
            var ex = Assert.ThrowsAsync<DirectionsServiceException>(async () =>
                await _directionService.GetDirectionsAsync(16.3738, 48.2082, 16.3738, 48.2082, "test-api-key", "Car"));

            Assert.That(ex.Message, Is.EqualTo("Error fetching directions data: Error fetching directions data: BadRequest - Bad Request - Bad Request"));
        }

        [Test]
        public void GetRouteDistanceAndDurationAsync_ShouldThrowDirectionsServiceException_ForUnsuccessfulResponse()
        {
            // Arrange
            _mockHttpMessageHandler.Setup(m => m.SendAsyncPublic(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Error fetching")
                });

            // Act & Assert
            var ex = Assert.ThrowsAsync<DirectionsServiceException>(async () =>
                await _directionService.GetRouteDistanceAndDurationAsync(16.3738, 48.2082, 16.3738, 48.2082, "Car", "test-api-key"));
            Assert.That(ex.Message, Is.EqualTo("Error fetching directions data: Error fetching directions data: BadRequest - Bad Request - Error fetching"));
        }


        public class MockableHttpMessageHandler : HttpMessageHandler
        {
            public virtual Task<HttpResponseMessage> SendAsyncPublic(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return SendAsyncPublic(request, cancellationToken);
            }
        }
    }
}
