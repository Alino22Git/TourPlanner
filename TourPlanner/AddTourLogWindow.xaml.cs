using System;
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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Erstellen Sie eine neue Tour-Log basierend auf den Eingaben im Fenster
            TourLog newTourLog = new TourLog
            {
                Date = DateDatePicker.SelectedDate,
                Comment = CommentTextBox.Text,
                Difficulty = DifficultyComboBox.SelectedItem?.ToString(),
                TotalDistance = TotalDistanceSlider.Value,
                TotalTime = TotalTimeSlider.Value,
                Rating = selectedDifficulty,
                Weather = GetSelectedWeather()
            };

            // Fügen Sie das neue Tour-Log zum ViewModel hinzu
            viewModel.AddTourLog(newTourLog);

            // Fenster schließen
            Close();
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