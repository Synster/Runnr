using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using log4net;
using Runnr.Model;

namespace Runnr
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    public class RunnrViewModel : BaseViewModel
    {
        private bool canAddNewApplication = true;

        public RunnrViewModel()
        {
            InitializeApplications();
            InitializeNotes();
            EnterKeyCommand = new DelegateCommand(AddNewNote);
            LaunchSelectedApplicationCommand = new DelegateCommand(OnLaunchSelectedApplication);
            AddNewApplicationCommand = new DelegateCommand(CanAddNewApplication, OnAddNewApplication);
            DeleteSelectedApplicationCommand = new DelegateCommand(CanDeleteApplication, OnDelete);
            DeleteSelectedNoteCommand = new DelegateCommand(OnDeleteNote);
            CopyNoteCommand = new DelegateCommand(OnCopyNote);
        }

        private void OnCopyNote(object notUsed)
        {
            Clipboard.SetText(SelectedNote.NoteText);
        }

        private void InitializeNotes()
        {
            var notes = JsonService.LoadAllNotes();
            if (notes == null)
            {
                Notes = new ObservableCollection<Note>();
            }
            else
            {
                Notes = new ObservableCollection<Note>(notes);
            }
        }

        private void AddNewNote(object notUsed)
        {
            try
            {
                if (!string.IsNullOrEmpty(NewNote.NoteText))
                {
                    Notes.Add(new Note() { NoteText = NewNote.NoteText });
                    JsonService.SaveNotes(Notes.ToList<Note>());
                    NewNote.NoteText = null;
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
        }

        private void OnDeleteNote(object note)
        {
            try
            {
                Notes.Remove((Note)note);
                JsonService.SaveNotes(Notes.ToList<Note>());
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
        }

        private void OnDelete(object notUsed)
        {
            try
            {
                Applications.Remove(SelectedApplication);
                JsonService.Save(Applications.ToList<ApplicationDetail>());
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
        }

        private bool CanDeleteApplication()
        {
            return true;
        }

        private void InitializeApplications()
        {
            var apps = JsonService.LoadAllApps();
            if (apps == null)
            {
                Applications = new ObservableCollection<ApplicationDetail>();
            }
            else
            {
                Applications = new ObservableCollection<ApplicationDetail>(apps);
            }
        }

        private bool CanAddNewApplication()
        {
            return canAddNewApplication;
        }

        private void OnAddNewApplication(object notUsed)
        {
            try
            {
                canAddNewApplication = false;
                var addNewDialog = new AddNewApplicationWindow();
                addNewDialog.DataContext = new AddApplicationViewModel(this);
                addNewDialog.ShowDialog();
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
            finally
            {
                canAddNewApplication = true;
            }
        }

        private void OnLaunchSelectedApplication(object notUsed)
        {
            if (SelectedApplication == null)
            {
                logger.LogError("Invalid application");
                return;
            }

            ApplicationLauncher.Launch(SelectedApplication);

        }



        public ApplicationDetail SelectedApplication { get; set; }

        private Note newNote = new Note();

        public Note NewNote
        {
            get { return newNote; }
            set { newNote = value; }
        }


        public Note SelectedNote { get; set; }

        public ObservableCollection<Note> Notes { get; set; }


        private ObservableCollection<ApplicationDetail> applications;

        public ObservableCollection<ApplicationDetail> Applications
        {
            get { return applications; }
            set
            {
                applications = value;
                OnPropertyChanged();
            }
        }


        public ICommand EnterKeyCommand { get; set; }

        public ICommand AddNewApplicationCommand { get; set; }
        public ICommand CopyNoteCommand { get; set; }
        public ICommand DeleteSelectedNoteCommand { get; set; }
        public ICommand DeleteSelectedApplicationCommand { get; set; }

        public ICommand LaunchSelectedApplicationCommand { get; set; }
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    CommandAction = (x) =>
                    {
                        Application.Current.MainWindow = new AddNewApplicationWindow();
                        Application.Current.MainWindow.DataContext = new AddApplicationViewModel(this);
                        Application.Current.MainWindow.ShowDialog();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = (x) => Application.Current.MainWindow.Close(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }


        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = (x) => Application.Current.Shutdown() };
            }
        }
    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Func<bool> canExecute, Action<object> execute = null)
        {
            CanExecuteFunc = canExecute;
            CommandAction = execute;
        }

        public DelegateCommand(Action<object> execute = null)
        {
            CanExecuteFunc = null;
            CommandAction = execute;
        }
        public DelegateCommand()
        {
        }
        public Action<object> CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter = null)
        {
            CommandAction(parameter);
        }

        public bool CanExecute(object parameter )
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
