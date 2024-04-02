using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TourPlanner.Models;

public class Tour : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<TourLog> tourLogs;
    public ObservableCollection<TourLog> TourLogs
    {
        get => tourLogs;
        set
        {
            if (tourLogs != value)
            {
                tourLogs = value;
                OnPropertyChanged(nameof(TourLogs));
            }
        }
    }

    public Tour()
    {
        TourLogs = new ObservableCollection<TourLog>();
    }

    private int id;
    public int Id
    {
        get { return id; }
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    private string? name;
    public string? Name
    {
        get { return name; }
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Distance { get; set; }
    public string? Time { get; set; }
    public string? Description { get; set; }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}