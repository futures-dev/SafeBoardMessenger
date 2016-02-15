using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SafeboardChat.Views.Converters
{
    public class SideHorizontalAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string sender = values[0] as string;
            string chatHost = values[1] as string;
            if (sender != null && chatHost != null)
                return sender == chatHost ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            else
                return HorizontalAlignment.Center;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
