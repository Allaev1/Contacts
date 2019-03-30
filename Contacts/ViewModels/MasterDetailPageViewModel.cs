using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Message;
using Template10.Mvvm;
using Contacts.Services.ContactsRepositoryService;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Data;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService _contactRepository;

        ObservableCollection<Models.Contacts> _contacts;
        Models.Contacts _contact;

        DelegateCommand _deleteContactCommand;
        DelegateCommand _goToSettingsCommand;
        DelegateCommand _editContactCommand;
        DelegateCommand _addContact;
        #endregion

        #region Contructors

        public MasterDetailPageViewModel()
        {
            _contactRepository = ContactDBService.Instance;
            _deleteContactCommand = new DelegateCommand(DeleteExecute, CanDeleteExecute);
            _goToSettingsCommand = new DelegateCommand(GoToSettingsExecute);

        }

        #endregion

        #region Page navigation event
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var list = await _contactRepository.GetAllAsync();
            _contacts = new ObservableCollection<Models.Contacts>(list);

            Messenger.Default.Register<OperationResultMessage>(this, (message) => HandlePersonsChangedMessage(message));
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            Messenger.Default.Unregister<OperationResultMessage>(this);

            return Task.CompletedTask;
        }
        #endregion

        #region PrimaryCommands
        #region DeleteCommand
        public DelegateCommand DeleteContact
        {
            get { return _deleteContactCommand ?? new DelegateCommand(DeleteExecute, CanDeleteExecute); }
        }

        private bool CanDeleteExecute() => this.SelectedContact == null ? false : true;

        private async void DeleteExecute()
        {
            await ShowDialog();

            await _contactRepository.DeleteAsync(SelectedContact.ID);
        }

        private async Task ShowDialog()
        {
            ContentDialog OkCancelDialog = new ContentDialog()
            {
                Title = $"Deleting {SelectedContact.FirstName} {SelectedContact.LastName}!",
                Content = "If agree press OK, otherwise Cancel",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await OkCancelDialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
        }
        #endregion

        #region AddCommand
        public DelegateCommand AddContact
        {
            get { return _addContact ?? new DelegateCommand(AddExecute); }
        }

        private void AddExecute() =>
            NavigationService.Navigate(typeof(Views.AddEditPage));

        #endregion

        #region EditCommand
        public DelegateCommand EditContact
        {
            get { return _editContactCommand ?? new DelegateCommand(ExecuteEdit, CanExecuteDelete); }
        }

        private bool CanExecuteDelete() => this.SelectedContact == null ? false : true;

        private void ExecuteEdit() => NavigationService.Navigate(typeof(Views.AddEditPage), SelectedContact);
        #endregion

        #endregion

        #region SecondaryCommands 

        #region GoToSetting
        public DelegateCommand GoToSettings
        {
            get { return _goToSettingsCommand ?? new DelegateCommand(GoToSettingsExecute); }
        }

        private void GoToSettingsExecute() => NavigationService.Navigate(typeof(Views.SettingsPage));
        #endregion

        #endregion

        #region Bindable properties
        public ObservableCollection<Models.Contacts> Contacts
        {
            get { return _contacts; }
            set { Set(ref _contacts, value); }
        }

        public Models.Contacts SelectedContact
        {
            get { return _contact; }
            set
            {
                Set(ref _contact, value);
                DeleteContact.RaiseCanExecuteChanged();
                EditContact.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Message handler
        private void HandlePersonsChangedMessage(OperationResultMessage message)
        {
            switch (message.Operation)
            {
                case CRUD.Delete:
                    Contacts.Remove(SelectedContact);
                    break;
            }
        }
        #endregion
    }
}
