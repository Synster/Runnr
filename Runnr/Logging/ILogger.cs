using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnr
{
    public interface ILogger
    {
        void LogError(string error);
        void LogException(Exception exception);
        void LogMessage(string message);
    }
}
