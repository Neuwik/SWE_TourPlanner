using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.DirectoryServices;
using System.Diagnostics;
using System.Net;

namespace SWE_TourPlanner_WPF.DataBase
{
    class DatabaseHandler
    {
        private static readonly string _ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GitHub\SWE_TourPlanner\SWE_TourPlanner_WPF\SWE_TourPlanner_WPF\DataBase\TourPlannerDB.mdf;Integrated Security = True";
        
        private SqlConnection _DatabaseConnection;
        private object _DATABASELOCK;

        private SqlDataAdapter _SqlDataAdapter;
        private SqlCommand _Command;

        public DatabaseHandler()
        {
            lock (_DATABASELOCK)
            {
                _DatabaseConnection = new SqlConnection(_ConnectionString);
            }
            SeedData();
        }
        private string ExecuteSQLQuery(string sqlCommand, Dictionary<string, string> queryParameterD)
        {
            lock (_DATABASELOCK)
            {
                try
                {
                    _DatabaseConnection.Open();
                    FillSQLCommandWithParameters(sqlCommand, queryParameterD);
                    _SqlDataAdapter = new SqlDataAdapter(_Command);
                }
                catch (Exception ex) 
                {
                    Debug.WriteLine(ex);
                    return "Query Error";
                }
                _DatabaseConnection.Close();
                return "";
            }
        }
        private void FillSQLCommandWithParameters(string sqlCommand, Dictionary<string, string> queryParameterD)
        {
            _Command = new SqlCommand(sqlCommand, _DatabaseConnection);
            if (sqlCommand.Contains('@'))
            {
                foreach (var parameter in queryParameterD)
                {
                    if (sqlCommand.Contains($"@{parameter.Key}"))
                    {
                        _Command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
            }
        }

        // READ Functions:
        public string GetTour(int TourId)
        {
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return "";
        }
        public List<string> GetTourLogs(int TourId)
        {
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return new List<string>();
        }

        // WRITE Functions:
        public string AddTour(Tour tour)
        {
            ConvertTourToParams(tour, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"INSERT INTO [dbo].[Tour] (
                    [Name],
                    [Description],
                    [FromLocation],
                    [ToLocation],
                    [TransportType],
                    [Distance],
                    [Time],
                    [RouteInformation],
                    [ImagePath]
                  ) VALUES (
                    @Name,
                    @Description,
                    @FromLocation,
                    @ToLocation,
                    @TransportType,
                    @Distance,
                    @Time,
                    @RouteInformation,
                    @ImagePath
                  );", 
                queryParameterD
            );

            if(tour.TourLogs.Count > 0)
            {
                foreach (TourLog tourLog in tour.TourLogs)
                {
                    AddTourLog(tourLog);
                }
            }
            
            return "";
        }

        public string AddTourLog(TourLog tourLog)
        {
            ConvertTourLogParamsD(tourLog, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"INSERT INTO [dbo].[TourLog] (
                    [TourId],
                    [DateTime],
                    [Comment],
                    [Difficulty],
                    [TotalDistance],
                    [TotalTime],
                    [Rating]
                  ) VALUES (
                    @TourId,
                    @DateTime,
                    @Comment,
                    @Difficulty,
                    @TotalDistance,
                    @TotalTime,
                    @Rating
                  );", 
                queryParameterD
            );
            return "";
        }

        // UPDATE functions:
        public string UpdateTour(Tour tour)
        {
            ConvertTourToParams(tour, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"UPDATE [dbo].[Tour] 
                  SET 
                    [Name] = @Name, 
                    [Description] = @Description, 
                    [FromLocation] = @FromLocation, 
                    [ToLocation] = @ToLocation, 
                    [TransportType] = @TransportType, 
                    [Distance] = @Distance,
                    [Time] = @Time,   
                    [RouteInformation] = @RouteInformation,
                    [ImagePath] = @ImagePath
                  WHERE 
                    [TourId] = @TourId;", 
                queryParameterD
            );
            return "";
        }

        public string UpdateTourLog(TourLog tourLog)
        {
            ConvertTourLogParamsD(tourLog, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"UPDATE [dbo].[TourLog]
                  SET 
                    [TourId] = @TourId,
                    [DateTime] = @DateTime,
                    [Comment] = @Comment,
                    [Difficulty] = @Difficulty,
                    [TotalDistance] = @TotalDistance,
                    [TotalTime] = @TotalTime,
                    [Rating] = @Rating
                  WHERE 
                    [TourLogId] = @TourLogId;", 
                queryParameterD
            );
            return "";
        }

        // DELETE Functions:
        public bool DeleteTour(Tour tour)
        {
            ConvertTourToParams(tour, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"DELETE FROM [dbo].[Tour] 
                  WHERE [TourId] = @TourId;", 
                queryParameterD
            );
            return true;
        }
        public bool DeleteTourLog(TourLog tourLog)
        {
            ConvertTourLogParamsD(tourLog, out Dictionary<string, string> queryParameterD);
            ExecuteSQLQuery(
                @"DELETE FROM [dbo].[TourLog] 
                  WHERE [TourLogId] = @TourId;", 
                queryParameterD
            );
            return true;
        }

        // HELPER Functions:
        private void ConvertTourToParams(Tour tour, out Dictionary<string, string> queryParameterD)
        {
            queryParameterD = new Dictionary<string, string>();
            queryParameterD.Add("TourId", /*tourID*/"");
            queryParameterD.Add("Name", tour.Name);
            queryParameterD.Add("Description", tour.Description);
            queryParameterD.Add("FromLocation", tour.From);
            queryParameterD.Add("ToLocation", tour.To);
            queryParameterD.Add("TransportType", "" + (int) tour.TransportType);
            queryParameterD.Add("Distance", "" + tour.Distance);
            queryParameterD.Add("Time", "" + tour.Time);
            queryParameterD.Add("RouteInformation", tour.RouteInformation);
            queryParameterD.Add("ImagePath", tour.ImagePath);
        }
        private void ConvertTourLogParamsD(TourLog tourLog, out Dictionary<string, string> queryParameterD)
        {
            queryParameterD = new Dictionary<string, string>();
            queryParameterD.Add("TourLogId", /*tourLog.Name*/ "");
            queryParameterD.Add("TourId", /*tourID*/"");
            queryParameterD.Add("DateTime", tourLog.DateTime.ToString());
            queryParameterD.Add("Comment", tourLog.Comment);
            queryParameterD.Add("Difficulty", "" + (int) tourLog.Difficulty);
            queryParameterD.Add("TotalDistance", "" + tourLog.TotalDistance);
            queryParameterD.Add("TotalTime", "" + tourLog.TotalTime);
            queryParameterD.Add("Rating", "" + (int) tourLog.Rating);
        }
        private void SeedData()
        {
            for (int i = 1; i < 6; i++)
            {
                Tour selectedTour = new Tour()
                {
                    Name = $"Name {i}",
                    Description = $"Desc {i}",
                    From = $"From {i}",
                    To = $"To {i}",
                    TransportType = ETransportType.Car,
                    Distance = i * 100,
                    Time = i * 30,
                    RouteInformation = $"Info {i}"
                };
                AddTour(selectedTour);
            }
        }
    }
}
