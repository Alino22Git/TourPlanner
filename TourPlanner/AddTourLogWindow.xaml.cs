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
        private string? selectedRating;
        private TourLog? selectedTourLog;

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

        public AddTourLogWindow(TourViewModel viewModel, TourLog selectedTourLog) : this(viewModel)
        {
            // Setzen Sie die DataContext-Eigenschaft auf das ausgewählte Tour-Log, um die vorhandenen Daten anzuzeigen
            DataContext = this.selectedTourLog = selectedTourLog;

            // Laden Sie die Daten des ausgewählten Tour-Logs in die Steuerelemente des Fensters
            LoadSelectedTourLogData();
        }

        private void LoadSelectedTourLogData()
        {
            // Laden Sie die Daten des ausgewählten Tour-Logs in die Steuerelemente
            DateDatePicker.SelectedDate = selectedTourLog?.Date;
            CommentTextBox.Text = selectedTourLog?.Comment;
            TotalDistanceSlider.Value = selectedTourLog?.TotalDistance ?? 0;
            TotalTimeSlider.Value = selectedTourLog?.TotalTime ?? 0;

            // ComboBox für den Schwierigkeitsgrad initialisieren
            if (selectedTourLog != null && !string.IsNullOrEmpty(selectedTourLog.Difficulty))
            {
                // Suchen Sie nach dem entsprechenden Schwierigkeitsgrad in der ComboBox
                foreach (var item in DifficultyComboBox.Items)
                {
                    if (item.ToString() == selectedTourLog.Difficulty)
                    {
                        DifficultyComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            // Initialisieren des Ratings
            if (selectedTourLog != null && !string.IsNullOrEmpty(selectedTourLog.Rating))
            {
                // Suchen Sie nach dem entsprechenden Rating in der ComboBox
                foreach (var item in RatingComboBox.Items)
                {
                    if (item.ToString() == selectedTourLog.Rating)
                    {
                        RatingComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            // RadioButtons für das Wetter initialisieren
            switch (selectedTourLog?.Weather)
            {
                case "Sunny":
                    SunnyRadioButton.IsChecked = true;
                    break;
                case "Cloudy":
                    CloudyRadioButton.IsChecked = true;
                    break;
                case "Rainy":
                    RainyRadioButton.IsChecked = true;
                    break;
                case "Snowy":
                    SnowyRadioButton.IsChecked = true;
                    break;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen Sie, ob ein vorhandenes Tour-Log bearbeitet wird oder ein neues erstellt wird
            if (selectedTourLog != null)
            {
                // Aktualisieren Sie die vorhandenen Tour-Log-Daten
                UpdateSelectedTourLog();
            }
            else
            {
                // Fügen Sie ein neues Tour-Log hinzu
                AddNewTourLog();
            }

            // Fenster schließen
            Close();
        }

        private void AddNewTourLog()
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
                Rating = DifficultyComboBox.SelectedItem?.ToString(),
                Weather = GetSelectedWeather(),
                SelectedTours = GetSelectedTours() // Fügen Sie die ausgewählten Touren hinzu

            };

            foreach (Tour tour in newTourLog.SelectedTours)
            {
                tour.TourLogs.Add(newTourLog);
            }

            // Fügen Sie das neue Tour-Log zum ViewModel hinzu
            viewModel.AddTourLog(newTourLog);
        }

        private void UpdateSelectedTourLog()
        {
            // Aktualisieren Sie die Daten des ausgewählten Tour-Logs
            if (selectedTourLog != null)
            {
                selectedTourLog.Date = DateDatePicker.SelectedDate;
                selectedTourLog.Comment = CommentTextBox.Text;
                selectedTourLog.Difficulty = DifficultyComboBox.SelectedItem?.ToString();
                selectedTourLog.TotalDistance = (int)Math.Round(TotalDistanceSlider.Value);
                selectedTourLog.TotalTime = (int)Math.Round(TotalTimeSlider.Value);
                selectedTourLog.Rating = RatingComboBox.SelectedItem?.ToString();
                selectedTourLog.Weather = GetSelectedWeather();
                selectedTourLog.SelectedTours = GetSelectedTours(); // Fügen Sie die ausgewählten Touren hinzu

                // Aktualisieren Sie das Tour-Log im ViewModel
                viewModel.UpdateTourLog(selectedTourLog);
            }
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

        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Überprüfen, ob eine Auswahl vorhanden ist
            if (RatingComboBox.SelectedItem != null)
            {
                // Typkonvertierung des ausgewählten Elements zu einem ComboBoxItem
                ComboBoxItem selectedItem = (ComboBoxItem)RatingComboBox.SelectedItem;

                // Zugriff auf den Inhalt des ausgewählten Elements (z. B. Bewertung)
                selectedRating = selectedItem.Content.ToString();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob ein Eintrag ausgewählt ist
            if (selectedTourLog != null)
            {
                // Entfernen Sie den ausgewählten Eintrag aus der Liste
                viewModel.TourLogs?.Remove(selectedTourLog);

                // Schließen Sie das Fenster
                Close();
            }
        }
    }
}
