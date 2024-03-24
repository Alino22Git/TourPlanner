using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace TourPlanner
{
    public partial class AddTourWindow : Window
    {
        public AddTourWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string location = LocationTextBox.Text;
            string distance = DistanceTextBox.Text;
            string description = DescriptionTextBox.Text;
        }
    }
}
