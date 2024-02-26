using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string GreetingName { get; set; }
        private string greetingMessage;
        public string GreetingMessage
        {
            get => greetingMessage;
            set
            {
                greetingMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GreetingName)));
            }
        }
            public event PropertyChangedEventHandler? PropertyChanged;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Bye");
            System.Environment.Exit(0);
        }
        private void Greet_Button(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"GreetingName is {GreetingName}");
            GreetingMessage = $"Hello GreetingName {GreetingName}";
            Debug.WriteLine($"Greeting Message is {GreetingName}");
        }
    }
}
