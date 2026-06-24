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
    class IdToProductName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            Product product = getProductById(id);
            return product.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private Product getProductById(int id)
        {
            ProductService service = new ProductService();
            Task<Product> product = Task.Run(() => service.GetProduct(id));
            return product.Result;
        }
    }
}
