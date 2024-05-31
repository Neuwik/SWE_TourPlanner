using log4net;
using SWE_TourPlanner_WPF.BusinessLayer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public interface IBusinessLayer
    {
        protected static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

        protected const string ApiKey = "5b3ce3597851110001cf62483a397f95f86441adb7cbf0789ae0d615";

        protected static IBusinessLayer? _instance;
        public static IBusinessLayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BusinessLayer();
                }
                return _instance;
            }
        }
        public Task<Tour> AddTour(Tour tour);
        public Tour UpdateTour(Tour tour);
        public Tour RemoveTour(Tour tour);
        public TourLog AddTourLogToTour(Tour tour, TourLog log);
        public TourLog UpdateTourLog(TourLog log);
        public TourLog RemoveTourLog(TourLog log);
        public Task<List<Tour>> GetAllTours();
        public List<TourLog> GetAllTourLogsOfTour(Tour tour);
    }
}
