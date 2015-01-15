using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_WPF.Master
{
    public class Bootstrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }


        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }

        #region Logging implementation
        private readonly NLogLogger _logger = new NLogLogger();

        protected override ILoggerFacade CreateLogger()
        {
            return _logger;
        }
        #endregion


    }
}
