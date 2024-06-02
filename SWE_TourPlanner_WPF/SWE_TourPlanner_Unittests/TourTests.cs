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
        public void Test_Tour_DefaultConstructor()
        {
            var tour = new Tour();

            Assert.That(tour.Id, Is.EqualTo(0));
            Assert.That(tour.Name, Is.Null);
            Assert.That(tour.Description, Is.Null);
            Assert.That(tour.From, Is.Null);
            Assert.That(tour.To, Is.Null);
            Assert.That(tour.TransportType, Is.EqualTo(default(ETransportType)));
            Assert.That(tour.Distance, Is.EqualTo(0));
            Assert.That(tour.Time, Is.EqualTo(0));
            Assert.That(tour.RouteInformation, Is.Null);
            Assert.That(tour.OSMjson, Is.Null);
            Assert.That(tour.TourLogs, Is.Not.Null);
            Assert.That(tour.TourLogs.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_Tour_ParameterizedConstructor()
        {
            var name = "Tour Name";
            var description = "Tour Description";
            var from = "Start Location";
            var to = "End Location";
            var transportType = ETransportType.Car;
            var distance = 100;
            var time = 60;
            var routeInformation = "Route Info";
            var osmjson = "OSM JSON";
            var tourLogs = new List<TourLog> { new TourLog { Comment = "Log1" } };

            var tour = new Tour(name, description, from, to, transportType, distance, time, routeInformation, osmjson, tourLogs);

            Assert.That(tour.Id, Is.EqualTo(0));
            Assert.That(tour.Name, Is.EqualTo(name));
            Assert.That(tour.Description, Is.EqualTo(description));
            Assert.That(tour.From, Is.EqualTo(from));
            Assert.That(tour.To, Is.EqualTo(to));
            Assert.That(tour.TransportType, Is.EqualTo(transportType));
            Assert.That(tour.Distance, Is.EqualTo(distance));
            Assert.That(tour.Time, Is.EqualTo(time));
            Assert.That(tour.RouteInformation, Is.EqualTo(routeInformation));
            Assert.That(tour.OSMjson, Is.EqualTo(osmjson));
            Assert.That(tour.TourLogs, Is.EqualTo(tourLogs));
        }

        [Test]
        public void Test_Tour_CopyConstructor()
        {
            var original = new Tour
            {
                Id = 1,
                Name = "Tour Name",
                Description = "Tour Description",
                From = "Start Location",
                To = "End Location",
                TransportType = ETransportType.Bike,
                Distance = 200,
                Time = 120,
                RouteInformation = "Route Info",
                OSMjson = "OSM JSON",
                TourLogs = new List<TourLog> { new TourLog { Comment = "Log1" } }
            };

            var copy = new Tour(original);

            Assert.That(copy.Id, Is.EqualTo(original.Id));
            Assert.That(copy.Name, Is.EqualTo(original.Name));
            Assert.That(copy.Description, Is.EqualTo(original.Description));
            Assert.That(copy.From, Is.EqualTo(original.From));
            Assert.That(copy.To, Is.EqualTo(original.To));
            Assert.That(copy.TransportType, Is.EqualTo(original.TransportType));
            Assert.That(copy.Distance, Is.EqualTo(original.Distance));
            Assert.That(copy.Time, Is.EqualTo(original.Time));
            Assert.That(copy.RouteInformation, Is.EqualTo(original.RouteInformation));
            Assert.That(copy.OSMjson, Is.EqualTo(original.OSMjson));
            Assert.That(copy.TourLogs, Has.Count.EqualTo(original.TourLogs.Count));
            Assert.That(copy.TourLogs.First().Comment, Is.EqualTo(original.TourLogs.First().Comment));
        }

        [Test]
        public void Test_Tour_Update()
        {
            var tour = new Tour
            {
                Name = "Initial Name",
                Description = "Initial Description",
                From = "Initial From",
                To = "Initial To",
                TransportType = ETransportType.Foot,
                Distance = 300,
                Time = 180,
                RouteInformation = "Initial Info",
                OSMjson = "Initial JSON"
            };

            var newTour = new Tour
            {
                Name = "Updated Name",
                Description = "Updated Description",
                From = "Updated From",
                To = "Updated To",
                TransportType = ETransportType.Bike,
                Distance = 400,
                Time = 240,
                RouteInformation = "Updated Info",
                OSMjson = "Updated JSON"
            };

            tour.Update(newTour);

            Assert.That(tour.Name, Is.EqualTo(newTour.Name));
            Assert.That(tour.Description, Is.EqualTo(newTour.Description));
            Assert.That(tour.From, Is.EqualTo(newTour.From));
            Assert.That(tour.To, Is.EqualTo(newTour.To));
            Assert.That(tour.TransportType, Is.EqualTo(newTour.TransportType));
            Assert.That(tour.Distance, Is.EqualTo(newTour.Distance));
            Assert.That(tour.Time, Is.EqualTo(newTour.Time));
            Assert.That(tour.RouteInformation, Is.EqualTo(newTour.RouteInformation));
            Assert.That(tour.OSMjson, Is.EqualTo(newTour.OSMjson));
        }

        [Test]
        public void Test_Tour_AreAllInputParamsSet()
        {
            var tourTrue = new Tour
            {
                Name = "Tour Name",
                Description = "Tour Description",
                From = "Start Location",
                To = "End Location",
                TransportType = ETransportType.Car
            };

            var resultTrue = tourTrue.AreAllInputParamsSet();

            Assert.That(resultTrue, Is.True);


            var tourFalse = new Tour
            {
                Name = null,
                Description = "Tour Description",
                From = "Start Location",
                To = "End Location",
                TransportType = ETransportType.Car
            };

            var resultFalse = tourFalse.AreAllInputParamsSet();

            Assert.That(resultFalse, Is.False);
        }

        [Test]
        public void Test_Tour_ContainsFilter()
        {
            var tourTrue = new Tour
            {
                Name = "Tour Name",
                Description = "Tour Description"
            };

            var resultTrue = tourTrue.ContainsFilter("Tour Name");

            Assert.That(resultTrue, Is.True);


            var tourFalse = new Tour
            {
                Name = "Tour Name",
                Description = "Tour Description"
            };

            var resultFalse = tourFalse.ContainsFilter("Nonexistent");

            Assert.That(resultFalse, Is.False);
        }

        [Test]
        public void Test_Tour_AvgTourLogDifficulty_CheckCorrectCalculation()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { Difficulty = EDifficulty.Easy },
                new TourLog { Difficulty = EDifficulty.Medium },
                new TourLog { Difficulty = EDifficulty.Hard }
            };
            var tour = new Tour
            {
                TourLogs = tourLogs
            };

            var expectedAverageDifficulty = ((int)EDifficulty.Easy + (int)EDifficulty.Medium + (int)EDifficulty.Hard) / 3;

            var actualAverageDifficulty = tour.AvgTourLogDiffiuly;

            Assert.That(actualAverageDifficulty, Is.EqualTo(expectedAverageDifficulty));
        }

        [Test]
        public void Test_Tour_AvgTourLogTotalDistance_CheckCorrectCalculation()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { TotalDistance = 10.0 },
                new TourLog { TotalDistance = 20.0 },
                new TourLog { TotalDistance = 30.0 }
            };
            var tour = new Tour
            {
                TourLogs = tourLogs
            };

            var expectedAverageDistance = (10.0 + 20.0 + 30.0) / 3;

            var actualAverageDistance = tour.AvgTourLogTotalDistance;

            Assert.That(actualAverageDistance, Is.EqualTo(expectedAverageDistance));
        }

        [Test]
        public void Test_Tour_AvgTourLogTotalTime_CheckCorrectCalculation()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { TotalTime = 60.0 },
                new TourLog { TotalTime = 120.0 },
                new TourLog { TotalTime = 180.0 }
            };
            var tour = new Tour
            {
                TourLogs = tourLogs
            };

            var expectedAverageTime = (60.0 + 120.0 + 180.0) / 3;

            var actualAverageTime = tour.AvgTourLogTotalTime;

            Assert.That(actualAverageTime, Is.EqualTo(expectedAverageTime));
        }

        [Test]
        public void Test_Tour_AvgTourLogRating_CheckCorrectCalculation()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { Rating = ERating.OneStars },
                new TourLog { Rating = ERating.ThreeStars },
                new TourLog { Rating = ERating.FiveStars }
            };
            var tour = new Tour
            {
                TourLogs = tourLogs
            };

            var expectedAverageRating = ((int)ERating.OneStars + (int)ERating.ThreeStars + (int)ERating.FiveStars) / 3.0;

            var actualAverageRating = tour.AvgTourLogRating;

            Assert.That(actualAverageRating, Is.EqualTo(expectedAverageRating));
        }

        [Test]
        public void Test_Tour_Popularity_CheckIfIncreasesWithTourLogCount()
        {
            var tour = new Tour();

            var prevPopularity = tour.Popularity;

            var tourLogs = new List<TourLog>
            {
                new TourLog { Comment = "Log1" },
                new TourLog { Comment = "Log2" },
                new TourLog { Comment = "Log3" }
            };
            tour = new Tour
            {
                TourLogs = tourLogs
            };

            var afterPopularity = tour.Popularity;

            Assert.That(afterPopularity, Is.GreaterThan(prevPopularity));
        }

        [Test]
        public void Test_Tour_ChildFriendliness_CheckIfVeryEasyRouteHasChildFriendlinessVeryEasy()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { TotalDistance = 0, TotalTime = 0, Difficulty=EDifficulty.VeryEasy },
            };
            var tour = new Tour
            {
                TransportType = ETransportType.Car,
                TourLogs = tourLogs
            };

            var expectedChildFriendliness = EDifficulty.VeryEasy;

            var actualChildFriendliness = tour.ChildFriendliness;

            Assert.That(actualChildFriendliness, Is.EqualTo(expectedChildFriendliness));
        }

        [Test]
        public void Test_Tour_ChildFriendliness_CheckIfVeryHardRouteHasChildFriendlinessVeryHard()
        {
            var tourLogs = new List<TourLog>
            {
                new TourLog { TotalDistance = 999999, TotalTime = 999999, Difficulty=EDifficulty.VeryHard },
            };
            var tour = new Tour
            {
                TransportType = ETransportType.Foot,
                TourLogs = tourLogs
            };

            var expectedChildFriendliness = EDifficulty.VeryHard;

            var actualChildFriendliness = tour.ChildFriendliness;

            Assert.That(actualChildFriendliness, Is.EqualTo(expectedChildFriendliness));
        }
    }
}
