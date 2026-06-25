using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace kursovayaTwo.Converters
{
    public class StringToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            if (str == "quarantine") return Brushes.Blue;
            else if (str == "consumed") return Brushes.Aqua;
            else if (str == "approved") return Brushes.Green;
            else if (str == "rejected") return Brushes.Red;
            else return Brushes.Orange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
