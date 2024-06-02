using System.Windows;
using TourPlanner.Viewmodels;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    public partial class AddTourLogWindow : Window
    {
        public AddTourLogWindow(TourLogViewModel viewModel){
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}