using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TourPlannerBusinessLayer.Models;

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
    private string? from;
    public string? From
    {
        get => from;
        set
        {
            if (from != value)
            {
                from = value;
                OnPropertyChanged(nameof(From));
            }
        }
    }

    private string? to;

    public string? To
    {
        get => to;
        set
        {
            if (to != value)
            {
                to = value;
                OnPropertyChanged(nameof(To));
            }
        }
    }
    
    private string? distance;

    public string? Distance
    {
        get => distance;
        set
        {
            if (distance != value)
            {
                distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }
    }

    private string time;

    public string? Time
    {
        get => time;
        set
        {
            if (time != value)
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }

    private string description;

    public string? Description
    {
        get => description;
        set
        {
            if (description != value)
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
   
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}