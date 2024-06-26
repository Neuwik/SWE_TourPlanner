﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Documents;
using System.Windows.Input;

namespace SWE_TourPlanner_WPF.Models
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

        public ICollection<TourLog> TourLogs { get; set; } = new List<TourLog>();

        [JsonIgnore]
        public string DistanceString
        {
            get
            {
                return ToStringHelpers.DistanceInMetersToString(Distance);
            }
        }
        [JsonIgnore]
        public string TimeString
        {
            get
            {
                return ToStringHelpers.DurationInSecondsToString(Time);
            }
        }
        [JsonIgnore]
        public EDifficulty ChildFriendliness
        {
            get
            {
                double typeMod = ((int)TransportType + 1) / (int)ETransportType.Foot;

                // Every Step = 1 Points
                double pointsDifficulty = AvgTourLogDiffiuly + 1;

                // Every 10km = 1 Point
                double pointsDistance = AvgTourLogTotalDistance * typeMod / 1000 / 10;

                // Every 2h = 1 Point
                double pointsTime = AvgTourLogTotalTime * typeMod / 3600 / 2;

                int intChildFriendliness = (int)(pointsDifficulty * 0.5 + pointsDistance * 0.25 + pointsTime * 0.25);

                if (intChildFriendliness < (int)EDifficulty.VeryEasy)
                    return EDifficulty.VeryEasy;
                if (intChildFriendliness > (int)EDifficulty.VeryHard)
                    return EDifficulty.VeryHard;

                return (EDifficulty)intChildFriendliness;
            }
        }
        [JsonIgnore]
        public int Popularity
        {
            get
            {
                return TourLogs != null ? TourLogs.Count : 0;
            }
        }
        [JsonIgnore]
        public double AvgTourLogDiffiuly
        {
            get
            {
                double avgTourLogDifficulty = (int)EDifficulty.Medium;
                if (TourLogs != null && TourLogs.Count > 0)
                {
                    avgTourLogDifficulty = TourLogs.Sum(l => (int)l.Difficulty) / TourLogs.Count;
                }
                return avgTourLogDifficulty;
            }
        }
        [JsonIgnore]
        public double AvgTourLogTotalDistance
        {
            get
            {
                double avgTourLogDistance = Distance;
                if (TourLogs != null && TourLogs.Count > 0)
                {
                    avgTourLogDistance = TourLogs.Sum(l => l.TotalDistance) / TourLogs.Count;
                }
                return avgTourLogDistance;
            }
        }
        [JsonIgnore]
        public double AvgTourLogTotalTime
        {
            get
            {
                double avgTourLogTime = Time;
                if (TourLogs != null && TourLogs.Count > 0)
                {
                    avgTourLogTime = TourLogs.Sum(l => l.TotalTime) / TourLogs.Count;
                }
                return avgTourLogTime;
            }
        }
        [JsonIgnore]
        public double AvgTourLogRating
        {
            get
            {
                double avgTourLogRating = (int)ERating.ZeroStars;
                if (TourLogs != null && TourLogs.Count > 0)
                {
                    avgTourLogRating = TourLogs.Sum(l => (int)l.Rating) / TourLogs.Count;
                }
                return avgTourLogRating;
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
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Description))
            {
                return false;
            }
            if (string.IsNullOrEmpty(From))
            {
                return false;
            }
            if (string.IsNullOrEmpty(To))
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

        public bool ContainsFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return true;
            }
            return ToString().Contains(filter);
        }
    }
}
