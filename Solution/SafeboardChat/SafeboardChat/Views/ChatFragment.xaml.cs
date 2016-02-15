using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MessengerCLR.Enums;
using Microsoft.Win32;
using SafeboardChat.ViewModels;

namespace SafeboardChat.Views
{
    /// <summary>
    /// Interaction logic for ChatFragment.xaml
    /// </summary>
    public partial class ChatFragment : UserControl, INotifyPropertyChanged
    {
        public ChatViewModel ViewModel
        {
            get
            {
                return (ChatViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }


        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel",
                                    typeof(ChatViewModel),
                                    typeof(ChatFragment),
                                    new FrameworkPropertyMetadata(default(ChatViewModel), FrameworkPropertyMetadataOptions.AffectsRender, ViewModelPropertyChanged));
        private static void ViewModelPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var fragment = source as ChatFragment;
            if (fragment != null)
            {
                fragment.OnPropertyChanged("ViewModel");
            }
        }


        public ChatFragment()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("ChatFragment: " + Dispatcher.GetHashCode());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            System.Diagnostics.Debug.WriteLine("View Model CHANGED" + ViewModel);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object MessageContent
        {
            get
            {
                if (Attachment == null)
                {
                    return new object[]
                    {
                        InputTextBox.Text, message_content_type.Text
                    };
                }
                else
                {
                    var temp = Attachment;
                    Attachment = null;
                    return temp;
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("MessageContent");
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Attachment = new object[]
                    {
                        File.ReadAllBytes(dialog.FileName),
                        // AWFUL CHECK
                        dialog.FileName.Substring(dialog.FileName.LastIndexOf('.')).ToLower() == ".mp4"
                            ? message_content_type.Video
                            : message_content_type.Image
                    };
                }
                catch
                {
                    MessageBox.Show("Error while reading file.");
                }
            }
        }

        private object _attachment;
        public object Attachment
        {
            get { return _attachment; }
            private set
            {
                _attachment = value;
                OnPropertyChanged();
            }
        }
    }
}
