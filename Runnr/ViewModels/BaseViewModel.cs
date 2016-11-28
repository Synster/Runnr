using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Runnr
{
    public class BaseViewModel:INotifyPropertyChanged
    {
        public ILogger logger;
        public BaseViewModel()
        {
            logger = new Logger();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
