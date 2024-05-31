using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.MapHelpers
{
    public class DirectionsResult
    {
        public List<Feature> Features { get; set; }

        public class Feature
        {
            public Geometry Geometry { get; set; }
            public Properties Properties { get; set; }
        }

        public class Properties
        {
            public Summary Summary { get; set; }
        }
        public class Summary
        {
            public double Distance { get; set; }
            public double Duration { get; set; }
        }

        public class Geometry
        {
            public List<List<double>> Coordinates { get; set; }
        }
    }
}
