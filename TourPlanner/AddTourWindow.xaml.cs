using System.Windows;

namespace TourPlanner
{
    public partial class AddTourWindow : Window
    {
        private readonly TourViewModel viewModel;
        private Tour? selectedTour;

        // Konstruktor für das Hinzufügen einer neuen Tour
        public AddTourWindow(TourViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            selectedTour = new Tour();
            DataContext = selectedTour; // Datenkontext auf die neue Tour setzen
        }

        // Konstruktor für das Bearbeiten einer vorhandenen Tour
        public AddTourWindow(TourViewModel viewModel, Tour? selectedTour) : this(viewModel)
        {
            this.selectedTour = selectedTour;

           
            LoadSelectedTourData();
        }

        private void LoadSelectedTourData()
        {
            
            NameTextBox.Text = selectedTour?.Name;
            FromTextBox.Text = selectedTour?.From;
            ToTextBox.Text = selectedTour?.To;
            DistanceTextBox.Text = selectedTour?.Distance;
            TimeTextBox.Text = selectedTour?.Time;
            DescriptionTextBox.Text = selectedTour?.Description;
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (selectedTour != null)
            {
                selectedTour.Name = NameTextBox.Text;
                selectedTour.From = FromTextBox.Text;
                selectedTour.To = ToTextBox.Text;
                selectedTour.Distance = DistanceTextBox.Text;
                selectedTour.Time = TimeTextBox.Text;
                selectedTour.Description = DescriptionTextBox.Text;

                
                if (viewModel.Tours != null && !viewModel.Tours.Contains(selectedTour))
                    viewModel.AddTour(selectedTour);
                else
                    viewModel.UpdateTour(selectedTour);
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (selectedTour != null)
            {
                
                viewModel.Tours?.Remove(selectedTour);

               
                Close();
            }
        }
    }
}
