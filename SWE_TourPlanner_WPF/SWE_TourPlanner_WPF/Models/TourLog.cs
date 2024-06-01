using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Windows.Documents;

namespace SWE_TourPlanner_WPF.Models
{
    [Table("TourLogs")]
    public class TourLog
    {
        [Key]
        public int Id { get; set; }
        public int TourId { get; set; }
        [JsonIgnore]
        public Tour Tour { get; set; } = null!;

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value.ToUniversalTime();
            }
        }
        public string Comment { get; set; }
        public EDifficulty Difficulty { get; set; }
        public double TotalDistance { get; set; }
        public double TotalTime { get; set; }
        public ERating Rating { get; set; }

        public TourLog()
        {
            DateTime = DateTime.Now;
            Comment = "---";
            Difficulty = EDifficulty.Medium;
            TotalDistance = 0;
            TotalTime = 0;
            Rating = ERating.ThreeStars;
        }

        public TourLog(DateTime dateTime, string comment, EDifficulty difficulty, double totalDistance, double totalTime, ERating rating)
        {
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totalDistance;
            TotalTime = totalTime;
            Rating = rating;
        }

        public TourLog(TourLog other)
        {
            Id = other.Id;
            TourId = other.TourId;
            DateTime = other.DateTime;
            Comment = other.Comment;
            Difficulty = other.Difficulty;
            TotalDistance = other.TotalDistance;
            TotalTime = other.TotalTime;
            Rating = other.Rating;
        }

        public override string ToString()
        {
            return $"TourLog Id: {Id}, TourId: {TourId}, DateTime: {DateTime}, Comment: {Comment}, Difficulty: {Difficulty}, TotalDistance: {TotalDistance}, TotalTime: {TotalTime}, Rating: {Rating}";
        }

        public void Update(TourLog other)
        {
            DateTime = other.DateTime;
            Comment = other.Comment;
            Difficulty = other.Difficulty;
            TotalDistance = other.TotalDistance;
            TotalTime = other.TotalTime;
            Rating = other.Rating;
        }
    }
}
