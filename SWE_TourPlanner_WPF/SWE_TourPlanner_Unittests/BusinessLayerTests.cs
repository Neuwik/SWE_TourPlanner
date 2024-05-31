using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWE_TourPlanner_WPF.BusinessLayer;
using SWE_TourPlanner_WPF.DataBase;
using SWE_TourPlanner_WPF;
using System.Linq;
using NUnit.Framework.Legacy;

namespace SWE_TourPlanner_Unittests
{
    [TestFixture]
    public class BusinessLayerTests
    {
        private Mock<DatabaseHandler> _databaseHandlerMock;
        private BusinessLayer _businessLayer;

        [SetUp]
        public void Setup()
        {
            _databaseHandlerMock = new Mock<DatabaseHandler>();
            //_businessLayer = new BusinessLayer(_databaseHandlerMock.Object);
        }

        [Test]
        public async Task AddTour_AddsTourToDatabase()
        {
            // Arrange
            var tour = new Tour
            {
                Name = "Test Tour",
                From = "A",
                To = "B",
                TransportType = ETransportType.Car
            };

            // Act
            var result = await _businessLayer.AddTour(tour);

            // Assert
            _databaseHandlerMock.Verify(d => d.AddTour(It.IsAny<Tour>()), Times.Once);
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(tour.Name, result.Name);
        }

        [Test]
        public void AddTourLogToTour_AddsTourLogToTour()
        {
            // Arrange
            var tour = new Tour { Id = 1, Name = "Test Tour" };
            var log = new TourLog { Comment = "Test Log", Difficulty = EDifficulty.easy };

            // Mock existing tour
            _databaseHandlerMock.Setup(d => d.GetTour(1)).Returns(tour);

            // Act
            var result = _businessLayer.AddTourLogToTour(tour, log);

            // Assert
            _databaseHandlerMock.Verify(d => d.AddTourLog(It.IsAny<TourLog>()), Times.Once);
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(log.Comment, result.Comment);
        }

        [Test]
        public async Task GetAllTours_ReturnsAllTours()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { Id = 1, Name = "Test Tour 1" },
                new Tour { Id = 2, Name = "Test Tour 2" }
            };

            _businessLayer.Setup(d => d.GetAllTours()).ReturnsAsync(tours);

            // Act
            var result = await _businessLayer.GetAllTours();

            // Assert
            ClassicAssert.AreEqual(tours.Count, result.Count);
            ClassicAssert.AreEqual(tours[0].Name, result[0].Name);
        }

        [Test]
        public void GetAllTourLogsOfTour_ReturnsAllLogs()
        {
            // Arrange
            var tour = new Tour { Id = 1, Name = "Test Tour" };
            var logs = new List<TourLog>
            {
                new TourLog { Id = 1, Comment = "Log 1", TourId = 1 },
                new TourLog { Id = 2, Comment = "Log 2", TourId = 1 }
            };

            _databaseHandlerMock.Setup(d => d.GetTour(1)).Returns(tour);
            _databaseHandlerMock.Setup(d => GetAllLogsByTourId(1)).Returns(logs);

            // Act
            var result = _businessLayer.GetAllTourLogsOfTour(tour);

            // Assert
            ClassicAssert.AreEqual(logs.Count, result.Count);
            ClassicAssert.AreEqual(logs[0].Comment, result[0].Comment);
        }
    }
}
