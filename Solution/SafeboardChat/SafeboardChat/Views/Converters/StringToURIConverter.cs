using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SafeboardChat.Views.Converters
{
    [ValueConversion(typeof(string), typeof(Uri))]
    public class StringToURIConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string v = (string) value;
            v = v.Replace(".png", ".mp4");
            var u = new Uri(v);
            Debug.WriteLine(u);
            return u;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

    }
}
