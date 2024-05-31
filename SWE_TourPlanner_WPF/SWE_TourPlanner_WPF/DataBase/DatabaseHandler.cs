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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SWE_TourPlanner_WPF.DataBase
{
    class DatabaseHandler
    {
        private string _ConnectionString = "Host=localhost;Port=5432;Database=SWE_TourPlanner_DB;Username=SWE_TourPlanner_User;Password=Debian123!;"; 
        private DatabaseContext _DatabaseContext;
        private TourRepository _TourRepo;
        private TourLogRepository _TourLogRepo;
        
        public DatabaseHandler()
        {
            //var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            //_DatabaseContext = new DatabaseContext(options);
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(_ConnectionString);
            _DatabaseContext = new DatabaseContext(optionsBuilder.Options);
            _DatabaseContext.Database.Migrate();
            _TourRepo = new TourRepository(_DatabaseContext);
            _TourLogRepo = new TourLogRepository(_DatabaseContext);

            SeedData();
        }
        
        // READ Functions:
        public Tour GetTour(int TourId)
        {
            return _TourRepo.GetOne(TourId);
        }
        public List<TourLog> GetTourLogs(int TourId)
        {
            return _TourLogRepo.GetAll().Where(t => t.TourId == TourId).ToList();
        }

        // WRITE Functions:
        public int AddTour(Tour tour)
        {
            
            if(_TourRepo.Add(tour) <= 0)
            {
                return 0;
            }
            
            if (tour.TourLogs.Count > 0)
            {
                foreach (TourLog tourLog in tour.TourLogs)
                {
                    AddTourLog(tourLog);
                }
            }
            return 1;
        }

        public int AddTourLog(TourLog tourLog)
        {
            return _TourLogRepo.Add(tourLog);
        }

        // UPDATE functions:
        public int UpdateTour(Tour tour)
        {
            return _TourRepo.Update(tour);
        }

        public int UpdateTourLog(TourLog tourLog)
        {
            return _TourLogRepo.Update(tourLog);
        }

        // DELETE Functions:
        public int DeleteTour(Tour tour)
        {
            return _TourRepo.Delete(tour); 
        }
        public int DeleteTourLog(TourLog tourLog)
        {
            return _TourLogRepo.Delete(tourLog); 
        }

        // HELPER Functions:
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
