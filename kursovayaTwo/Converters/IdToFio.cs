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
    public class IdToFio : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "—";
            int id = (int)value;
            User user = getUserById(id);
            return user?.FullName ?? "—";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private User getUserById(int id)
        {
            GetListsService service = new GetListsService();
            Task<User> user = Task.Run(() => service.GetUser(id));
            return user.Result;
        }
    }
}
