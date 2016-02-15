using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MessengerCLR.Enums;
using SafeboardChat.Models;
using SafeboardChat.ViewModels;

namespace SafeboardChat.Views.Converters
{
    public class MessageContentSelector: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var message = item as MessageViewModel;

            if (element != null && message != null)
            {
                switch (message.ContentType)
                {
                    case message_content_type.Text:
                        return element.FindResource("TextContentTemplate") as DataTemplate;
                    case message_content_type.Image:
                        return element.FindResource("ImageContentTemplate") as DataTemplate;
                    case message_content_type.Video:
                        return element.FindResource("VideoContentTemplate") as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
