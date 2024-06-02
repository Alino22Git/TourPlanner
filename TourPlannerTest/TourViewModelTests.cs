using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TourPlanner.ViewModels;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;
using Models;

namespace TourPlannerTests
{
    [TestFixture]
    public class TourViewModelTests
    {
        private Mock<TourService> _mockTourService;
        private Mock<RouteDataManager> _mockRouteDataManager;
        private TourViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockTourService = new Mock<TourService>();
            _mockRouteDataManager = new Mock<RouteDataManager>();
            _viewModel = new TourViewModel(_mockTourService.Object, _mockRouteDataManager.Object);
            _viewModel.Tours = new ObservableCollection<Tour>();
        }

        [Test]
        public void StartEditing_ShouldSetSelectedTour()
        {
            // Arrange
            var originalTour = new Tour
            {
                Id = 1,
                Name = "Original Tour",
                From = "A",
                To = "B",
                TransportType = "Car",
                TourLogs = new ObservableCollection<TourLog>()
            };

            var distance = "10 km";
            var time = "1 hour";
            _mockRouteDataManager.Setup(manager => manager.GetDistanceAndDurationAsync(originalTour.From, originalTour.To, originalTour.TransportType))
                .ReturnsAsync((distance, time));

            // Act
            _viewModel.StartEditing(originalTour);

            // Assert
            Assert.AreEqual(originalTour.Id, _viewModel.SelectedTour.Id);
            Assert.AreEqual(originalTour.Name, _viewModel.SelectedTour.Name);
            Assert.AreEqual(originalTour.From, _viewModel.SelectedTour.From);
            Assert.AreEqual(originalTour.To, _viewModel.SelectedTour.To);
            Assert.AreEqual(distance, _viewModel.SelectedTour.Distance);
            Assert.AreEqual(time, _viewModel.SelectedTour.Time);
            _mockRouteDataManager.Verify(manager => manager.GetDistanceAndDurationAsync(originalTour.From, originalTour.To, originalTour.TransportType), Times.Once);
        }

        [Test]
        public async Task SaveTourCommand_ShouldAddNewTour()
        {
            // Arrange
            var newTour = new Tour { Name = "New Tour", From = "A", To = "B", TransportType = "Car" };
            _viewModel.SelectedTour = newTour;

            var distance = "10 km";
            var time = "1 hour";
            _mockRouteDataManager.Setup(manager => manager.GetDistanceAndDurationAsync(newTour.From, newTour.To, newTour.TransportType))
                .ReturnsAsync((distance, time));
            _mockTourService.Setup(service => service.AddTourAsync(newTour))
                .Returns(Task.CompletedTask);

            // Act
            _viewModel.SaveTourCommand.Execute(null);

            // Assert
            Assert.AreEqual(distance, newTour.Distance);
            Assert.AreEqual(time, newTour.Time);
        }

        [Test]
        public async Task SaveTourCommand_ShouldUpdateExistingTour()
        {
            // Arrange
            var existingTour = new Tour { Id = 1, Name = "Existing Tour", From = "A", To = "B", TransportType = "Car" };
            _viewModel.SelectedTour = existingTour;
            _viewModel.OriginalTour = existingTour;

            var updatedTour = new Tour { Id = 1, Name = "Updated Tour", From = "A", To = "B", TransportType = "Car" };
            _viewModel.SelectedTour = updatedTour;

            

            // Act
            _viewModel.SaveTourCommand.Execute(null);

            // Assert
            Assert.AreEqual(updatedTour.Name, existingTour.Name);
            Assert.AreEqual(updatedTour.From, existingTour.From);
            Assert.AreEqual(updatedTour.To, existingTour.To);
        }

        [Test]
        public async Task DeleteTourCommand_ShouldDeleteTour()
        {
            // Arrange
            var tourToDelete = new Tour { Id = 1, Name = "Tour to delete" };
            _viewModel.SelectedTour = tourToDelete;
            _viewModel.Tours = new ObservableCollection<Tour> { tourToDelete };

            _mockTourService.Setup(service => service.DeleteTourAsync(tourToDelete))
                .Returns(Task.CompletedTask);

            // Act
            _viewModel.DeleteTourCommand.Execute(null);

            // Assert
            Assert.IsNull(_viewModel.SelectedTour);
            Assert.IsEmpty(_viewModel.Tours);
        }
    }
}
