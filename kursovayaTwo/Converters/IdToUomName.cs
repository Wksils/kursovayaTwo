using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace kursovayaTwo.Converters
{
    public class IdToUomName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            UnitsOfMeasure uom = getUomById(id);
            return uom.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private UnitsOfMeasure getUomById(int id)
        {
            GetListsService service = new GetListsService();
            Task<UnitsOfMeasure> uom = Task.Run(() => service.GetUom(id));
            return uom.Result;
        }
    }
}
