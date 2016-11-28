using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Runnr
{
   public class ApplicationDetail : BaseViewModel
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string path;

        public string ApplicationPath
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged();
            }
        }

        private string parameters;

        public string Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                OnPropertyChanged();
            }
        }

        
    }
}
