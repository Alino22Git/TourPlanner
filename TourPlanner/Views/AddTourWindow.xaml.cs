using System.Windows;
using Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    public partial class AddTourWindow : Window{
        public AddTourWindow(TourViewModel viewModel, Tour originalTour){
            InitializeComponent();
            DataContext = viewModel;
            viewModel.StartEditing(originalTour);
        }
    }
}