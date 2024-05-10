using System.Windows;
using TourPlanner.ViewModels;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.Views
{
    public partial class AddTourWindow : Window
    {
        public AddTourWindow(TourViewModel viewModel, Tour originalTour)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.StartEditing(originalTour);
        }
    }
}