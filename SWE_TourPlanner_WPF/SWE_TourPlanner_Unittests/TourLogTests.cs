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
    }
}
