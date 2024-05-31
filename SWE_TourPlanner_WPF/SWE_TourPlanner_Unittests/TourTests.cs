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
        }
    }
}
