using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlannerDAL;
using System.Windows.Controls;
using TourPlanner.Viewmodels;
using TourPlannerBusinessLayer.Managers;
using TourPlannerBusinessLayer.Service;
using TourPlannerConfig;

namespace TourPlanner
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

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
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            services.AddDbContext<TourPlannerDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            services.AddHttpClient<GeocodeService>();
            services.AddHttpClient<DirectionService>();

            services.AddScoped<TourRepository>();
            services.AddScoped<TourLogRepository>();
            services.AddScoped<TourService>();
            services.AddScoped<TourLogService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<TourViewModel>();
            services.AddTransient<TourLogViewModel>();
            services.AddTransient<RouteDataManager>();
            services.AddTransient<ReportManager>();
            services.AddTransient<FileTransferManager>();
            services.AddTransient<MainWindow>(provider => {
                var mainViewModel = provider.GetRequiredService<MainViewModel>();
                return new MainWindow(mainViewModel);
            });

            services.AddTransient<MainViewModel>(provider => {
                var contentControl = new ContentControl();
                var tourService = provider.GetRequiredService<TourService>();
                var tourLogService = provider.GetRequiredService<TourLogService>();
                var geocodeService = provider.GetRequiredService<GeocodeService>();
                var directionService = provider.GetRequiredService<DirectionService>();
                var routeDataManager = provider.GetRequiredService<RouteDataManager>();
                var reportManager = provider.GetRequiredService<ReportManager>();
                var fileTransferManager = provider.GetRequiredService<FileTransferManager>();
                return new MainViewModel(contentControl, tourService, tourLogService, routeDataManager, reportManager, fileTransferManager);
            });
        }
    }
}
