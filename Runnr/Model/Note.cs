using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnr.Model
{
    public class Note : BaseViewModel
    {
        private string noteText;

        public string NoteText
        {
            get { return noteText; }
            set
            {
                noteText = value;
                OnPropertyChanged();
            }
        }

    }
}
