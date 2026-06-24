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
    public class BatchIdToLastTestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int batchId) return "—";
            var service = new GetListsService();
            var tests = Task.Run(() => service.GetLabTests()).Result;
            var last = tests.Where(t => t.MatBatchId == batchId)
                             .OrderByDescending(t => t.CreatedAt)
                             .FirstOrDefault();
            return last == null ? "—" : $"{last.CreatedAt:dd.MM.yyyy} ({last.Status})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
