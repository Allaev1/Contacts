using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template10.Services.NavigationService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Contacts.Views;
using Template10.Common;
using Template10.Mvvm;

namespace Contacts.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        INavigationService navigationService;
        public ShellViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
        }

        #region Bindable properties
        public List<NavigationViewItem> Items
        {
            get
            {
                return new List<NavigationViewItem>()
                {
                    new NavigationViewItem() { Content="Contacts",Tag="MasterDetailPage"},
                    new NavigationViewItem() { Content="Favorites",Tag="FavoritesPage" }
                };
            }
        }
        #endregion

        #region Commands
        DelegateCommand<NavigationView> _navigateTo;
        public DelegateCommand<NavigationView> NavigateTo
        {
            get { return _navigateTo ?? new DelegateCommand<NavigationView>(ExecuteNavigateTo); }
        }

        private void ExecuteNavigateTo(NavigationView navigationView)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)navigationView.SelectedItem;

            var tag = selectedItem.Tag;
            var typeString = "Contacts.Views." + tag.ToString();

            navigationService.Navigate(Type.GetType(typeString));
        }
        #endregion
    }
}
