using System.Windows;

using SafeboardChat.Bootstrapping;

namespace SafeboardChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            _bootstrapper = bootstrapper;
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _bootstrapper.Container.Dispose();
        }
        private Bootstrapper _bootstrapper;

    }
}
