using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Message;
using Template10.Mvvm;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Models;
using Template10.Services.NavigationService;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace Contacts.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService _contactRepository;
        ObservableCollection<Models.Contacts> _contacts;
        Models.Contacts _contact;
        DelegateCommand _deleteCommand;
        #endregion

        #region Contructors
        public MasterDetailPageViewModel(IContactRepositoryService contactRepository)
        {
            _contactRepository = contactRepository;
            _deleteCommand = new DelegateCommand(DeleteExecute, CanDeleteExecute);
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

        #region Commands

        #region DeleteCommand
        public DelegateCommand DeleteContact
        {
            get { return _deleteCommand ?? new DelegateCommand(DeleteExecute, CanDeleteExecute); }
        }

        private bool CanDeleteExecute() => this.SelectedContact == null ? false : true;

        private async void DeleteExecute()
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

            await _contactRepository.DeleteAsync(SelectedContact.ID);
        }
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
