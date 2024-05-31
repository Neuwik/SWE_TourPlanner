using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SWE_TourPlanner_WPF.BusinessLayer.MapHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private List<Tour> mock_tours = new List<Tour>();
        private List<TourLog> mock_logs = new List<TourLog>();
        private Tour mock_selected_tour;

        public async Task<Tour> AddTour(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to add Tour {tour}.");

                //throw new BLLNotImplementedException("Add Tour");

                //Make a new Tour with incoming Information
                Tour newTour = new Tour(tour);


                //Calculate Route
                await CalculateRoute(newTour);

                //Add Tour to DB -> here only Mock
                newTour.Id = Mock_GetNextTourId();
                mock_tours.Add(newTour);

                IBusinessLayer.logger.Debug($"Tour {newTour} was added.");

                //Return Copy of added Tour
                return new Tour(newTour);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Adding Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Adding Tour {tour} failed: {e.Message}");
                throw;
            }
        }

        public TourLog AddTourLogToTour(Tour tour, TourLog log)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to add TourLog {log} to Tour {tour}.");
                //throw new BLLNotImplementedException("Add Tour Log");

                //Make a new TourLog with incoming Information
                TourLog newLog = new TourLog(log);

                // Check if Tour exists
                mock_selected_tour = GetExistingTour(tour.Id);

                //Set Tour Id
                newLog.TourId = tour.Id;

                //Add TourLog to DB -> here only Mock
                newLog.Id = Mock_GetNextTourLogId();
                mock_selected_tour.TourLogs.Add(newLog);
                mock_logs.Add(newLog);

                IBusinessLayer.logger.Debug($"TourLog {newLog} was added to Tour {mock_selected_tour}.");
                //Return Copy of added TourLog
                return new TourLog(newLog);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Adding TourLog {log} to Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Adding TourLog {log} to Tour {tour} failed: {e.Message}");
                throw;
            }
        }

        public async Task<List<Tour>> GetAllTours()
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get all Tours.");
                //throw new BLLNotImplementedException("Get All Tours");

                // currently only seed data
                if (mock_tours == null || mock_tours.Count <= 0)
                {
                    await Mock_TestTours();
                }

                // Return List of Copies
                List<Tour> tours = new List<Tour>();

                foreach (Tour t in mock_tours)
                {
                    tours.Add(new Tour(t));
                }

                IBusinessLayer.logger.Debug($"Got all Tours.");
                return tours;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting all Tours failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting all Tours failed: {e.Message}");
                throw;
            }
        }

        public List<TourLog> GetAllTourLogsOfTour(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get all TourLogs from Tour {tour}.");
                //throw new BLLNotImplementedException("Get All Tours Log Of Tour");

                // check if tour exists
                // get all logs
                mock_selected_tour = GetExistingTour(tour.Id);

                // Return List of Copies
                List<TourLog> logs = new List<TourLog>();

                foreach (TourLog l in mock_selected_tour.TourLogs)
                {
                    logs.Add(new TourLog(l));
                }

                IBusinessLayer.logger.Debug($"Got all TourLogs from Tour {tour}.");
                return logs;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting all TourLogs from Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting all TourLogs from Tour {tour} failed: {e.Message}");
                throw;
            }
        }

        public Tour RemoveTour(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to remove Tour {tour}.");
                //throw new BLLNotImplementedException("Remove Tour");
                // check if tour exists
                // remove tour
                mock_selected_tour = GetExistingTour(tour.Id);
                mock_tours.Remove(mock_selected_tour);

                IBusinessLayer.logger.Debug($"Removed Tour {tour}.");
                return mock_selected_tour;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Removing Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Removing Tour {tour} failed: {e.Message}");
                throw;
            }
        }

        public TourLog RemoveTourLog(TourLog log)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to remove TourLog {log}.");

                //throw new BLLNotImplementedException("Remove Tour Log");
                // check if tour exists
                // check if tour log exists
                // remove tour log

                mock_logs.Remove(GetExistingTourLog(log.Id));
                mock_selected_tour = GetExistingTour(log.TourId);
                mock_selected_tour.TourLogs.Remove(log);

                IBusinessLayer.logger.Debug($"Removed TourLog {log}.");
                return log;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Removing TourLog {log} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Removing TourLog {log} failed: {e.Message}");
                throw;
            }
        }

        public Tour UpdateTour(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to update Tour {tour}.");
                //throw new BLLNotImplementedException("Update Tour");
                // check if tour exists
                // check what changed
                mock_selected_tour = GetExistingTour(tour.Id);
                List<TourLog> temp_TourLogs = GetAllTourLogsOfTour(mock_selected_tour);
                foreach (TourLog log in tour.TourLogs)
                {
                    TourLog temp = temp_TourLogs.Find(l => l.Id == log.Id);
                    if (temp != null)
                    {
                        temp_TourLogs.Remove(temp);
                        UpdateTourLog(log);
                    }
                    else
                    {
                        AddTourLogToTour(tour, log);
                    }
                }

                foreach (TourLog temp in temp_TourLogs)
                {
                    RemoveTourLog(temp);
                }

                IBusinessLayer.logger.Debug($"Updated Tour {tour}.");
                return tour;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Updating Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Updating Tour {tour} failed: {e.Message}");
                throw;
            }
        }

        public TourLog UpdateTourLog(TourLog log)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to update TourLog {log}.");
                //throw new BLLNotImplementedException("Update Tour Log");

                TourLog tLog = GetExistingTourLog(log.Id);
                tLog.Update(log);

                IBusinessLayer.logger.Debug($"Updated TourLog {log}.");
                return log;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Updating TourLog {log} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Updating TourLog {log} failed: {e.Message}");
                throw;
            }
        }

        private Tour GetExistingTour(int id)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get Tour {id}.");
                Tour tour = mock_tours.Find(t => t.Id == id);
                if (tour == null)
                {
                    throw new BLLConflictException("Tour does not exist");
                }
                IBusinessLayer.logger.Debug($"Got Tour {tour}.");
                return tour;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting Tour {id} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting Tour {id} failed: {e.Message}");
                throw;
            }
        }

        private TourLog GetExistingTourLog(int id)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get TourLog {id}.");
                TourLog log = mock_logs.Find(l => l.Id == id);
                if (log == null)
                {
                    throw new BLLConflictException("Tour Log does not exist");
                }
                IBusinessLayer.logger.Debug($"Got TourLog {log}.");
                return log;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting TourLog {id} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting TourLog {id} failed: {e.Message}");
                throw;
            }
        }

        private async Task CalculateRoute(Tour tour)
        {
            if (tour != null && !String.IsNullOrEmpty(tour.From) && !String.IsNullOrEmpty(tour.To))
            {
                try
                {
                    IBusinessLayer.logger.Debug($"Trying to calculate Route for Tour {tour}.");

                    var json = await new OpenRouteService(IBusinessLayer.ApiKey).GetDirectionsAsync(tour.From, tour.To, tour.TransportType);

                    var directionsResult = JsonConvert.DeserializeObject<DirectionsResult>(json);
                    if (directionsResult != null && directionsResult.Features != null && directionsResult.Features.Count > 0)
                    {
                        var firstFeature = directionsResult.Features[0];
                        var coordinates = firstFeature.Geometry.Coordinates;
                        var properties = firstFeature.Properties;
                        var summary = properties.Summary;

                        tour.OSMjson = json;
                        tour.Distance = summary.Distance;
                        tour.Time = summary.Duration;
                        tour.RouteInformation = $"Tour from {tour.From} to {tour.To} by {tour.TransportType}\n{properties.ToString()}";
                    }

                    IBusinessLayer.logger.Debug($"Calculated Route for Tour {tour}.");
                }
                catch (BusinessLayerException e)
                {
                    IBusinessLayer.logger.Error($"Calculating Route for Tour {tour} failed: {e}");
                    throw;
                }
                catch (Exception e)
                {
                    IBusinessLayer.logger.Error($"Calculating Route for Tour {tour} failed: {e.Message}");
                    throw new BLLConflictException("Route could not be calculated.");
                }
            }
            else
            {
                throw new BLLConflictException("Route could not be calculated.");
            }
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

        private async Task Mock_TestTours()
        {
            mock_tours = new List<Tour>();
            for (int i = 1; i < 2; i++)
            {
                Tour t = await AddTour(new Tour()
                {
                    Name = $"Name {i}",
                    Description = $"Desc {i}",
                    From = $"HTL Krems",
                    To = $"FH-Technikum Wien",
                    TransportType = ETransportType.Car,
                    RouteInformation = $"Info {i}"
                });
            }
        }
    }
}
