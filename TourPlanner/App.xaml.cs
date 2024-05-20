using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlannerDAL;
using TourPlannerBusinessLayer.Services;
using System.Windows.Controls;
using TourPlanner.Viewmodels;

namespace TourPlanner
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TourPlannerDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=root;Password=tourplanner"), ServiceLifetime.Scoped);

            services.AddScoped<TourRepository>();
            services.AddScoped<TourLogRepository>();
            services.AddScoped<TourService>();
            services.AddScoped<TourLogService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<TourViewModel>();
            services.AddTransient<TourLogViewModel>();

            services.AddTransient<MainWindow>(provider =>
            {
                var mainViewModel = provider.GetRequiredService<MainViewModel>();
                return new MainWindow(mainViewModel);
            });

            services.AddTransient<MainViewModel>(provider =>
            {
                var contentControl = new ContentControl();
                var tourService = provider.GetRequiredService<TourService>();
                var tourLogService = provider.GetRequiredService<TourLogService>();
                return new MainViewModel(contentControl, tourService, tourLogService);
            });
        }
    }
}
