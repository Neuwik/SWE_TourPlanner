using System.Collections.Generic;

namespace SWE_TourPlanner_WPF.BusinessLayer.MapHelpers
{
    public class GeocodeResult
    {
        public List<GeocodeFeature> Features { get; set; }

        public class GeocodeFeature
        {
            public Geometry Geometry { get; set; }
        }

        public class Geometry
        {
            public List<double> Coordinates { get; set; }
        }
    }
}
