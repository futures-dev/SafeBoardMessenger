using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SafeboardChat.ViewModels;

namespace SafeboardChat.Views.Converters
{
    /*
    [ValueConversion(typeof(ChatViewModel), typeof(Chat))]
    class ChatViewModelConverter :IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
            {
                return new Chat((ChatViewModel) value);
            }

            public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
            {
            // wrong
                return new ChatViewModel();
            }

        }
        */
}
