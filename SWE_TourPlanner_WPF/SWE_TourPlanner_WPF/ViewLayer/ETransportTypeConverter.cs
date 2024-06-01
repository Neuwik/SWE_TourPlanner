using SWE_TourPlanner_WPF.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SWE_TourPlanner_WPF.ViewLayer
{
    public class ETransportTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ETransportType)value == (ETransportType)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? parameter : null;
        }
    }
}
