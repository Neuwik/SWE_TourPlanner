using System.Globalization;
using System.Windows.Data;
using System;
using System.Collections.Generic;

namespace SWE_TourPlanner_WPF
{
    public enum ETransportType
    {
        Foot = 1,
        Bike = 2,
        Car = 3
    }

    public class ETransportTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ETransportType)value == (ETransportType)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) ? parameter : null;
        }
    }
}