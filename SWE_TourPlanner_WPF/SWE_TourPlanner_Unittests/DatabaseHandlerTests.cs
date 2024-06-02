using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using SWE_TourPlanner_WPF.DataAccessLayer;
using SWE_TourPlanner_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class DatabaseHandlerTests
    {
        private DatabaseContext _dbContext;
        private DatabaseHandler _databaseHandler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new DatabaseContext(options);

            _databaseHandler = new DatabaseHandler(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void Test_DatabaseHandler_InMemoryDatabase()
        {
            Assert.That(_databaseHandler, Is.Not.Null);
        }

        [Test]
        public void Test_DatabaseHandler_AddTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            Assert.Multiple(() =>
            {
                Assert.That(addedTour.Name, Is.EqualTo(tour.Name));
                Assert.That(addedTour.Description, Is.EqualTo(tour.Description));
                Assert.That(addedTour.From, Is.EqualTo(tour.From));
                Assert.That(addedTour.To, Is.EqualTo(tour.To));
                Assert.That(addedTour.TransportType, Is.EqualTo(tour.TransportType));
                Assert.That(addedTour.Distance, Is.EqualTo(tour.Distance));
                Assert.That(addedTour.Time, Is.EqualTo(tour.Time));
                Assert.That(addedTour.RouteInformation, Is.EqualTo(tour.RouteInformation));
                Assert.That(addedTour.OSMjson, Is.EqualTo(tour.OSMjson));
            });
        }

        [Test]
        public void Test_DatabaseHandler_GetAllTours()
        {
            var tour1 = new Tour
            {
                Name = "Tour 1",
                Description = "Description 1",
                From = "A",
                To = "B",
                TransportType = ETransportType.Bike,
                Distance = 10.0,
                Time = 60.0,
                RouteInformation = "Route 1",
                OSMjson = "JSON 1"
            };

            var tour2 = new Tour
            {
                Name = "Tour 2",
                Description = "Description 2",
                From = "C",
                To = "D",
                TransportType = ETransportType.Foot,
                Distance = 5.0,
                Time = 30.0,
                RouteInformation = "Route 2",
                OSMjson = "JSON 2"
            };

            _databaseHandler.AddTour(tour1);
            _databaseHandler.AddTour(tour2);

            var tours = _databaseHandler.GetAllTours();

            Assert.That(tours.Count, Is.EqualTo(2));
        }

        [Test]
        public void Test_DatabaseHandler_GetLastTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            Assert.Multiple(() =>
            {
                Assert.That(addedTour.Name, Is.EqualTo(tour.Name));
                Assert.That(addedTour.Description, Is.EqualTo(tour.Description));
                Assert.That(addedTour.From, Is.EqualTo(tour.From));
                Assert.That(addedTour.To, Is.EqualTo(tour.To));
                Assert.That(addedTour.TransportType, Is.EqualTo(tour.TransportType));
                Assert.That(addedTour.Distance, Is.EqualTo(tour.Distance));
                Assert.That(addedTour.Time, Is.EqualTo(tour.Time));
                Assert.That(addedTour.RouteInformation, Is.EqualTo(tour.RouteInformation));
                Assert.That(addedTour.OSMjson, Is.EqualTo(tour.OSMjson));
            });
        }

        [Test]
        public void Test_DatabaseHandler_GetTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();
            var fetchedTour = _databaseHandler.GetTour(addedTour.Id);

            Assert.Multiple(() =>
            {
                Assert.That(fetchedTour.Name, Is.EqualTo(tour.Name));
                Assert.That(fetchedTour.Description, Is.EqualTo(tour.Description));
                Assert.That(fetchedTour.From, Is.EqualTo(tour.From));
                Assert.That(fetchedTour.To, Is.EqualTo(tour.To));
                Assert.That(fetchedTour.TransportType, Is.EqualTo(tour.TransportType));
                Assert.That(fetchedTour.Distance, Is.EqualTo(tour.Distance));
                Assert.That(fetchedTour.Time, Is.EqualTo(tour.Time));
                Assert.That(fetchedTour.RouteInformation, Is.EqualTo(tour.RouteInformation));
                Assert.That(fetchedTour.OSMjson, Is.EqualTo(tour.OSMjson));
            });
        }

        [Test]
        public void Test_DatabaseHandler_UpdateTour()
        {
            var tour = new Tour
            {
                Name = "Old Name",
                Description = "Old Description",
                From = "Old Location A",
                To = "Old Location B",
                TransportType = ETransportType.Car,
                Distance = 100.0,
                Time = 120.0,
                RouteInformation = "Old Route Info",
                OSMjson = "Old OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            var changedTour = new Tour(addedTour);

            changedTour.Name = "New Name";
            changedTour.Description = "New Description";
            changedTour.From = "New Location A";
            changedTour.To = "New Location B";
            changedTour.TransportType = ETransportType.Bike;
            changedTour.Distance = 150.0;
            changedTour.Time = 180.0;
            changedTour.RouteInformation = "New Route Info";
            changedTour.OSMjson = "New OSM Data";

            _databaseHandler.UpdateTour(changedTour);
            var updatedTour = _databaseHandler.GetTour(changedTour.Id);

            Assert.Multiple(() =>
            {
                Assert.That(updatedTour.Name, Is.EqualTo(changedTour.Name));
                Assert.That(updatedTour.Description, Is.EqualTo(changedTour.Description));
                Assert.That(updatedTour.From, Is.EqualTo(changedTour.From));
                Assert.That(updatedTour.To, Is.EqualTo(changedTour.To));
                Assert.That(updatedTour.TransportType, Is.EqualTo(changedTour.TransportType));
                Assert.That(updatedTour.Distance, Is.EqualTo(changedTour.Distance));
                Assert.That(updatedTour.Time, Is.EqualTo(changedTour.Time));
                Assert.That(updatedTour.RouteInformation, Is.EqualTo(changedTour.RouteInformation));
                Assert.That(updatedTour.OSMjson, Is.EqualTo(changedTour.OSMjson));
            });
        }

        [Test]
        public void Test_DatabaseHandler_DeleteTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();
            _databaseHandler.DeleteTour(addedTour);

            var fetchedTour = _databaseHandler.GetTour(addedTour.Id);

            Assert.That(fetchedTour, Is.Null);
        }

        [Test]
        public void Test_DatabaseHandler_AddTourLog()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            var tourLog = new TourLog
            {
                TourId = addedTour.Id,
                DateTime = DateTime.Now,
                Comment = "Test Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.5,
                TotalTime = 2.5,
                Rating = ERating.FourStars
            };

            _databaseHandler.AddTourLog(tourLog);
            var addedTourLog = _databaseHandler.GetLastTourLog();

            Assert.Multiple(() =>
            {
                Assert.That(addedTourLog.Comment, Is.EqualTo(tourLog.Comment));
                Assert.That(addedTourLog.Difficulty, Is.EqualTo(tourLog.Difficulty));
                Assert.That(addedTourLog.TotalDistance, Is.EqualTo(tourLog.TotalDistance));
                Assert.That(addedTourLog.TotalTime, Is.EqualTo(tourLog.TotalTime));
                Assert.That(addedTourLog.Rating, Is.EqualTo(tourLog.Rating));
                Assert.That(addedTourLog.DateTime, Is.EqualTo(tourLog.DateTime.ToUniversalTime()));
            });
        }

        [Test]
        public void Test_DatabaseHandler_GetTourLogs()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            var tourLog1 = new TourLog
            {
                TourId = addedTour.Id,
                DateTime = DateTime.Now,
                Comment = "Log 1",
                Difficulty = EDifficulty.Easy,
                TotalDistance = 5.0,
                TotalTime = 1.0,
                Rating = ERating.FiveStars
            };

            var tourLog2 = new TourLog
            {
                TourId = addedTour.Id,
                DateTime = DateTime.Now,
                Comment = "Log 2",
                Difficulty = EDifficulty.Hard,
                TotalDistance = 15.0,
                TotalTime = 3.5,
                Rating = ERating.OneStars
            };

            _databaseHandler.AddTourLog(tourLog1);
            _databaseHandler.AddTourLog(tourLog2);

            var logs = _databaseHandler.GetTourLogs(addedTour.Id);

            Assert.That(logs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Test_DatabaseHandler_UpdateTourLog()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            var tourLog = new TourLog
            {
                TourId = addedTour.Id,
                DateTime = DateTime.Now,
                Comment = "Old Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.0,
                TotalTime = 2.0,
                Rating = ERating.ThreeStars
            };

            _databaseHandler.AddTourLog(tourLog);
            var addedTourLog = _databaseHandler.GetLastTourLog();

            var changedTourLog = new TourLog(addedTourLog);
            changedTourLog.Comment = "New Comment";
            changedTourLog.Difficulty = EDifficulty.Hard;
            changedTourLog.TotalDistance = 12.0;
            changedTourLog.TotalTime = 2.5;
            changedTourLog.Rating = ERating.FourStars;
            _databaseHandler.UpdateTourLog(changedTourLog);

            var updatedTourLog = _databaseHandler.GetTourLog(changedTourLog.Id);

            Assert.Multiple(() =>
            {
                Assert.That(updatedTourLog.Comment, Is.EqualTo(changedTourLog.Comment));
                Assert.That(updatedTourLog.Difficulty, Is.EqualTo(changedTourLog.Difficulty));
                Assert.That(updatedTourLog.TotalDistance, Is.EqualTo(changedTourLog.TotalDistance));
                Assert.That(updatedTourLog.TotalTime, Is.EqualTo(changedTourLog.TotalTime));
                Assert.That(updatedTourLog.Rating, Is.EqualTo(changedTourLog.Rating));
            });
        }

        [Test]
        public void Test_DatabaseHandler_DeleteTourLog()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "Location A",
                To = "Location B",
                TransportType = ETransportType.Car,
                Distance = 100.5,
                Time = 120.0,
                RouteInformation = "Route Info",
                OSMjson = "OSM Data"
            };

            _databaseHandler.AddTour(tour);
            var addedTour = _databaseHandler.GetLastTour();

            var tourLog = new TourLog
            {
                TourId = addedTour.Id,
                DateTime = DateTime.Now,
                Comment = "Test Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.0,
                TotalTime = 2.0,
                Rating = ERating.ThreeStars
            };

            _databaseHandler.AddTourLog(tourLog);
            var addedTourLog = _databaseHandler.GetLastTourLog();
            _databaseHandler.DeleteTourLog(addedTourLog);

            var fetchedTourLog = _databaseHandler.GetTourLog(addedTourLog.Id);

            Assert.That(fetchedTourLog, Is.Null);
        }
    }
}
