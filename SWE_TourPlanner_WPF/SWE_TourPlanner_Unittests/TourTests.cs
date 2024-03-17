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
    public class TourTests
    {
        [Test]
        public void CopyConstructor_CopiesPropertiesCorrectly()
        {
            var original = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Origin",
                To = "Destination",
                TransportType = ETransportType.Car,
                Distance = 100,
                Time = 120,
                RouteInformation = "Test Route Info",
                TourLogs = new List<TourLog>
            {
                new TourLog
                {
                    DateTime = System.DateTime.Now,
                    Comment = "Test Log",
                    Difficulty = EDifficulty.easy,
                    TotalDistance = 50,
                    TotalTime = 60,
                    Rating = ERating.FiveStars
                }
            }
            };

            var copy = new Tour(original);

            ClassicAssert.AreEqual(original.Name, copy.Name);
            ClassicAssert.AreEqual(original.Description, copy.Description);
            ClassicAssert.AreEqual(original.From, copy.From);
            ClassicAssert.AreEqual(original.To, copy.To);
            ClassicAssert.AreEqual(original.TransportType, copy.TransportType);
            ClassicAssert.AreEqual(original.Distance, copy.Distance);
            ClassicAssert.AreEqual(original.Time, copy.Time);
            ClassicAssert.AreEqual(original.RouteInformation, copy.RouteInformation);
            ClassicAssert.AreEqual(original.TourLogs.Count, copy.TourLogs.Count);
            ClassicAssert.AreEqual(original.TourLogs[0].DateTime, copy.TourLogs[0].DateTime);
            ClassicAssert.AreEqual(original.TourLogs[0].Comment, copy.TourLogs[0].Comment);
            ClassicAssert.AreEqual(original.TourLogs[0].Difficulty, copy.TourLogs[0].Difficulty);
            ClassicAssert.AreEqual(original.TourLogs[0].TotalDistance, copy.TourLogs[0].TotalDistance);
            ClassicAssert.AreEqual(original.TourLogs[0].TotalTime, copy.TourLogs[0].TotalTime);
            ClassicAssert.AreEqual(original.TourLogs[0].Rating, copy.TourLogs[0].Rating);
        }
    }
}
