using Hardcodet.Wpf.TaskbarNotification;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Runnr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ILog logger;
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            logger = StaticLogger.GetLogger();
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            logger.Info("Starting up the application");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            logger.Info("Exiting application");
            notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
