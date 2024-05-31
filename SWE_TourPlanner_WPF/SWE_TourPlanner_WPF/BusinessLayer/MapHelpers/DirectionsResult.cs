using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer.MapHelpers
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
            public List<Segment> Segments { get; set; }

            public override string ToString()
            {
                string distance = $"{Math.Truncate((Summary.Distance * 10)) / 10}m";
                if (Summary.Distance > 1000)
                {
                    distance = $"{Math.Truncate((Summary.Distance / 1000 * 10)) / 10}km";
                }

                string ret = $"Total Distance: {distance}";

                TimeSpan t = TimeSpan.FromSeconds(Summary.Duration);

                string duration = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds
                                );
                ret += $"\nTotal Time: {duration}";

                if (Segments != null)
                {
                    ret += $"\nSegments: {Segments.Count}";
                    for (int i = 0; i < Segments.Count; i++)
                    {
                        ret += $"\n\nSegment {(i+1)}: {Segments[i].ToString()}";
                    }
                }
                return ret;
            }
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
        public class Segment
        {
            public double Distance { get; set; }
            public double Duration { get; set; }
            public List<Step> Steps { get; set; }

            public override string ToString()
            {
                string distance = $"{Math.Truncate((Distance * 10)) / 10}m";
                if (Distance > 1000)
                {
                    distance = $"{Math.Truncate((Distance / 1000 * 10)) / 10}km";
                }

                string ret = $"Distance: {distance}";

                TimeSpan t = TimeSpan.FromSeconds(Duration);

                string duration = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds
                                );
                ret += $", Time: {duration}";

                if (Steps != null)
                {
                    //ret += $"\nSteps: ";
                    for (int i = 0; i < Steps.Count; i++)
                    {
                        ret += $"\n{(i + 1)}) {Steps[i].ToString()}";
                    }
                }
                return ret;
            }
        }

        public class Step
        {
            public string Instruction { get; set; }
            public double Distance { get; set; }
            public double Duration { get; set; }

            public override string ToString()
            {
                return $"In {Distance}m ({Duration}min) {Instruction}";
            }
        }
    }
}
