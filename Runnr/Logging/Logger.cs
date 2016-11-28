using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnr
{
    public class Logger : ILogger
    {
       static ILog logger;

         static Logger()
        {
            logger = StaticLogger.GetLogger();
        }

        public void LogError(string error)
        {
            logger.Error(error);
        }

        public void LogException(Exception exception)
        {
            logger.Error(exception.Message);
        }

        public void LogMessage(string message)
        {
            logger.Error(message);
        }
    }
}
