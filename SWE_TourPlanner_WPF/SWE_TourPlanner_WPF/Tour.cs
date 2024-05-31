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
using System.Windows;
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
        public double Distance { get; set; }
        public double Time { get; set; }
        public string RouteInformation { get; set; }
        public string OSMjson { get; set; }
        public ICollection<TourLog> TourLogs { get; } = new List<TourLog>();
        public string DistanceString
        {
            get
            {
                string distance = $"{Math.Truncate((Distance * 10)) / 10}m";
                if (Distance > 1000)
                {
                    distance = $"{Math.Truncate((Distance / 1000 * 10)) / 10}km";
                }
                return distance;
            }
        }
        public string TimeString
        {
            get
            {
                TimeSpan t = TimeSpan.FromSeconds(Time);

                return string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds
                                );
            }
        }


        public Tour() { }

        public Tour(string name, string description, string from, string to, ETransportType transportType, int distance, int time, string routeInformation, string osmjson, List<TourLog> tourLogs)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            Time = time;
            RouteInformation = routeInformation;
            OSMjson = osmjson;
            TourLogs = tourLogs;
        }

        public Tour(Tour other)
        {
            Id = other.Id;
            Name = other.Name;
            Description = other.Description;
            From = other.From;
            To = other.To;
            TransportType = other.TransportType;
            Distance = other.Distance;
            Time = other.Time;
            RouteInformation = other.RouteInformation;
            OSMjson = other.OSMjson;
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
            OSMjson = newTour.OSMjson;
        }

        public bool AreAllInputParamsSet()
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

            return true;
        }

        public override string ToString()
        {
            return $"Tour Id: {Id}, Name: {Name}, Description: {Description}, From: {From}, To: {To}, Transport Type: {TransportType}, Distance: {DistanceString}, Time: {TimeString}, Tour Logs Count: {TourLogs.Count}";
        }
    }
}
