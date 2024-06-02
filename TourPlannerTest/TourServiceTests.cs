using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using TourPlannerDAL;
using TourPlannerBusinessLayer.Service;

namespace TourPlannerTest
{
    [TestFixture]
    public class TourServiceTests
    {
        private ServiceProvider _serviceProvider;
        private TourPlannerDbContext _dbContext;
        private TourService _tourService;

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TourPlannerDbContext>(options =>
                options.UseInMemoryDatabase("TourPlannerDb"));

            serviceCollection.AddScoped<TourRepository>();
            serviceCollection.AddScoped<TourService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<TourPlannerDbContext>();
            _tourService = _serviceProvider.GetRequiredService<TourService>();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _serviceProvider.Dispose();
        }

        [Test]
        public async Task AddTourAsync_ShouldAddTour()
        {
            //Arrange
            var newTour = new Tour { Name = "New Tour", From = "A", To = "B", TransportType = "Car" };

            await _tourService.AddTourAsync(newTour);

            //Act
            var tours = await _tourService.GetToursAsync();

            //Assert
            Assert.AreEqual(1, tours.Count);
            Assert.AreEqual("New Tour", tours[0].Name);
        }

        [Test]
        public async Task GetToursAsync_ShouldReturnAllTours()
        {
            //Arrange
            var tour1 = new Tour { Name = "Tour 1", From = "A", To = "B", TransportType = "Car" };
            var tour2 = new Tour { Name = "Tour 2", From = "C", To = "D", TransportType = "Bicycle" };


            //Act
            await _tourService.AddTourAsync(tour1);
            await _tourService.AddTourAsync(tour2);
            var tours = await _tourService.GetToursAsync();

            //Assert
            Assert.That(tours.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateTourAsync_ShouldUpdateTour()
        {
            //Arrange
            var tour = new Tour { Name = "Tour", From = "A", To = "B", TransportType = "Car" };
            await _tourService.AddTourAsync(tour);

            //Act
            tour.Name = "Updated Tour";
            await _tourService.UpdateTourAsync(tour);
            var tours = await _tourService.GetToursAsync();

            //Assert
            Assert.That(tours[0].Name, Is.EqualTo("Updated Tour"));
        }

        [Test]
        public async Task DeleteTourAsync_ShouldRemoveTour()
        {
            //Arrange
            var tour = new Tour { Name = "Tour", From = "A", To = "B", TransportType = "Car" };
            await _tourService.AddTourAsync(tour);


            //Act
            await _tourService.DeleteTourAsync(tour);
            var tours = await _tourService.GetToursAsync();

            //Assert
            Assert.That(tours.Count, Is.EqualTo(0));
        }
    }
}
