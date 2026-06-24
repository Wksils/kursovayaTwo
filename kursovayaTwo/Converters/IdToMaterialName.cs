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
    class IdToMaterialName : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            RawMaterial material = getMaterialById(id);
            return material.Name;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private RawMaterial getMaterialById(int id)
        {
            BatchService service = new BatchService();
            Task<RawMaterial> material = Task.Run(() => service.GetMaterial(id));
            return material.Result;
        }
    }
}
