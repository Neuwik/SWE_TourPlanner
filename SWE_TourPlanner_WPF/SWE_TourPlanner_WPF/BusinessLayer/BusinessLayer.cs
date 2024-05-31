using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private List<Tour> mock_tours = new List<Tour>();
        private List<TourLog> mock_logs = new List<TourLog>();
        private Tour mock_selected_tour;

        public Tour AddTour(Tour tour)
        {
            //throw new BLLNotImplementedException("Add Tour");

            // Call Open Street View
            // Calling DAL and try add
            tour.Id = Mock_GetNextTourId();
            tour.OSMjson = GetOSMjson(tour);
            mock_tours.Add(tour);
            return tour;
        }

        public TourLog AddTourLog(TourLog log)
        {
            //throw new BLLNotImplementedException("Add Tour Log");

            // Check if Tour exists
            log.Id = Mock_GetNextTourLogId();
            mock_selected_tour = GetExistingTour(log.TourId);
            mock_selected_tour.TourLogs.Add(log);
            mock_logs.Add(log);

            return log;
        }

        public List<Tour> GetAllTours()
        {
            //throw new BLLNotImplementedException("Get All Tours");

            // currently only seed data
            if (mock_tours == null || mock_tours.Count <= 0)
            {
                mock_tours = Mock_TestTours();
            }

            return mock_tours;
        }

        public List<TourLog> GetAllToursLogOfTour(Tour tour)
        {
            //throw new BLLNotImplementedException("Get All Tours Log Of Tour");

            // check if tour exists
            // get all logs
            mock_selected_tour = GetExistingTour(tour.Id);
            return mock_selected_tour.TourLogs;
        }

        public Tour RemoveTour(Tour tour)
        {
            //throw new BLLNotImplementedException("Remove Tour");
            // check if tour exists
            // remove tour
            mock_selected_tour = GetExistingTour(tour.Id);
            mock_tours.Remove(mock_selected_tour);
            return mock_selected_tour;
        }

        public TourLog RemoveTourLog(TourLog log)
        {
            //throw new BLLNotImplementedException("Remove Tour Log");
            // check if tour exists
            // check if tour log exists
            // remove tour log

            mock_logs.Remove(GetExistingTourLog(log.Id));
            mock_selected_tour = GetExistingTour(log.TourId);
            mock_selected_tour.TourLogs.Remove(log);
            return log;
        }

        public Tour UpdateTour(Tour tour)
        {
            // check if tour exists
            // check what changed
            throw new BLLNotImplementedException("Update Tour");
            foreach (TourLog log in tour.TourLogs)
            {

            }
        }

        public TourLog UpdateTourLog(TourLog log)
        {
            throw new BLLNotImplementedException("Update Tour Log");
        }

        private Tour GetExistingTour(int id)
        {
            Tour tour = mock_tours.Find(t => t.Id == id);
            if (tour == null)
            {
                throw new BLLConflictException("Tour does not exist");
            }
            return tour;
        }

        private TourLog GetExistingTourLog(int id)
        {
            TourLog log = mock_logs.Find(l => l.Id == id);
            if (log == null)
            {
                throw new BLLConflictException("Tour Log does not exist");
            }
            return log;
        }

        private string GetOSMjson(Tour tour)
        {
            return "Not Implemented Yet";
        }

        private int Mock_GetNextTourId()
        {
            Tour tour = mock_tours.OrderByDescending(t => t.Id).FirstOrDefault();
            if (tour == null)
            {
                return 1;
            }
            return tour.Id + 1;
        }

        private int Mock_GetNextTourLogId()
        {
            TourLog log = mock_logs.OrderByDescending(l => l.Id).FirstOrDefault();
            if (log == null)
            {
                return 1;
            }
            return log.Id + 1;
        }

        private List<Tour> Mock_TestTours()
        {
            List<Tour> tours = new List<Tour>();
            for (int i = 1; i < 6; i++)
            {
                tours.Add(new Tour()
                {
                    Name = $"Name {i}",
                    Description = $"Desc {i}",
                    From = $"HTL Krems",
                    To = $"FH-Technikum Wien",
                    TransportType = ETransportType.Car,
                    Distance = i * 100,
                    Time = i * 30,
                    RouteInformation = $"Info {i}"
                });
            }
            return tours;
        }
    }
}
