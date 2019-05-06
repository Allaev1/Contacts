using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Contacts.Services.ContactsRepositoryService;

namespace Contacts.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService contactRepository;

        ObservableCollection<Models.Contacts> _favoriteContacts;
        #endregion

        #region Constructors
        public FavoritePageViewModel(IContactRepositoryService contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        #endregion

        #region Navigation events
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var listOfFavorites = await contactRepository.GetAllFavoritesAsync();
            FavoriteContacts = new ObservableCollection<Models.Contacts>(listOfFavorites);
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Models.Contacts> FavoriteContacts
        {
            set { Set(ref _favoriteContacts, value); }
            get { return _favoriteContacts; }
        }
        #endregion
    }
}
