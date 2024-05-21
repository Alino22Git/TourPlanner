using System.Windows;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;

            if (mainViewModel.InitializeWebViewCommand.CanExecute(webView))
            {
                mainViewModel.InitializeWebViewCommand.Execute(webView);
                webView.Visibility = Visibility.Collapsed;
            }
        }
    }
}