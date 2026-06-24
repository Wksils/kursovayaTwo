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
    public class DateToNumber : IValueConverter
    {
        private static List<ProductionBatch>? _cachedBatches = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string date = value.ToString()!;
            ProductionBatch production = getProductions(date);
            return production == null? "---": production.BatchNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private ProductionBatch getProductions(string date)
        {
            if(_cachedBatches == null)
            {
                GetListsService listsService = new GetListsService();
                _cachedBatches = Task.Run(() => listsService.getBatches()).Result;
            }
            return _cachedBatches.FirstOrDefault(n => n.CreatedAt.ToShortDateString() == date)!;
        }
    }
}
