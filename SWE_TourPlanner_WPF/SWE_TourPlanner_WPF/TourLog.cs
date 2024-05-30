using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SWE_TourPlanner_WPF
{
    public class TourLog
    {
        [Key]
        public int Id { get; set; }
        public int TourId { get; set; }
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
    }
}
