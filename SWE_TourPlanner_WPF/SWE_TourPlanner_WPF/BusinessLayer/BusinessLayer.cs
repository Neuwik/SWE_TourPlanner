﻿using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using Npgsql.Internal;
using SWE_TourPlanner_WPF.BusinessLayer.MapHelpers;
using SWE_TourPlanner_WPF.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private DatabaseHandler _DatabaseHandler;

        public BusinessLayer()
        {
            try
            {
                _DatabaseHandler = new DatabaseHandler();
            }
            catch (Exception)
            {
                throw new BLLServiceUnavailable("Could not connect to Database.");
            }
        }

        public BusinessLayer(DatabaseHandler databaseHandler)
        {
            // Used for UnitTests
            _DatabaseHandler = databaseHandler;
        }

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
                /*newTour.Id = Mock_GetNextTourId();
                mock_tours.Add(newTour);*/

                _DatabaseHandler.AddTour(newTour);

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
                throw new BusinessLayerException(e.Message);
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
                Tour dbTour = GetExistingTour(tour.Id);

                //Set Tour Id
                newLog.TourId = dbTour.Id;

                //Add TourLog to DB
                _DatabaseHandler.AddTourLog(newLog);

                IBusinessLayer.logger.Debug($"TourLog {newLog} was added to Tour {dbTour}.");
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
                throw new BusinessLayerException(e.Message);
            }
        }

        public async Task<List<Tour>> GetAllTours()
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get all Tours.");
                //throw new BLLNotImplementedException("Get All Tours");

                // Seed Data if DB Empty
                if (_DatabaseHandler.GetAllTours().Count <= 0)
                {
                    await SeedData();
                }

                // Return List of Copies
                List<Tour> tours = new List<Tour>();

                foreach (Tour t in _DatabaseHandler.GetAllTours())
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
                throw new BusinessLayerException(e.Message);
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
                Tour dbTour = GetExistingTour(tour.Id);

                // Return List of Copies
                List<TourLog> logs = new List<TourLog>();

                foreach (TourLog l in dbTour.TourLogs)
                {
                    logs.Add(new TourLog(l));
                }

                IBusinessLayer.logger.Debug($"Got all TourLogs from Tour {dbTour}.");
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
                throw new BusinessLayerException(e.Message);
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
                Tour dbTour = GetExistingTour(tour.Id);

                _DatabaseHandler.DeleteTour(dbTour);

                IBusinessLayer.logger.Debug($"Removed Tour {dbTour}.");
                return new Tour(dbTour);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Removing Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Removing Tour {tour} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
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

                TourLog dbTourLog = GetExistingTourLog(log.Id);

                _DatabaseHandler.DeleteTourLog(dbTourLog);

                IBusinessLayer.logger.Debug($"Removed TourLog {dbTourLog}.");
                return new TourLog(dbTourLog);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Removing TourLog {log} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Removing TourLog {log} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
            }
        }

        public Tour UpdateTour(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to update Tour {tour}.");
                //throw new BLLNotImplementedException("Update Tour");

                // check if tour exists
                Tour dbTour = GetExistingTour(tour.Id);

                _DatabaseHandler.UpdateTour(tour);

                dbTour = GetExistingTour(tour.Id);

                List<TourLog> temp_TourLogs = GetAllTourLogsOfTour(dbTour);
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

                IBusinessLayer.logger.Debug($"Updated Tour {dbTour}.");
                return new Tour(dbTour);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Updating Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Updating Tour {tour} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
            }
        }

        public TourLog UpdateTourLog(TourLog log)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to update TourLog {log}.");
                //throw new BLLNotImplementedException("Update Tour Log");

                TourLog dbLog = GetExistingTourLog(log.Id);

                _DatabaseHandler.UpdateTourLog(log);

                dbLog = GetExistingTourLog(log.Id);

                IBusinessLayer.logger.Debug($"Updated TourLog {dbLog}.");
                return new TourLog(dbLog);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Updating TourLog {log} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Updating TourLog {log} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
            }
        }

        public Tour PrintReportPDF(Tour tour)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to print report for Tour {tour}.");

                Tour dbTour = GetExistingTour(tour.Id);

                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(appDir, IBusinessLayer.ReportPath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = $"{dbTour.Id} - {DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss")} - {dbTour.Name}";
                string filePathAndName = System.IO.Path.Combine(filePath, $"{fileName}.pdf");
                int index = 1;

                while (File.Exists(filePathAndName))
                {
                    filePathAndName = System.IO.Path.Combine(filePath, $"{fileName} ({index}).pdf");
                    index++;
                }


                PdfWriter writer = new PdfWriter(filePathAndName);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Paragraph header = new Paragraph(fileName)
                    .SetFontSize(18)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                document.Add(header);

                document.Add(new Paragraph());

                Paragraph tourInfoHeader = new Paragraph("Tour Info")
                    .SetFontSize(14)
                    .SetBold();
                document.Add(tourInfoHeader);

                List tourInfo = new List()
                    .SetSymbolIndent(12)
                    .SetListSymbol("\u2022");
                tourInfo.Add(new ListItem("Id: " + tour.Id));
                tourInfo.Add(new ListItem("Name: " + tour.Name));
                tourInfo.Add(new ListItem("Description: " + tour.Description));
                tourInfo.Add(new ListItem("From: " + tour.From));
                tourInfo.Add(new ListItem("To: " + tour.To));
                tourInfo.Add(new ListItem("Transport Type: " + tour.TransportType.ToString()));
                tourInfo.Add(new ListItem("Distance: " + tour.DistanceString));
                tourInfo.Add(new ListItem("Time: " + tour.TimeString));
                tourInfo.Add(new ListItem("Tour Logs Count: " + tour.TourLogs.Count));
                tourInfo.Add(new ListItem("Popularity: " + tour.Popularity));
                tourInfo.Add(new ListItem("Child-Friendliness: " + tour.ChildFriendliness.ToString()));
                document.Add(tourInfo);

                document.Add(new Paragraph());

                Paragraph tableHeader = new Paragraph("Tour Logs")
                    .SetFontSize(14)
                    .SetBold();
                document.Add(tableHeader);

                int tourLogVarCount = 7;
                Table logTable = new Table(UnitValue.CreatePercentArray(tourLogVarCount))
                    .UseAllAvailableWidth()
                    .SetFontSize(12);

                logTable.AddHeaderCell("Id");
                logTable.AddHeaderCell("Date Time (UTC)");
                logTable.AddHeaderCell("Total Distance");
                logTable.AddHeaderCell("Total Time");
                logTable.AddHeaderCell("Difficulty");
                logTable.AddHeaderCell("Rating");
                logTable.AddHeaderCell("Comment");

                foreach (var log in dbTour.TourLogs)
                {
                    logTable.AddCell(log.Id.ToString());
                    logTable.AddCell(log.DateTime.ToString());
                    logTable.AddCell(ToStringHelpers.DistanceInMetersToString(log.TotalDistance));
                    logTable.AddCell(ToStringHelpers.DurationInSecondsToString(log.TotalTime));
                    logTable.AddCell(log.Difficulty.ToString());
                    logTable.AddCell(log.Rating.ToString());
                    logTable.AddCell(log.Comment);
                }

                document.Add(logTable);

                document.Add(new AreaBreak());

                Paragraph routeInfoHeader = new Paragraph("Detailed Route Information")
                    .SetFontSize(14)
                    .SetBold();
                document.Add(routeInfoHeader);

                Paragraph routeInfo = new Paragraph(tour.RouteInformation)
                    .SetFontSize(12);
                document.Add(routeInfo);


                document.Close();

                IBusinessLayer.logger.Debug($"Printed report for Tour {dbTour}.");
                return new Tour(dbTour);
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Printing report for Tour {tour} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Printing report for Tour {tour} failed: {e.ToString()}");
                throw new BusinessLayerException(e.Message);
            }
        }

        private Tour GetExistingTour(int id)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get Tour {id}.");

                Tour dbTour = _DatabaseHandler.GetTour(id);

                if (dbTour == null)
                {
                    throw new BLLConflictException("Tour does not exist");
                }
                IBusinessLayer.logger.Debug($"Got Tour {dbTour}.");
                return dbTour;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting Tour {id} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting Tour {id} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
            }
        }

        private TourLog GetExistingTourLog(int id)
        {
            try
            {
                IBusinessLayer.logger.Debug($"Trying to get TourLog {id}.");

                TourLog dbTourLog = _DatabaseHandler.GetTourLog(id);

                if (dbTourLog == null)
                {
                    throw new BLLConflictException("Tour Log does not exist");
                }

                IBusinessLayer.logger.Debug($"Got TourLog {dbTourLog}.");
                return dbTourLog;
            }
            catch (BusinessLayerException e)
            {
                IBusinessLayer.logger.Error($"Getting TourLog {id} failed: {e}");
                throw;
            }
            catch (Exception e)
            {
                IBusinessLayer.logger.Error($"Getting TourLog {id} failed: {e.Message}");
                throw new BusinessLayerException(e.Message);
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

        private async Task SeedData()
        {
            for (int i = 1; i < 4; i++)
            {
                Tour t = await AddTour(new Tour()
                {
                    Name = $"Name {i}",
                    Description = $"Desc {i}",
                    From = $"HTL Krems",
                    To = $"FH-Technikum Wien",
                    TransportType = (ETransportType)((i - 1) % 3)
                });
            }
        }
    }
}
