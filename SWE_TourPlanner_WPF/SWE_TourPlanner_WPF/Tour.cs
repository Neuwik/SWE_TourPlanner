using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SWE_TourPlanner_WPF
{
    public class Tour
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ETransportType TransportType { get; set; }
        public float Distance { get; set; }
        public float Time { get; set; }
        public string RouteInformation { get; set; }
        public List<TourLog> TourLogs { get; set; } = new List<TourLog>();


        public Tour() { }

        public Tour(string name, string description, string from, string to, ETransportType transportType, float distance, float time, string routeInformation, List<TourLog> tourLogs)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            Time = time;
            RouteInformation = routeInformation;
            TourLogs = tourLogs;
        }

        public Tour(Tour other)
        {
            Name = other.Name;
            Description = other.Description;
            From = other.From;
            To = other.To;
            TransportType = other.TransportType;
            Distance = other.Distance;
            Time = other.Time;
            RouteInformation = other.RouteInformation;
            foreach (TourLog tourLog in other.TourLogs)
            {
                TourLogs.Add(new TourLog(tourLog));
            }
        }
    }
}
