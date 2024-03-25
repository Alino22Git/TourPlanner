using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner
{
    public class TourLog
    {
        public ObservableCollection<Tour> Tours { get; set; }

        // Konstruktor
        public TourLog()
        {
            // Initialisiere die Liste der Touren
            Tours = new ObservableCollection<Tour>();

            // Füge einige Beispiel-Touren hinzu (kann optional sein)
            Tours.Add(new Tour { Name = "Tour 1", From = "Location 1",To = "Location 1", Distance = "10 km", Time = "00",Description = "Description 1" });
            Tours.Add(new Tour { Name = "Tour 2", From = "Location 2", To = "Location 1" , Distance = "15 km", Time = "00", Description = "Description 2" });
            Tours.Add(new Tour { Name = "Tour 3", From = "Location 3", To = "Location 1", Distance = "20 km", Time = "00", Description = "Description 3" });
        }
    }
}