using NUnit.Framework.Legacy;
using SWE_TourPlanner_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class TourLogTests
    {
        [Test]
        public void Constructor_WithParameters_SetsPropertiesCorrectly()
        {
            DateTime dateTime = DateTime.Now;
            string comment = "Test comment";
            EDifficulty difficulty = EDifficulty.medium;
            float totalDistance = 10.5f;
            float totalTime = 2.5f;
            ERating rating = ERating.FiveStars;

            var tourLog = new TourLog(dateTime, comment, difficulty, totalDistance, totalTime, rating);

            ClassicAssert.AreEqual(dateTime, tourLog.DateTime);
            ClassicAssert.AreEqual(comment, tourLog.Comment);
            ClassicAssert.AreEqual(difficulty, tourLog.Difficulty);
            ClassicAssert.AreEqual(totalDistance, tourLog.TotalDistance);
            ClassicAssert.AreEqual(totalTime, tourLog.TotalTime);
            ClassicAssert.AreEqual(rating, tourLog.Rating);
        }
        [Test]
        public void DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var tourLog = new TourLog();

            // Assert
            ClassicAssert.AreEqual("---", tourLog.Comment);
            ClassicAssert.AreEqual(EDifficulty.medium, tourLog.Difficulty);
            ClassicAssert.AreEqual(0, tourLog.TotalDistance);
            ClassicAssert.AreEqual(0, tourLog.TotalTime);
            ClassicAssert.AreEqual(ERating.ThreeStars, tourLog.Rating);
            ClassicAssert.AreEqual(DateTime.Now.Date, tourLog.DateTime.Date);
        }

        [Test]
        public void CopyConstructor_CopiesPropertiesCorrectly()
        {

            var original = new TourLog
            {
                DateTime = DateTime.Now,
                Comment = "Original comment",
                Difficulty = EDifficulty.easy,
                TotalDistance = 15.7f,
                TotalTime = 3.0f,
                Rating = ERating.FourStars
            };

            var copy = new TourLog(original);

            ClassicAssert.AreEqual(original.DateTime, copy.DateTime);
            ClassicAssert.AreEqual(original.Comment, copy.Comment);
            ClassicAssert.AreEqual(original.Difficulty, copy.Difficulty);
            ClassicAssert.AreEqual(original.TotalDistance, copy.TotalDistance);
            ClassicAssert.AreEqual(original.TotalTime, copy.TotalTime);
            ClassicAssert.AreEqual(original.Rating, copy.Rating);
        }
        [Test]
        public void Update_UpdatesValues()
        {
            // Arrange
            var original = new TourLog
            {
                DateTime = new DateTime(2023, 5, 1, 10, 0, 0),
                Comment = "Original comment",
                Difficulty = EDifficulty.easy,
                TotalDistance = 12.3,
                TotalTime = 1800,
                Rating = ERating.FourStars
            };

            var updated = new TourLog
            {
                DateTime = new DateTime(2024, 6, 2, 15, 30, 0),
                Comment = "Updated comment",
                Difficulty = EDifficulty.hard,
                TotalDistance = 20.5,
                TotalTime = 3600,
                Rating = ERating.FiveStars
            };

            // Act
            original.Update(updated);

            // Assert
            
            ClassicAssert.AreEqual(updated.DateTime, original.DateTime);
            ClassicAssert.AreEqual(updated.Comment, original.Comment);
            ClassicAssert.AreEqual(updated.Difficulty, original.Difficulty);
            ClassicAssert.AreEqual(updated.TotalDistance, original.TotalDistance);
            ClassicAssert.AreEqual(updated.TotalTime, original.TotalTime);
            ClassicAssert.AreEqual(updated.Rating, original.Rating);
        }

        [Test]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var tourLog = new TourLog
            {
                Id = 1,
                TourId = 10,
                DateTime = new DateTime(2023, 5, 1, 10, 0, 0),
                Comment = "Test comment",
                Difficulty = EDifficulty.medium,
                TotalDistance = 15.5,
                TotalTime = 3600,
                Rating = ERating.ThreeStars
            };

            // Act
            var result = tourLog.ToString();

            // Assert
            var expected = "TourLog Id: 1, TourId: 10, DateTime: 5/1/2023 10:00:00 AM, Comment: Test comment, Difficulty: medium, TotalDistance: 15.5, TotalTime: 3600, Rating: ThreeStars";
            ClassicAssert.AreEqual(expected, result);
        }
    }
}
