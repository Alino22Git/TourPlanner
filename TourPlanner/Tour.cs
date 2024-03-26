using System.ComponentModel;

namespace TourPlanner;

public class Tour : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

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