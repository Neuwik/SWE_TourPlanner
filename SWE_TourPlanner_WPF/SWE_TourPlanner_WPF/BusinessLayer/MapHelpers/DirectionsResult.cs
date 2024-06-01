using System.Collections.Generic;
using SWE_TourPlanner_WPF.Models;

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
                string ret = $"Total Distance: {ToStringHelpers.DistanceInMetersToString(Summary.Distance)}";

                ret += $"\nTotal Time: {ToStringHelpers.DurationInSecondsToString(Summary.Duration)}";

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
                string ret = $"Distance: {ToStringHelpers.DistanceInMetersToString(Distance)}";

                ret += $", Time: {ToStringHelpers.DurationInSecondsToString(Duration)}";

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
                string ret = $"In {ToStringHelpers.DistanceInMetersToString(Distance)}";

                ret += $" ({ToStringHelpers.DurationInSecondsToString(Duration)}) {Instruction}";

                return ret;
            }
        }
    }
}
