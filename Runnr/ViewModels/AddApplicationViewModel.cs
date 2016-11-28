using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Runnr
{
    public class AddApplicationViewModel : BaseViewModel
    {
        public RunnrViewModel ParentViewModel { get; set; }

        private string applicationName;

        public string ApplicationName
        {
            get { return applicationName; }
            set
            {
                applicationName = value;
                OnPropertyChanged();

            }
        }

        private string applicationPath;

        public string ApplicationPath
        {
            get { return applicationPath; }
            set
            {
                applicationPath = value;
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

        public AddApplicationViewModel(RunnrViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            AddApplicationCommand = new DelegateCommand(CanAddApplication, OnAddApplication);
            BrowseApplicationCommand = new DelegateCommand(OnBrowseApplication);
        }
        public ICommand AddApplicationCommand { get; set; }

        public ICommand BrowseApplicationCommand { get; set; }

        private void OnAddApplication(object notUsed)
        {
            try
            {
                if (ParentViewModel == null || ParentViewModel.Applications == null)
                {
                    logger.LogError("Invalid application");
                    return;
                }
                ParentViewModel.Applications.Add(new ApplicationDetail()
                {
                    Name = ApplicationName,
                    ApplicationPath = ApplicationPath,
                    Parameters = Parameters
                });

                JsonService.Save(ParentViewModel.Applications.ToList<ApplicationDetail>());
                ClearApplication();
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        private void ClearApplication()
        {
            ApplicationName = string.Empty;
            ApplicationPath = string.Empty;
            Parameters = string.Empty;
        }

        private void OnBrowseApplication(object notUsed)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                var result = fileDialog.ShowDialog();
                if (result == true)
                {
                    ApplicationPath = fileDialog.FileName;
                    if (string.IsNullOrEmpty(ApplicationName))
                    {
                        ApplicationName = fileDialog.SafeFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }

        }

        public bool CanAddApplication()
        {
            return !string.IsNullOrEmpty(ApplicationName) && !string.IsNullOrEmpty(ApplicationPath);
        }
    }
}
