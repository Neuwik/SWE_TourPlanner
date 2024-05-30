using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;

namespace SWE_TourPlanner_WPF
{
    [Table("TourLogs")]
    public class TourLog
    {
        [Key]
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
        public EDifficulty Difficulty { get; set; }
        public float TotalDistance { get; set; }
        public float TotalTime { get; set; }
        public ERating Rating { get; set; }

        public TourLog()
        {
            DateTime = DateTime.Now;
            Comment = "---";
            Difficulty = EDifficulty.medium;
            TotalDistance = 0;
            TotalTime = 0;
            Rating = ERating.ThreeStars;
        }

        public TourLog(DateTime dateTime, string comment, EDifficulty difficulty, float totalDistance, float totalTime, ERating rating)
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
            DateTime = other.DateTime;
            Comment = other.Comment;
            Difficulty = other.Difficulty;
            TotalDistance = other.TotalDistance;
            TotalTime = other.TotalTime;
            Rating = other.Rating;
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
