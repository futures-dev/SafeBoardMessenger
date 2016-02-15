using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Unity;
using SafeboardChat.Models;
using SafeboardChat.ViewModels;
using SafeboardChat.Views;
using SafeboardChat = SafeboardChat.Models.SafeboardChat;

namespace SafeboardChat.Bootstrapping
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<LauncherWindow>();
            shell.Show();
            return shell;
        }
        private void Navigator(Type type)
        {
            if (type.Equals(typeof(MainWindow)))
            {
                Container.Resolve<MainWindow>().Show();
                return;
            }
            if (type.Equals(typeof(SettingsDialog)))
            {
                var vm = Container.Resolve<SettingsViewModel>();
                var settings = Container.Resolve<SettingsDialog>();
                if (settings.ShowDialog() == true)
                {
                    var s = Container.Resolve<ISettings>();
                    s.ServerPort = vm.ServerPort;
                    s.ServerURL = vm.ServerURL;
                }
                return;
            }
            if (type.Equals(typeof(AboutWindow))){
                Container.Resolve<AboutWindow>().Show();
                return;
            }
        }

        private void UpdateSettings()
        {
            var s = Container.Resolve<ISettings>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            var settings = new SafeboardSettings("127.0.0.1", 5222);
            var securityPolicy = new PublicKeySecurityPolicy(0, "");
            var settingsViewModel = new SettingsViewModel();
            Container.RegisterInstance<SettingsViewModel>(settingsViewModel, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<ISettings>(settings, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IPublicKeySecurityPolicy>(securityPolicy, new ContainerControlledLifetimeManager());

            Container.RegisterType<SettingsDialog>(new InjectionFactory(c=>new SettingsDialog(c.Resolve<SettingsViewModel>())));
            Container.RegisterType<AboutWindow>(new InjectionFactory(c=>new AboutWindow()));


            Container.RegisterType<IMessenger, SafeboardMessenger>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(
                    c => new SafeboardMessenger(c.Resolve<ISettings>(), c.Resolve<IPublicKeySecurityPolicy>())));
            Container.RegisterType<ChatChooserViewModel>(
                new InjectionFactory(
                    c => new ChatChooserViewModel(c.Resolve<IMessenger>())));

            Container.RegisterType<MainWindowViewModel>(
                new InjectionFactory(
                c => new MainWindowViewModel(
                    c.Resolve<ChatChooserViewModel>(), 
                    c.Resolve<IMessenger>().UserId + " @ ")));
            Container.RegisterType<MainWindow>(
                new InjectionFactory(
                    c => new MainWindow(
                        c.Resolve<MainWindowViewModel>())));
                        Container.RegisterType<LauncherViewModel>(
                new InjectionFactory(
                    c => new LauncherViewModel(
                        c.Resolve<IMessenger>())));
                        Container.RegisterType<LauncherWindow>(
                new InjectionFactory(
                    c => new LauncherWindow(
                        c.Resolve<LauncherViewModel>(),
                        Navigator)));

            /*
            Container.RegisterType<IChat, Models.SafeboardChat>();
            Container.RegisterType<IMessage, SafeboardMessage>();
            Container.RegisterType<IMessageContent, SafeboardMessageContent>();
            Container.RegisterType<IPublicKeySecurityPolicy, PublicKeySecurityPolicy>();
            */
        }

    }
}
