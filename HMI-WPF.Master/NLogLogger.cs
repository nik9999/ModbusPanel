using Microsoft.Practices.Prism.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_WPF.Master
{
    public class NLogLogger : ILoggerFacade
    {
        public NLogLogger()
        {
            var config = new LoggingConfiguration();

            var target = new FileTarget()
            {
                FileName = @"${basedir}\logs\" + typeof(App).FullName.Replace(".App","_") + "${date:format=yyyy.MM.dd}.log", 
                CreateDirs = true,
                Layout = "${longdate}|${level}|${message}",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveEvery = FileArchivePeriod.Day,
            };

            config.AddTarget("logfile", target);
            var rule = new LoggingRule("*", LogLevel.Debug, target);

            config.LoggingRules.Add(rule); 

            LogManager.Configuration = config;

        }

        private Logger logger = LogManager.GetCurrentClassLogger();

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    logger.Debug(message);
                    break;
                case Category.Warn:
                    logger.Warn(message);
                    break;
                case Category.Exception:
                    logger.Error(message);
                    break;
                case Category.Info:
                    logger.Info(message);
                    break;
            }
        }
    }
}
