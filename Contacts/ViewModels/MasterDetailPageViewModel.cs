using Contacts.Message;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Services.FileStoringService;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Contacts.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService _contactRepository;
        IFileStoringService _storingService;

        ObservableCollection<Models.Contacts> _contacts;
        Models.Contacts _contact;
        Symbol _favoriteSymbol = Symbol.Favorite;

        DelegateCommand _makeFavoriteCommand;
        DelegateCommand _deleteContactCommand;
        DelegateCommand _goToSettingsCommand;
        DelegateCommand _editContactCommand;
        DelegateCommand _addContact;
        #endregion

        #region Contructors

        public MasterDetailPageViewModel(IFileStoringService fileStoringService, IContactRepositoryService contactRepositoryService)
        {
            _contactRepository = contactRepositoryService;
            _storingService = fileStoringService;
            _addContact = new DelegateCommand(AddExecute);
            _makeFavoriteCommand = new DelegateCommand(MakeFavoriteExecute, CanMakeFavoriteExcute);
            _deleteContactCommand = new DelegateCommand(DeleteExecute, CanDeleteExecute);
            _goToSettingsCommand = new DelegateCommand(GoToSettingsExecute);
            _editContactCommand = new DelegateCommand(ExecuteEdit, CanEditExecute);
        }
        #endregion

        #region Page navigation event
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var list = await _contactRepository.GetAllAsync();
            _contacts = new ObservableCollection<Models.Contacts>(list.OrderBy(u => u.LastName));

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
            ContentDialogResult result = await GetDialogResult();

            if (result != ContentDialogResult.Primary) return;

            StorageFile imageFileToDelete = await ApplicationData.Current.LocalFolder.TryGetItemAsync(SelectedContact.ID) as StorageFile;

            if (imageFileToDelete != null)
                await imageFileToDelete.DeleteAsync();

            FavoriteSymbol = Symbol.Favorite;

            await _contactRepository.DeleteAsync(SelectedContact.ID);
        }

        private async Task<ContentDialogResult> GetDialogResult()
        {
            ContentDialog OkCancelDialog = new ContentDialog()
            {
                Title = $"Deleting {SelectedContact.FirstName} {SelectedContact.LastName}!",
                Content = "If agree press OK, otherwise Cancel",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };
            return await OkCancelDialog.ShowAsync();
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
            get { return _editContactCommand ?? new DelegateCommand(ExecuteEdit, CanEditExecute); }
        }

        private bool CanEditExecute() => this.SelectedContact == null ? false : true;

        private async void ExecuteEdit() => await NavigationService.NavigateAsync(typeof(Views.AddEditPage), SelectedContact);
        #endregion

        #region MakeFavoriteCommand
        public DelegateCommand MakeFavortieContact
        {
            get { return _makeFavoriteCommand ?? new DelegateCommand(MakeFavoriteExecute, CanMakeFavoriteExcute); }
        }

        private bool CanMakeFavoriteExcute() => this.SelectedContact == null ? false : true;

        private async void MakeFavoriteExecute()
        {
            FavoriteSymbol = SelectedContact.IsFavorite == 1 ? Symbol.UnFavorite : Symbol.Favorite;

            SelectedContact.IsFavorite = SelectedContact.IsFavorite == 1 ? 0 : 1;

            await _contactRepository.UpdateAsync(SelectedContact);
        }
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
                MakeFavortieContact.RaiseCanExecuteChanged();

                if (SelectedContact != null)
                    FavoriteSymbol = (SelectedContact.IsFavorite == 1) ? Symbol.Favorite : Symbol.UnFavorite;
            }
        }

        public Symbol FavoriteSymbol
        {
            get { return _favoriteSymbol; }
            set { Set(ref _favoriteSymbol, value); }
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
                //case CRUD.Update:
                //    SelectedContact.IsFavorite = SelectedContact.IsFavorite == 1 ? 0 : 1;
                //    break;
            }
        }
        #endregion
    }
}
