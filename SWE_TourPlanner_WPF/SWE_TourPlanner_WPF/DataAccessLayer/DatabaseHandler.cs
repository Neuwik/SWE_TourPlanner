using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SWE_TourPlanner_WPF.Models;

namespace SWE_TourPlanner_WPF.DataAccessLayer
{
    public class DatabaseHandler : IDisposable
    {
        private string _ConnectionString; 
        private DatabaseContext _DatabaseContext;
        private TourRepository _TourRepo;
        private TourLogRepository _TourLogRepo;
        private IServiceScope _Scope;
        private IHost _Host;

        public DatabaseHandler()
        {
            try
            {
                string json = File.ReadAllText(TourPlannerConfig.ConfigFile);
                TourPlannerConfig config = JsonConvert.DeserializeObject<TourPlannerConfig>(json);
                _ConnectionString = config.DataAccessLayer.ConnectionString;
            }
            catch (Exception)
            {
                throw new Exception("Database Handler could not read config file.");
            }

            _Host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DatabaseContext>(options =>
                    options.UseNpgsql(_ConnectionString));
            })
            .Build();

            _Host.Start();
            _Scope = _Host.Services.CreateScope();
            _DatabaseContext = _Scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _DatabaseContext.Database.EnsureCreated();

            _TourRepo = new TourRepository(_DatabaseContext);
            _TourLogRepo = new TourLogRepository(_DatabaseContext);
        }

        public DatabaseHandler(DatabaseContext databaseContext)
        {
            // For UnitTests ONLY
            _DatabaseContext = databaseContext;

            _TourRepo = new TourRepository(_DatabaseContext);
            _TourLogRepo = new TourLogRepository(_DatabaseContext);
        }

        public DatabaseHandler(DatabaseContext databaseContext, TourRepository tourRepo, TourLogRepository tourLogRepo)
        {
            // For UnitTests ONLY
            _DatabaseContext = databaseContext;
            _TourRepo = tourRepo;
            _TourLogRepo = tourLogRepo;
        }

        public void Dispose()
        {
            _Scope?.Dispose();
            _Host?.StopAsync().GetAwaiter().GetResult();
            _Host?.Dispose();
        }

        // READ Functions:
        public List<Tour> GetAllTours()
        {
            return _TourRepo.GetAllEager();
        }
        public Tour GetTour(int TourId)
        {
            return _TourRepo.GetOneEager(TourId);
        }
        public TourLog GetTourLog(int TourLogId)
        {
            return _TourLogRepo.GetOne(TourLogId);
        }
        public List<TourLog> GetTourLogs(int TourId)
        {
            return _TourLogRepo.GetAll().Where(t => t.TourId == TourId).ToList();
        }
        public Tour GetLastTour()
        {
            return _TourRepo.GetLast();
        }
        public TourLog GetLastTourLog()
        {
            return _TourLogRepo.GetLast();
        }

        // WRITE Functions:
        public int AddTour(Tour tour)
        {
            tour.Id = 0;

            List<TourLog> logs = tour.TourLogs.ToList();
            tour.TourLogs = new List<TourLog>();

            if (_TourRepo.Add(tour) <= 0)
            {
                return 0;
            }
            
            if (logs.Count > 0)
            {
                foreach (TourLog tourLog in logs)
                {
                    tourLog.TourId = tour.Id;
                    AddTourLog(tourLog);
                }
            }
            tour.TourLogs = logs;

            return 1;
        }

        public int AddTourLog(TourLog tourLog)
        {
            tourLog.Id = 0;
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
    }
}
