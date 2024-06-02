using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;
using Models;
using Microsoft.Web.WebView2.Core;
using System.Collections.ObjectModel;

namespace TourPlannerTest
{
    [TestFixture]
    public class ReportManagerTests
    {
        private ReportManager _reportManager;
        private Mock<TourService> _mockTourService;

        [SetUp]
        public void SetUp()
        {
            _mockTourService = new Mock<TourService>(MockBehavior.Strict, null);

            _reportManager = new ReportManager();
        }

        [Test]
        public void GenerateReport_ShouldCreatePdfDocument()
        {
            // Arrange
            var tour = new Tour
            {
                Name = "Test Tour",
                From = "A",
                To = "B",
                Distance = "100",
                Time = "2",
                Description = "A to B",
                TransportType = "Car",
                Popularity = 3,
                ChildFriendliness = 12,
                TourLogs = new ObservableCollection<TourLog>
                {
                    new TourLog { Date = DateTime.Now, Comment = "Good", Difficulty = "Easy", TotalDistance = 100, TotalTime = 2, Rating = "5" }
                }
            };
            var destinationPath = "test_report.pdf";

            // Act
            _reportManager.GenerateReport(tour, destinationPath, _mockTourService.Object);

            // Assert
            Assert.IsTrue(File.Exists(destinationPath));
            File.Delete(destinationPath);
        }

      

        [Test]
        public async Task GenerateSummaryReport_ShouldCreatePdfDocument()
        {
            // Arrange
            var mockTours = new List<Tour>
            {
                new Tour
                {
                    Name = "Tour 1",
                    TourLogs = new ObservableCollection<TourLog>
                    {
                        new TourLog { TotalDistance = 100, TotalTime = 2 },
                        new TourLog { TotalDistance = 150, TotalTime = 3 }
                    }
                }
            };
            _mockTourService.Setup(service => service.GetToursAsync()).ReturnsAsync(mockTours);
            var destinationPath = "summary_report.pdf";

            // Act
            _reportManager.GenerateSummaryReport(destinationPath, _mockTourService.Object);

            // Assert
            Assert.IsTrue(File.Exists(destinationPath));
            File.Delete(destinationPath);
        }
    }
}
