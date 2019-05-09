using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Services.DialogService;
using Windows.UI.Xaml.Controls;

namespace Contacts.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService contactRepository;
        IDialogService dialogService;

        Models.Contacts _selectedContact;
        ObservableCollection<Models.Contacts> _favoriteContacts;

        DelegateCommand _showContentDialog;
        #endregion

        #region Constructors
        public FavoritePageViewModel(IContactRepositoryService contactRepository,IDialogService dialogService)
        {
            this.contactRepository = contactRepository;
            this.dialogService = dialogService;
            _showContentDialog = new DelegateCommand(ExecuteShowContentDialog);
        }
        #endregion

        #region Navigation events
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var listOfFavorites = await contactRepository.GetAllFavoritesAsync();
            FavoriteContacts = new ObservableCollection<Models.Contacts>(listOfFavorites.OrderBy(a => a.ID));
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Models.Contacts> FavoriteContacts
        {
            set { Set(ref _favoriteContacts, value); }
            get { return _favoriteContacts; }
        }

        public Models.Contacts SelectedContact
        {
            set { Set(ref _selectedContact, value); }
            get { return _selectedContact; }
        }
        #endregion

        #region Commands

        #region ShowContentDialogCommand
        public DelegateCommand ShowContentDialog
        {
            get { return _showContentDialog ?? new DelegateCommand(ExecuteShowContentDialog); }
        }

        public async void ExecuteShowContentDialog()
        {
            ContentDialogResult result = await dialogService.ShowDialogAsync(SelectedContact);

            if(result == ContentDialogResult.Primary)
            {
                if(SelectedContact.IsFavorite == 0)
                {
                    FavoriteContacts.Remove(SelectedContact);
                }
            }
        }
        #endregion

        #endregion

    }
}
