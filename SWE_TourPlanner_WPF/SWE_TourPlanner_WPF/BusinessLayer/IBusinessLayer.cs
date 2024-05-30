using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    public interface IBusinessLayer
    {
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
        public Tour AddTour(Tour tour);
        public Tour UpdateTour(Tour tour);
        public Tour RemoveTour(Tour tour);
        public TourLog AddTourLog(TourLog log);
        public TourLog UpdateTourLog(TourLog log);
        public TourLog RemoveTourLog(TourLog log);
        public List<Tour> GetAllTours();
        public List<TourLog> GetAllToursLogOfTour(Tour tour);
    }
}
