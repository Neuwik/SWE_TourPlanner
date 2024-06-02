using NUnit.Framework;
using SWE_TourPlanner_WPF.Models;
using System;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class TourLogTests
    {
        [Test]
        public void Test_TourLog_DefaultConstructor()
        {
            var tourLog = new TourLog();
            var dateTimeNow = DateTime.Now.ToUniversalTime();

            Assert.That(tourLog.Id, Is.EqualTo(0));
            Assert.That(tourLog.TourId, Is.EqualTo(0));
            Assert.That(tourLog.Comment, Is.EqualTo("---"));
            Assert.That(tourLog.Difficulty, Is.EqualTo(EDifficulty.Medium));
            Assert.That(tourLog.TotalDistance, Is.EqualTo(0));
            Assert.That(tourLog.TotalTime, Is.EqualTo(0));
            Assert.That(tourLog.Rating, Is.EqualTo(ERating.ThreeStars));
            Assert.That(tourLog.DateTime, Is.EqualTo(dateTimeNow).Within(1).Minutes);
        }

        [Test]
        public void Test_TourLog_ParameterizedConstructor()
        {
            var dateTime = new DateTime(2023, 1, 1);
            var comment = "Great tour!";
            var difficulty = EDifficulty.Hard;
            var totalDistance = 10.5;
            var totalTime = 5.25;
            var rating = ERating.FiveStars;

            var tourLog = new TourLog(dateTime, comment, difficulty, totalDistance, totalTime, rating);

            Assert.That(tourLog.Id, Is.EqualTo(0));
            Assert.That(tourLog.TourId, Is.EqualTo(0));
            Assert.That(tourLog.DateTime, Is.EqualTo(dateTime.ToUniversalTime()));
            Assert.That(tourLog.Comment, Is.EqualTo(comment));
            Assert.That(tourLog.Difficulty, Is.EqualTo(difficulty));
            Assert.That(tourLog.TotalDistance, Is.EqualTo(totalDistance));
            Assert.That(tourLog.TotalTime, Is.EqualTo(totalTime));
            Assert.That(tourLog.Rating, Is.EqualTo(rating));
        }

        [Test]
        public void Test_TourLog_CopyConstructor()
        {
            var original = new TourLog
            {
                Id = 1,
                TourId = 2,
                DateTime = new DateTime(2023, 1, 1),
                Comment = "Amazing!",
                Difficulty = EDifficulty.Easy,
                TotalDistance = 15.0,
                TotalTime = 3.0,
                Rating = ERating.FourStars
            };

            var copy = new TourLog(original);

            Assert.That(copy.Id, Is.EqualTo(original.Id));
            Assert.That(copy.TourId, Is.EqualTo(original.TourId));
            Assert.That(copy.DateTime, Is.EqualTo(original.DateTime));
            Assert.That(copy.Comment, Is.EqualTo(original.Comment));
            Assert.That(copy.Difficulty, Is.EqualTo(original.Difficulty));
            Assert.That(copy.TotalDistance, Is.EqualTo(original.TotalDistance));
            Assert.That(copy.TotalTime, Is.EqualTo(original.TotalTime));
            Assert.That(copy.Rating, Is.EqualTo(original.Rating));
        }

        [Test]
        public void Test_TourLog_Update()
        {
            var tourLog = new TourLog
            {
                Id = 1,
                TourId = 2,
                DateTime = new DateTime(2023, 1, 1),
                Comment = "Initial comment",
                Difficulty = EDifficulty.Easy,
                TotalDistance = 10.0,
                TotalTime = 2.0,
                Rating = ERating.TwoStars
            };

            var updatedLog = new TourLog
            {
                DateTime = new DateTime(2024, 1, 1),
                Comment = "Updated comment",
                Difficulty = EDifficulty.Hard,
                TotalDistance = 20.0,
                TotalTime = 4.0,
                Rating = ERating.FiveStars
            };

            tourLog.Update(updatedLog);

            Assert.That(tourLog.DateTime, Is.EqualTo(updatedLog.DateTime));
            Assert.That(tourLog.Comment, Is.EqualTo(updatedLog.Comment));
            Assert.That(tourLog.Difficulty, Is.EqualTo(updatedLog.Difficulty));
            Assert.That(tourLog.TotalDistance, Is.EqualTo(updatedLog.TotalDistance));
            Assert.That(tourLog.TotalTime, Is.EqualTo(updatedLog.TotalTime));
            Assert.That(tourLog.Rating, Is.EqualTo(updatedLog.Rating));
        }

        [Test]
        public void Test_TourLog_DateTimeSetterUTC()
        {
            var tourLog = new TourLog();
            var localTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Local);
            var expectedUniversalTime = localTime.ToUniversalTime();

            tourLog.DateTime = localTime;

            Assert.That(tourLog.DateTime, Is.EqualTo(expectedUniversalTime));
        }
    }
}