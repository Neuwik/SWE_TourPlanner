using NUnit.Framework.Legacy;
using SWE_TourPlanner_WPF;
using SWE_TourPlanner_WPF.Models;
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
        [Test]
        public void DistanceString_ReturnsCorrectFormat()
        {
            // Arrange
            var tour = new Tour { Distance = 1500 };

            // Act
            var result = tour.DistanceString;

            // Assert
            ClassicAssert.AreEqual("1.5km", result);
        }

        [Test]
        public void DistanceString_ReturnsCorrectFormatForMeters()
        {
            // Arrange
            var tour = new Tour { Distance = 500 };

            // Act
            var result = tour.DistanceString;

            // Assert
            ClassicAssert.AreEqual("500m", result);
        }

        [Test]
        public void TimeString_ReturnsCorrectFormat()
        {
            // Arrange
            var tour = new Tour { Time = 3661 };

            // Act
            var result = tour.TimeString;

            // Assert
            ClassicAssert.AreEqual("01h:01m:01s", result);
        }

        [Test]
        public void Update_UpdatesTourProperties()
        {
            // Arrange
            var tour = new Tour
            {
                Name = "Old Name",
                Description = "Old Description",
                From = "A",
                To = "B",
                TransportType = ETransportType.Car,
                Distance = 1000,
                Time = 600,
                RouteInformation = "Old Route",
                OSMjson = "Old JSON"
            };

            var newTour = new Tour
            {
                Name = "New Name",
                Description = "New Description",
                From = "C",
                To = "D",
                TransportType = ETransportType.Bike,
                Distance = 2000,
                Time = 1200,
                RouteInformation = "New Route",
                OSMjson = "New JSON"
            };

            // Act
            tour.Update(newTour);

            // Assert
            ClassicAssert.AreEqual("New Name", tour.Name);
            ClassicAssert.AreEqual("New Description", tour.Description);
            ClassicAssert.AreEqual("C", tour.From);
            ClassicAssert.AreEqual("D", tour.To);
            ClassicAssert.AreEqual(ETransportType.Bike, tour.TransportType);
            ClassicAssert.AreEqual(2000, tour.Distance);
            ClassicAssert.AreEqual(1200, tour.Time);
            ClassicAssert.AreEqual("New Route", tour.RouteInformation);
            ClassicAssert.AreEqual("New JSON", tour.OSMjson);
        }

        [Test]
        public void AreAllInputParamsSet_AllParamsSet_ReturnsTrue()
        {
            // Arrange
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Description",
                From = "A",
                To = "B",
                TransportType = ETransportType.Car
            };

            // Act
            var result = tour.AreAllInputParamsSet();

            // Assert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void AreAllInputParamsSet_MissingParams_ReturnsFalse()
        {
            // Arrange
            var tour = new Tour
            {
                Description = "Description",
                From = "A",
                To = "B",
                TransportType = ETransportType.Car
            };

            // Act
            var result = tour.AreAllInputParamsSet();

            // Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var tour = new Tour
            {
                Id = 1,
                Name = "Test Tour",
                Description = "Description",
                From = "A",
                To = "B",
                TransportType = ETransportType.Car,
                Distance = 1500,
                Time = 3661
            };

            // Act
            var result = tour.ToString();

            // Assert
            var expected = "Tour Id: 1, Name: Test Tour, Description: Description, From: A, To: B, Transport Type: Car, Distance: 1.5km, Time: 01h:01m:01s";
            ClassicAssert.AreEqual(expected, result);
        }
    }
}
