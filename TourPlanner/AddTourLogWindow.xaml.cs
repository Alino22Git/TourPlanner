using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TourPlanner
{
    public partial class AddTourLogWindow : Window
    {
        private readonly TourViewModel viewModel;
        private string? selectedDifficulty;

        public AddTourLogWindow(TourViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;

            // Setzen Sie den DataContext auf das ViewModel, damit die ListBox korrekt gebunden wird
            DataContext = viewModel;

            // Populieren Sie die ListBox mit vorhandenen Touren
            ToursListBox.ItemsSource = viewModel.Tours;
            ToursListBox.DisplayMemberPath = "Name";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Runden Sie die Dezimalwerte für TotalTime und TotalDistance auf die nächste ganze Zahl
            int roundedTotalTime = (int)Math.Round(TotalTimeSlider.Value);
            int roundedTotalDistance = (int)Math.Round(TotalDistanceSlider.Value);

            // Erstellen Sie eine neue Tour-Log basierend auf den Eingaben im Fenster
            TourLog newTourLog = new TourLog
            {
                Date = DateDatePicker.SelectedDate,
                Comment = CommentTextBox.Text,
                Difficulty = DifficultyComboBox.SelectedItem?.ToString(),
                TotalDistance = roundedTotalDistance,
                TotalTime = roundedTotalTime,
                Rating = selectedDifficulty,
                Weather = GetSelectedWeather(),
                SelectedTours = GetSelectedTours() // Fügen Sie die ausgewählten Touren hinzu
            };

            // Fügen Sie das neue Tour-Log zum ViewModel hinzu
            viewModel.AddTourLog(newTourLog);

            // Fenster schließen
            Close();
        }

        private List<Tour> GetSelectedTours()
        {
            List<Tour> selectedTours = new List<Tour>();

            foreach (Tour tour in ToursListBox.SelectedItems)
            {
                selectedTours.Add(tour);
            }

            return selectedTours;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fenster schließen
            Close();
        }

        private string? GetSelectedWeather()
        {
            if (SunnyRadioButton.IsChecked == true)
                return SunnyRadioButton.Content.ToString();
            if (CloudyRadioButton.IsChecked == true)
                return CloudyRadioButton.Content.ToString();
            if (RainyRadioButton.IsChecked == true)
                return RainyRadioButton.Content.ToString();
            if (SnowyRadioButton.IsChecked == true)
                return SnowyRadioButton.Content.ToString();

            return string.Empty;
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Überprüfen, ob eine Auswahl vorhanden ist
            if (DifficultyComboBox.SelectedItem != null)
            {
                // Typkonvertierung des ausgewählten Elements zu einem ComboBoxItem
                ComboBoxItem selectedItem = (ComboBoxItem)DifficultyComboBox.SelectedItem;

                // Zugriff auf den Inhalt des ausgewählten Elements (z. B. Schwierigkeitsgrad)
                selectedDifficulty = selectedItem.Content.ToString();
            }
        }
    }
}
