using NUnit.Framework; 
using System.Linq;
using TourPlanner;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlannerTest
{
    [TestFixture] 
    public class Tests
    {
        private TourViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new TourViewModel();
        }


        [Test]
        public void AddTourLog_ShouldAddTourLogToList()
        {
            var tourLog = new TourLog
            {
                
                Comment = "Great tour!",
                Difficulty = "Medium"
                
            };

            _viewModel.AddTourLog(tourLog);

            Assert.IsTrue(_viewModel.TourLogs.Contains(tourLog));
        }


        [Test]
        public void DeleteTourLog_ShouldRemoveTourLogFromList()
        {
            var tourLog = new TourLog
            {
                Comment = "Great tour!",
                Difficulty = "Medium"
            };

            _viewModel.AddTourLog(tourLog);
            _viewModel.DeleteTourLog(tourLog);

            Assert.IsFalse(_viewModel.TourLogs.Contains(tourLog));
        }


        [Test]
        public void DeleteTourLog_ListCountShouldBeOneLowerThenBefore()
        {
            var tourLogToDelete = _viewModel.TourLogs.First();
            int initialCount = _viewModel.TourLogs.Count;

            _viewModel.DeleteTourLog(tourLogToDelete);

            Assert.IsFalse(_viewModel.TourLogs.Contains(tourLogToDelete));
            Assert.AreEqual(initialCount - 1, _viewModel.TourLogs.Count);
        }


        [Test]
        public void SelectTour_ShouldUpdateSelectedTour()
        {
           
            var tourToSelect = _viewModel.Tours.First();
            _viewModel.SelectedTour = tourToSelect;

            Assert.AreEqual(tourToSelect, _viewModel.SelectedTour);
        }

        [Test]
        public void UpdateTour_ShouldModifyTourDetails()
        {
            var tourToUpdate = _viewModel.Tours.First();
            var originalName = tourToUpdate.Name;
            var updatedName = "Updated Name";

            tourToUpdate.Name = updatedName;
            _viewModel.UpdateTour(tourToUpdate);

            Assert.AreNotEqual(originalName, _viewModel.Tours.First().Name);
            Assert.AreEqual(updatedName, _viewModel.Tours.First().Name);
        }

        [Test]
        public void UpdateTourLog_ExistingTourLog_ShouldInvokePropertyChanged()
        {
            var viewModel = new TourViewModel();
            var tourLog = new TourLog { Id = 4, Comment = "Initial Comment" };
            viewModel.TourLogs.Add(tourLog);

            bool propertyChangedInvoked = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TourViewModel.TourLogs))
                {
                    propertyChangedInvoked = true;
                }
            };

            var updatedTourLog = new TourLog { Id = 4, Comment = "Updated Comment" };

            viewModel.UpdateTourLog(updatedTourLog);

            Assert.IsTrue(propertyChangedInvoked, "Property changed event should be invoked for TourLogs.");
            var actualTourLog = viewModel.TourLogs.First(log => log.Id == 4);
            Assert.AreEqual("Updated Comment", actualTourLog.Comment, "Tour log should be updated with new comment.");
            
        }

        
    }
}