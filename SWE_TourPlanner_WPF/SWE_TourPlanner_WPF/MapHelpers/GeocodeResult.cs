using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.MapHelpers
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

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
