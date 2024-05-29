using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.DirectoryServices;
using System.Diagnostics;

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
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return "";
        }
        public string AddTourLog(TourLog tourLog)
        {
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return "";
        }

        // ALTER functions:
        public string AlterTour(Tour tour)
        {
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return "";
        }

        public string AlterTourLog(TourLog tourLog)
        {
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            ExecuteSQLQuery("", queryParameterD);
            return "";
        }

        // DELETE Functions:
        public bool DeleteTour(int TourId)
        {
            string idAsString = TourId.ToString();
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            queryParameterD.Add("TourId", idAsString);
            ExecuteSQLQuery("", queryParameterD);
            return true;
        }
        public bool DeleteTourLog(int TourLogId)
        {
            string idAsString = TourLogId.ToString();
            Dictionary<string, string> queryParameterD = new Dictionary<string, string>();
            queryParameterD.Add("TourLogId", idAsString);
            ExecuteSQLQuery("", queryParameterD);
            return true;
        }
    }
}
