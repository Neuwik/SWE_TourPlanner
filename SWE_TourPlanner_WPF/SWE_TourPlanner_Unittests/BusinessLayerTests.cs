using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.EntityFrameworkCore;
using Moq;
using SWE_TourPlanner_WPF.BusinessLayer;
using SWE_TourPlanner_WPF.DataAccessLayer;
using SWE_TourPlanner_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class BusinessLayerTests
    {
        private DatabaseContext _dbContext;
        private DatabaseHandler _databaseHandler;
        private BusinessLayer _businessLayer;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new DatabaseContext(options);

            _databaseHandler = new DatabaseHandler(_dbContext);
            _businessLayer = new BusinessLayer(_databaseHandler);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Test_BusinessLayer_AddTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "1200 Wien",
                To = "4600 Wels"
            };

            await _businessLayer.AddTour(tour);
            var allTour = _businessLayer.GetAllTours();
            var addedTour = allTour.Last();

            Assert.Multiple(() =>
            {
                Assert.That(allTour.Count, Is.GreaterThan(0));
                Assert.That(addedTour.Name, Is.EqualTo(tour.Name));
                Assert.That(addedTour.Description, Is.EqualTo(tour.Description));
                Assert.That(addedTour.From, Is.EqualTo(tour.From));
                Assert.That(addedTour.To, Is.EqualTo(tour.To));
                Assert.That(addedTour.TransportType, Is.EqualTo(tour.TransportType));

                Assert.That(addedTour.Distance, Is.GreaterThan(0));
                Assert.That(addedTour.Time, Is.GreaterThan(0));
                Assert.That(addedTour.RouteInformation, Is.Not.Null.Or.Empty);
                Assert.That(addedTour.OSMjson, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        public async Task Test_BusinessLayer_RemoveTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "1200 Wien",
                To = "4600 Wels",
                TransportType = ETransportType.Car
            };

            await _businessLayer.AddTour(tour);
            var allTourBefor = _businessLayer.GetAllTours();
            var addedTour = allTourBefor.Last();

            _businessLayer.RemoveTour(addedTour);
            var allTourAfter = _businessLayer.GetAllTours();

            Assert.That(allTourAfter.Count, Is.LessThan(allTourBefor.Count));
        }

        [Test]
        public async Task Test_BusinessLayer_UpdateTour()
        {
            var oldTour = new Tour
            {
                Name = "Old Name",
                Description = "Old Description",
                From = "1200 Wien",
                To = "4600 Wels",
                TransportType = ETransportType.Car
            };

            await _businessLayer.AddTour(oldTour);
            var addedTour = _businessLayer.GetAllTours().Last();

            var changedTour = new Tour(addedTour);

            changedTour.Name = "New Name";
            changedTour.Description = "New Description";

            _businessLayer.UpdateTour(changedTour);
            var updatedTour = _businessLayer.GetAllTours().Last();

            Assert.Multiple(() =>
            {
                Assert.That(updatedTour.Name, Is.EqualTo(changedTour.Name));
                Assert.That(updatedTour.Description, Is.EqualTo(changedTour.Description));
            });
        }


        [Test]
        public async Task Test_BusinessLayer_AddTourLogToTour()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "1200 Wien",
                To = "4600 Wels"
            };

            await _businessLayer.AddTour(tour);
            var allTour = _businessLayer.GetAllTours();
            var addedTour = allTour.Last();

            var tourLog = new TourLog
            {
                DateTime = DateTime.Now,
                Comment = "Test Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.5,
                TotalTime = 2.5,
                Rating = ERating.FourStars
            };

            _businessLayer.AddTourLogToTour(addedTour, tourLog);
            var allTourLogs = _businessLayer.GetAllTourLogsOfTour(addedTour);
            var addedTourLog = allTourLogs.Last();

            Assert.Multiple(() =>
            {
                Assert.That(addedTourLog.TourId, Is.EqualTo(addedTour.Id));

                Assert.That(addedTourLog.Comment, Is.EqualTo(tourLog.Comment));
                Assert.That(addedTourLog.Difficulty, Is.EqualTo(tourLog.Difficulty));
                Assert.That(addedTourLog.TotalDistance, Is.EqualTo(tourLog.TotalDistance));
                Assert.That(addedTourLog.TotalTime, Is.EqualTo(tourLog.TotalTime));
                Assert.That(addedTourLog.Rating, Is.EqualTo(tourLog.Rating));
                Assert.That(addedTourLog.DateTime, Is.EqualTo(tourLog.DateTime.ToUniversalTime()));
            });
        }

        [Test]
        public async Task Test_BusinessLayer_RemoveTourLog()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "1200 Wien",
                To = "4600 Wels",
                TransportType = ETransportType.Car
            };

            await _businessLayer.AddTour(tour);
            var addedTour = _businessLayer.GetAllTours().Last();

            var tourLog = new TourLog
            {
                DateTime = DateTime.Now,
                Comment = "Test Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.5,
                TotalTime = 2.5,
                Rating = ERating.FourStars
            };

            _businessLayer.AddTourLogToTour(addedTour, tourLog);
            var allTourLogsBefor = _businessLayer.GetAllTourLogsOfTour(addedTour);
            var addedTourLog = allTourLogsBefor.Last();

            _businessLayer.RemoveTourLog(addedTourLog);
            var allTourLogsAfter = _businessLayer.GetAllTourLogsOfTour(addedTour);

            Assert.That(allTourLogsAfter.Count, Is.LessThan(allTourLogsBefor.Count));
        }


        [Test]
        public async Task Test_BusinessLayer_UpdateTourLog()
        {
            var tour = new Tour
            {
                Name = "Test Tour",
                Description = "Test Description",
                From = "1200 Wien",
                To = "4600 Wels"
            };

            await _businessLayer.AddTour(tour);
            var allTour = _businessLayer.GetAllTours();
            var addedTour = allTour.Last();

            var tourLog = new TourLog
            {
                DateTime = DateTime.Now,
                Comment = "Old Comment",
                Difficulty = EDifficulty.Medium,
                TotalDistance = 10.5,
                TotalTime = 2.5,
                Rating = ERating.FourStars
            };

            _businessLayer.AddTourLogToTour(addedTour, tourLog);
            var addedTourLog = _businessLayer.GetAllTourLogsOfTour(addedTour).Last();

            var changedTourLog = new TourLog(addedTourLog);

            changedTourLog.Comment = "New Comment";
            changedTourLog.Difficulty = EDifficulty.Hard;
            changedTourLog.TotalDistance = 100;
            changedTourLog.TotalTime = 10;
            changedTourLog.Rating = ERating.OneStars;

            _businessLayer.UpdateTourLog(changedTourLog);
            var updatedTourLog = _businessLayer.GetAllTourLogsOfTour(addedTour).Last();

            Assert.Multiple(() =>
            {
                Assert.That(updatedTourLog.Comment, Is.EqualTo(changedTourLog.Comment));
                Assert.That(updatedTourLog.Difficulty, Is.EqualTo(changedTourLog.Difficulty));
                Assert.That(updatedTourLog.TotalDistance, Is.EqualTo(changedTourLog.TotalDistance));
                Assert.That(updatedTourLog.TotalTime, Is.EqualTo(changedTourLog.TotalTime));
                Assert.That(updatedTourLog.Rating, Is.EqualTo(changedTourLog.Rating));
            });
        }
    }
}
