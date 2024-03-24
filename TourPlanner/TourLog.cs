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
            Tours.Add(new Tour { Name = "Tour 1", Location = "Location 1", Distance = "10 km", Description = "Description 1" });
            Tours.Add(new Tour { Name = "Tour 2", Location = "Location 2", Distance = "15 km", Description = "Description 2" });
            Tours.Add(new Tour { Name = "Tour 3", Location = "Location 3", Distance = "20 km", Description = "Description 3" });
        }
    }
}