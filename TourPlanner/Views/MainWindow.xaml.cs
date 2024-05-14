using System.Windows;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(DynamicContentControl);
            DataContext = mainViewModel;
        }
    }
}