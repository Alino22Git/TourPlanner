using System.Windows;
using Microsoft.Win32;
using TourPlanner.Views;
using TourPlanner.Viewmodels;
using Models;

namespace TourPlanner.ViewModels
{
    public class DialogManager
    {
        public string? ShowSaveFileDialog(string filter, string defaultExt, string defaultFileName)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = defaultFileName
            };

            bool? result = dialog.ShowDialog();
            return result == true ? dialog.FileName : null;
        }

        public string? ShowOpenFileDialog(string filter, string defaultExt)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt
            };

            bool? result = dialog.ShowDialog();
            return result == true ? dialog.FileName : null;
        }

        public void ShowAddTourWindow(TourViewModel tourViewModel, Tour? originalTour = null)
        {
            var addTourWindow = new AddTourWindow(tourViewModel, new Tour());
            addTourWindow.ShowDialog();
        }

        public void ShowEditTourWindow(TourViewModel tourViewModel, Tour? originalTour)
        {
            var addTourWindow = new AddTourWindow(tourViewModel, originalTour);
            addTourWindow.ShowDialog();
        }

        public void ShowAddTourLogWindow(TourLogViewModel tourLogViewModel)
        {
            var addTourLogWindow = new AddTourLogWindow(tourLogViewModel);
            addTourLogWindow.ShowDialog();
        }
    }
}
