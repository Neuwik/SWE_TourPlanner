using System;

namespace SWE_TourPlanner_WPF.Models
{
    public static class ToStringHelpers
    {
        public static string DistanceInMetersToString(double distanceInMeters)
        {
            string distance = $"{Math.Truncate(distanceInMeters * 10) / 10}m";
            if (distanceInMeters > 1000)
            {
                distance = $"{Math.Truncate(distanceInMeters / 1000 * 10) / 10}km";
            }
            return distance;
        }

        public static string DurationInSecondsToString(double durationInSeconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(durationInSeconds);

            string duration = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            (int)t.TotalHours,
                            t.Minutes,
                            t.Seconds
                            );
            return duration;
        }
    }
}
