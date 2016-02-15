using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SafeboardChat.ViewModels;

namespace SafeboardChat.Views
{
    /// <summary>
    /// Interaction logic for ChatChooser.xaml
    /// </summary>
    public partial class ChatChooser : UserControl
    {
        private ChatChooserViewModel viewModel;
         public ChatChooserViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                DataContext = viewModel = value;
            }
        }
        public ChatChooser()
        {
            System.Diagnostics.Debug.WriteLine("ChatChooser: " + Dispatcher.GetHashCode());
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("ChatChooser Thread:"+Dispatcher.Thread.GetHashCode());
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            /*
            var selected = ListView.SelectedItem as ChatViewModel;
            if (selected != null)
            {
                viewModel.ShowChat(selected);
            }
            */
        }
    }
}
