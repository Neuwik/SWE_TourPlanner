using System.Globalization;
using System.Windows.Data;
using System;

namespace SWE_TourPlanner_WPF
{
    public enum ETransportType
    {
        Car = 0,
        Bike = 1,
        Foot = 2
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