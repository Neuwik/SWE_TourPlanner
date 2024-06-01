using SWE_TourPlanner_WPF.BusinessLayer.Logging;
using SWE_TourPlanner_WPF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public interface IBusinessLayer
    {
        protected static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

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
        public List<Tour> GetAllTours();
        public List<TourLog> GetAllTourLogsOfTour(Tour tour);
        public Tour PrintReportPDF(Tour tour);
        public Tour PrintSummarizedReportPDF(Tour tour);
        public Tour ExportTourToJson(Tour tour);
        public Task<List<Tour>> ImportToursFromJson();
    }
}
