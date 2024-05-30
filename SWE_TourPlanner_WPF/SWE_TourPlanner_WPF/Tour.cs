using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;

namespace SWE_TourPlanner_WPF
{
    [Table("Tours")]
    public class Tour
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ETransportType TransportType { get; set; }
        public int Distance { get; set; }
        public int Time { get; set; }
        public string RouteInformation { get; set; }
        public string ImagePath { get; set; }
        public string OSMjson { get; set; }
        public ICollection<TourLog> TourLogs { get; } = new List<TourLog>();
        public Tour() { }
        public Tour(string name, string description, string from, string to, ETransportType transportType, int distance, int time, string routeInformation, string imagePath, List<TourLog> tourLogs)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            Time = time;
            RouteInformation = routeInformation;
            ImagePath = imagePath;
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
            ImagePath = other.ImagePath;
            foreach (TourLog tourLog in other.TourLogs)
            {
                TourLogs.Add(new TourLog(tourLog));
            }
        }
        public void Update(Tour newTour)
        {
            Name = newTour.Name;
            Description = newTour.Description;
            From = newTour.From;
            To = newTour.To;
            TransportType = newTour.TransportType;
            Distance = newTour.Distance;
            Time = newTour.Time;
            RouteInformation = newTour.RouteInformation;
            ImagePath = newTour.ImagePath;
        }

        public bool AreAllParamsSet()
        {
            if (String.IsNullOrEmpty(Name))
            {
                return false;
            }
            if (String.IsNullOrEmpty(Description))
            {
                return false;
            }
            if (String.IsNullOrEmpty(From))
            {
                return false;
            }
            if (String.IsNullOrEmpty(To))
            {
                return false;
            }
            if (TransportType == null)
            {
                return false;
            }
            if (Distance < 0)
            {
                return false;
            }
            if (Time < 0)
            {
                return false;
            }
            if (String.IsNullOrEmpty(RouteInformation))
            {
                return false;
            }

            return true;
        }
    }
}
