using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TourPlanner.ViewModels;
using TourPlannerBusinessLayer.Models;

namespace TourPlanner.Views
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
            DataContext = viewModel;
            ToursListBox.ItemsSource = viewModel.Tours;
            ToursListBox.DisplayMemberPath = "Name";
        }

        public AddTourLogWindow(TourViewModel viewModel, TourLog selectedTourLog) : this(viewModel)
        {
           
            DataContext = this.selectedTourLog = selectedTourLog;

            LoadSelectedTourLogData();
        }

        private void LoadSelectedTourLogData()
        {
            
            DateDatePicker.SelectedDate = selectedTourLog?.Date;
            CommentTextBox.Text = selectedTourLog?.Comment;
            TotalDistanceSlider.Value = selectedTourLog?.TotalDistance ?? 0;
            TotalTimeSlider.Value = selectedTourLog?.TotalTime ?? 0;

            // ComboBox für den Schwierigkeitsgrad initialisieren
            if (selectedTourLog != null && !string.IsNullOrEmpty(selectedTourLog.Difficulty))
            {
                
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (selectedTourLog != null)
            {
                
                UpdateSelectedTourLog();
            }
            else
            {
                
                AddNewTourLog();
            }

            
            Close();
        }

        private void AddNewTourLog()
        {
            
            int roundedTotalTime = (int)Math.Round(TotalTimeSlider.Value);
            int roundedTotalDistance = (int)Math.Round(TotalDistanceSlider.Value);

            
            TourLog newTourLog = new TourLog
            {
                Date = DateDatePicker.SelectedDate,
                Comment = CommentTextBox.Text,
                Difficulty = DifficultyComboBox.SelectedItem?.ToString(),
                TotalDistance = roundedTotalDistance,
                TotalTime = roundedTotalTime,
                Rating = DifficultyComboBox.SelectedItem?.ToString(),
                Weather = GetSelectedWeather(),
                SelectedTours = GetSelectedTours() 

            };

            foreach (Tour tour in newTourLog.SelectedTours)
            {
                tour.TourLogs.Add(newTourLog);
            }
        }

        private void UpdateSelectedTourLog()
        {
            
            if (selectedTourLog != null)
            {
                selectedTourLog.Date = DateDatePicker.SelectedDate;
                selectedTourLog.Comment = CommentTextBox.Text;
                selectedTourLog.Difficulty = DifficultyComboBox.SelectedItem?.ToString();
                selectedTourLog.TotalDistance = (int)Math.Round(TotalDistanceSlider.Value);
                selectedTourLog.TotalTime = (int)Math.Round(TotalTimeSlider.Value);
                selectedTourLog.Rating = RatingComboBox.SelectedItem?.ToString();
                selectedTourLog.Weather = GetSelectedWeather();
                selectedTourLog.SelectedTours = GetSelectedTours(); 

                
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
           
            if (DifficultyComboBox.SelectedItem != null)
            {
                
                ComboBoxItem selectedItem = (ComboBoxItem)DifficultyComboBox.SelectedItem;

              
                selectedDifficulty = selectedItem.Content.ToString();
            }
        }

        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (RatingComboBox.SelectedItem != null)
            {
                
                ComboBoxItem selectedItem = (ComboBoxItem)RatingComboBox.SelectedItem;

                selectedRating = selectedItem.Content.ToString();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (selectedTourLog != null)
            {
                
                viewModel.TourLogs?.Remove(selectedTourLog);
                Close();
            }
        }
    }
}
