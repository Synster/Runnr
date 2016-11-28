using System;
using System.Diagnostics;

namespace Runnr
{
   public static class ApplicationLauncher
    {
        public static void Launch(ApplicationDetail appDetail)
        {
            try
            {
                Process.Start(appDetail.ApplicationPath, appDetail.Parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to launch application" + appDetail.Name + ex.Message);
                throw;
            }
        }
    }
}
