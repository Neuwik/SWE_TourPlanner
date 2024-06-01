﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

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
                return ToStringHelpers.DistanceInMetersToString(Distance);
            }
        }
        public string TimeString
        {
            get
            {
                return ToStringHelpers.DurationInSecondsToString(Time);
            }
        }
        public EDifficulty ChildFriendliness
        {
            get
            {
                double avgTourLogDifficulty = (int)EDifficulty.Medium;
                double avgTourLogDistance = Distance;
                double avgTourLogTime = Time;
                if (TourLogs != null && TourLogs.Count > 0)
                {
                    avgTourLogDifficulty = TourLogs.Sum<TourLog>(l => (int)l.Difficulty) / TourLogs.Count;
                    avgTourLogDistance = TourLogs.Sum<TourLog>(l => l.TotalDistance) / TourLogs.Count;
                    avgTourLogTime = TourLogs.Sum<TourLog>(l => l.TotalTime) / TourLogs.Count;
                }

                double typeMod = ((int)TransportType + 1)/  (int)ETransportType.Foot;

                // Every Step = 1 Points
                double pointsDifficulty = avgTourLogDifficulty+1;

                // Every 10km = 1 Point
                double pointsDistance = avgTourLogDistance * typeMod / 1000 / 10;

                // Every 2h = 1 Point
                double pointsTime = avgTourLogTime * typeMod / 3600 / 2;

                int intChildFriendliness = (int)((pointsDifficulty * 0.5 + pointsDistance * 0.25 + pointsTime * 0.25));

                if (intChildFriendliness < (int)EDifficulty.VeryEasy)
                    return EDifficulty.VeryEasy;
                if (intChildFriendliness > (int)EDifficulty.VeryHard)
                    return EDifficulty.VeryHard;

                return (EDifficulty)intChildFriendliness;
            }
        }
        public int Popularity
        {
            get
            {
                return TourLogs != null ? TourLogs.Count : 0;
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
            return $"Tour Id: {Id}, Name: {Name}, Description: {Description}, From: {From}, To: {To}, Transport Type: {TransportType}, Distance: {DistanceString}, Time: {TimeString}, Tour Logs Count: {TourLogs.Count}, Popularity: {Popularity}, Child-Friendliness: {ChildFriendliness}";
        }
    }
}
